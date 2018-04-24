using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Threading;

namespace nasbench
{
    public partial class MainForm : Form
    {
        private ConnectionDialog connectionDialog;

        public enum VerificationMode
        {
            NONE,
            SIMPLE,
            BOTH
        }

        public MainForm()
        {
            InitializeComponent();

            // Event handler for the start button
            startButton.Click += new EventHandler((object sender, EventArgs args) => StartTest());

            // Remove empty file group rows when possible. The RowLeave event would be better, but
            // it does not permit concurrent modification of the grid.
            fileGroupGrid.Leave += new EventHandler((object sender, EventArgs args) =>
            {
                for (int i = 0; i < fileGroupGrid.RowCount - 1; i++)
                {
                    DataGridViewRow row = fileGroupGrid.Rows[i];
                    if(WinForms.RowIsEmpty(row))
                    {
                        fileGroupGrid.Rows.RemoveAt(i--);
                    }
                }
            });

            // Disable sorting in the file group table
            foreach (DataGridViewColumn column in fileGroupGrid.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            // Add default file groups
            fileGroupGrid.Rows.Add("1000", "256K");
            fileGroupGrid.Rows.Add("100", "2M");
            fileGroupGrid.Rows.Add("10", "400M");

            // Add verification modes
            IDictionary<VerificationMode, string> vModes = new Dictionary<VerificationMode, string>()
            {
                { VerificationMode.NONE, "Nein" },
                { VerificationMode.SIMPLE, "Einfach" },
                { VerificationMode.BOTH, "Doppelt" }
            };
            verifyModeComboBox.DataSource = new BindingSource(vModes, null);
            verifyModeComboBox.SelectedIndexChanged += new EventHandler(UpdateVerificationModeDescription);
            verifyModeComboBox.SelectedIndex = 1;
            verifyModeComboBox.DisplayMember = "Value";
            verifyModeComboBox.ValueMember = "Key";

            // Enable browsing
            targetDirInput.Browse += new EventHandler(BrowseForTargetDirectory);
            testFileDirInput.Browse += new EventHandler(BrowseForTargetDirectory);
            logDirInput.Browse += new EventHandler(BrowseForTargetDirectory);

            // We will use a single instance of the connection dialog to preserve its state
            connectionDialog = new ConnectionDialog();
            nasConnToolStripMenuItem.Click += new EventHandler((sender, args) => ShowConnectionDialog());

            logFileToolStripMenuItem.Click += new EventHandler((sender, args) =>
            {
                LogFileViewerForm logViewer = new LogFileViewerForm();
                AppContext.Instance.Register(logViewer);
                logViewer.Show();
                logViewer.OpenFile();
            });

            infoToolStripMenuItem.Click += new EventHandler((sender, args) => new AboutDialog().ShowDialog(this));

            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = 60000;
            timer.Tick += new EventHandler((sender, args) => logBox.Log("Heartbeat"));
            timer.Start();
        }

        private void UpdateVerificationModeDescription(object sender, EventArgs e)
        {
            VerificationMode verificationMode = ((KeyValuePair<VerificationMode, string>)verifyModeComboBox.SelectedItem).Key;
            switch (verificationMode)
            {
                case VerificationMode.NONE:
                    verifyModeDescription.Text = "Gesendete und empfangene Dateien werden nicht überprüft.";
                    break;
                case VerificationMode.SIMPLE:
                    verifyModeDescription.Text = "Die empfangenen Dateien werden überprüft. Diese Einstellung ist empfohlen.";
                    break;
                case VerificationMode.BOTH:
                    verifyModeDescription.Text = "Sowohl die gesendeten als auch die empfangenen Dateien werden überprüft.";
                    break;
            }
        }

        private void BrowseForTargetDirectory(object sender, EventArgs args)
        {
            PathTextBox input = (PathTextBox)sender;

            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (String.IsNullOrEmpty(input.Path))
            {
                dialog.RootFolder = Environment.SpecialFolder.MyComputer;
            }
            else
            {
                dialog.SelectedPath = input.Path;
            }

            if (DialogResult.OK == dialog.ShowDialog(this))
            {
                input.Path = dialog.SelectedPath;
            }
        }

        private void ShowConnectionDialog()
        {
            connectionDialog.ShowDialog(this);
            if(connectionDialog.LastConnectedDrive != null)
            {
                DialogResult r;
                r = MessageBox.Show(this, "Soll die zuletzt hergestellte NAS-Verbindung (Laufwerk " + connectionDialog.LastConnectedDrive + ") als Zielort verwendet werden?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if(r == DialogResult.Yes)
                {
                    targetDirInput.Path = connectionDialog.LastConnectedDrive + ":\\";
                }
                connectionDialog.LastConnectedDrive = null;
            }
        }

        private void StartTest()
        {
            // NAS directory
            string targetDir = targetDirInput.Path;
            if(!Path.IsPathRooted(targetDir))
            {
                MessageBox.Show(this, "Zielverzeichnis muss absolut sein.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Test file directory
            string testFileDir = testFileDirInput.Path;
            if(!Path.IsPathRooted(testFileDir))
            {
                MessageBox.Show(this, "Verzeichnis für Testdateien muss absolut sein.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Log file directory
            string logDir = logDirInput.Path;
            if(!Path.IsPathRooted(logDir))
            {
                MessageBox.Show(this, "Verzeichnis für Logdateien muss absolut sein.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (Path.GetPathRoot(targetDir) == Path.GetPathRoot(testFileDir))
            {
                DialogResult r;
                r = MessageBox.Show(this, "Quell- und Zielverzeichnis befinden sich möglicherweise auf demselben Laufwerk. Möchten Sie dennoch fortfahren?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (r != DialogResult.Yes) return;
            }

            // Remember the log directory so the log viewer can use it
            AppContext.Instance.CurrentLogFolder = logDir;

            // Whether to allow the system to buffer file contents interally
            bool allowSystemBuffer = false;

            // Whether to delete target files before copying
            bool deleteTargetFiles = true;

            // If set to true, only one test file will be created per group locally.
            // This allows to use tiny RAM drives instead of multiple gigs.
            bool singleFilePerGroup = singleFilePerGroupCheckbox.Checked;

            // Parse file groups
            int nAllFiles = 0;
            List<string> fileGroupList = new List<string>();
            for (int i = 0; i < fileGroupGrid.RowCount; i++)
            {
                DataGridViewRow row = fileGroupGrid.Rows[i];
                if (!WinForms.RowIsEmpty(row))
                {
                    string count = (string)row.Cells[0].Value, size = (string)row.Cells[1].Value;
                    fileGroupList.Add(count + "x" + size);
                    nAllFiles += int.Parse(count);
                }
            }

            // Fail if no file groups were specified
            if(fileGroupList.Count == 0)
            {
                MessageBox.Show(this, "Es wurden keine Testdateien festgelegt.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Convert file groups to an array
            string[] fileGroups = fileGroupList.ToArray();

            // Calculate number of local test files
            int nLocalTestFiles = (singleFilePerGroup ? fileGroups.Length : nAllFiles);

            // See below for all three verification modes.
            VerificationMode verificationMode = ((KeyValuePair<VerificationMode, string>)verifyModeComboBox.SelectedItem).Key;

            // Ask for a base name for the log files
            LogFileNameInputDialog lfnDialog = new LogFileNameInputDialog();
            lfnDialog.LogFileName = DateTime.Now.ToString("yyyyMMddHHmmss");
            if(lfnDialog.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            string logFileName = lfnDialog.LogFileName;

            // Determine path
            string assemblyLocation = Assembly.GetExecutingAssembly().Location;
            string assemblyDir = Path.GetDirectoryName(assemblyLocation);
            string nbPath = Path.Combine(assemblyDir, "fileperf2.exe");

            while(!File.Exists(nbPath))
            {
                DialogResult r;
                r = MessageBox.Show(this, "Das Programm fileperf2.exe wurde nicht gefunden. Bitte stellen Sie sicher, dass es sich unter " + nbPath + " befindet.", "", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                if (r != DialogResult.Retry) return;
            }

            // Create folders if necessary
            try
            {
                if (!Directory.Exists(targetDir))
                {
                    logBox.Log("Erstelle Verzeichnis auf NAS: " + targetDir);
                    Directory.CreateDirectory(targetDir);
                }
                if (!Directory.Exists(testFileDir))
                {
                    logBox.Log("Erstelle Verzeichnis für lokale Testdateien: " + testFileDir);
                    Directory.CreateDirectory(testFileDir);
                }
                if (!Directory.Exists(logDir))
                {
                    logBox.Log("Erstelle Verzeichnis für Logdateien: " + logDir);
                    Directory.CreateDirectory(logDir);
                }
            }
            catch (IOException e)
            {
                logBox.Log("Fehler: " + e.Message);
                MessageBox.Show(this, "Mindestens ein Verzeichnis existiert nicht und kann nicht angelegt werden: " + e.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Interface to the native binary. All tests will be performed in a separate process
            // to prevent any performance loss.
            NativeBenchmark benchmark = new NativeBenchmark(nbPath);
            benchmark.ParseLog = true;

            // Disable user inputs (not really necessary as long as the progress dialog is visible)
            setAllInputsEnabled(false);

            BenchmarkProgressDialog progressDialog = new BenchmarkProgressDialog();

            // Perform everything else in a new thread to prevent blocking the UI.
            // Note that we show the progress dialog *after* starting the thread as this
            // thread will actually be blocked until the progress dialog is closed.
            new Thread(() =>
            {
                // Mark this thread as a background thread
                Thread.CurrentThread.Name = "TestRunner";
                Thread.CurrentThread.IsBackground = true;

                // We are parsing and writing the log at the same time. This
                // object is the current event handler so it can be removed
                // after the process has finished.
                NativeBenchmark.LogEntryEventHandler eventHandler;

                try
                {
                    var logFiles = new List<KeyValuePair<string, string>>();

                    WinForms.InvokeIfRequired(this, () =>
                    {
                        progressDialog.Text = "Überprüfe lokale Testdateien";
                        progressDialog.SubProgressEnabled = false;
                        // TODO: Add real progress calculation
                        progressDialog.MainProgressLabel = "Überprüfe " + nLocalTestFiles + " Testdateien...";
                        progressDialog.MainProgressBar.Style = ProgressBarStyle.Marquee;
                    });

                    logBox.Log("Verifiziere lokale Testdateien...");

                    bool testFilesOkay = true;
                    eventHandler = new NativeBenchmark.LogEntryEventHandler((sender, args) =>
                    {
                        switch (args.Entry.Tag)
                        {
                            case LogFile.Tag.VERIFY_INIT_DONE:
                                logBox.Log("fileperf2: Überprüfung lokaler Testdateien begonnen.");
                                break;
                            case LogFile.Tag.VERIFY_FAIL:
                                testFilesOkay = false;
                                break;
                        }
                    });

                    benchmark.LogEntryParsed += eventHandler;
                    benchmark.LogFilePath = Path.Combine(logDir, logFileName + ".prever.log");
                    logFiles.Add(new KeyValuePair<string, string>("Überprüfung existierender Testdateien", benchmark.LogFilePath));
                    int exitCode = benchmark.Verify(testFileDir, fileGroups, singleFilePerGroup);
                    benchmark.LogEntryParsed -= eventHandler;

                    if (exitCode != 0 || !testFilesOkay)
                    {
                        logBox.Log("Eine oder mehrere Testdateien fehlen oder wurden verändert. Erzeuge neue Testdateien.");
                        WinForms.InvokeIfRequired(this, () =>
                        {
                            progressDialog.Text = "Erzeuge Testdateien";
                            progressDialog.SubProgressEnabled = false;
                            // TODO: Add real progress calculation
                            progressDialog.MainProgressLabel = "Erzeuge " + nLocalTestFiles + " Testdateien...";
                            progressDialog.MainProgressBar.Style = ProgressBarStyle.Marquee;
                        });

                        eventHandler = new NativeBenchmark.LogEntryEventHandler((sender, args) =>
                        {
                            switch (args.Entry.Tag)
                            {
                                case LogFile.Tag.GENERATE_INIT_DONE:
                                    logBox.Log("fileperf2: Erzeugung der Testdateien begonnen.");
                                    break;
                                case LogFile.Tag.GENERATE_FILE_DONE:
                                    LogFile.GenerateFileEntry a = (LogFile.GenerateFileEntry)args.Entry;
                                    logBox.Log("fileperf2: Testdatei erstellt: " + a.Path);
                                    break;
                            }
                        });

                        benchmark.LogEntryParsed += eventHandler;
                        benchmark.LogFilePath = Path.Combine(logDir, logFileName + ".gen.log");
                        logFiles.Add(new KeyValuePair<string, string>("Erzeugung der Testdateien", benchmark.LogFilePath));
                        exitCode = benchmark.Generate(testFileDir, fileGroups, singleFilePerGroup, true);
                        benchmark.LogEntryParsed -= eventHandler;

                        if (exitCode != 0)
                        {
                            // TODO: Show an error: Retry, cancel
                            logBox.Log("Fehler beim Erzeugen der Testdateien.");
                            return;
                        }
                    }

                    WinForms.InvokeIfRequired(this, () =>
                    {
                        progressDialog.Text = "Schreibe Dateien";
                        progressDialog.SubProgressEnabled = false;
                        // TODO: Add real progress calculation
                        progressDialog.MainProgressLabel = "Schreibe " + nAllFiles + " Testdateien...";
                        progressDialog.MainProgressBar.Style = ProgressBarStyle.Marquee;
                    });

                    logBox.Log("Schreibe Dateien...");

                    eventHandler = new NativeBenchmark.LogEntryEventHandler((sender, args) =>
                    {
                        LogFile.CopyGroupEntry groupEntry;
                        LogFile.CopyFileEntry fileEntry;
                        switch (args.Entry.Tag)
                        {
                            case LogFile.Tag.COPY_GROUP_START:
                                groupEntry = (LogFile.CopyGroupEntry)args.Entry;
                                logBox.Log("fileperf2: Gruppe " + (groupEntry.GroupIndex + 1) + " von " + fileGroups.Count() + " begonnen.");
                                break;
                            case LogFile.Tag.COPY_GROUP_END:
                                groupEntry = (LogFile.CopyGroupEntry)args.Entry;
                                logBox.Log("fileperf2: Gruppe " + (groupEntry.GroupIndex + 1) + " von " + fileGroups.Count() + " fertig.");
                                break;
                            case LogFile.Tag.COPY_FILE_START:
                                fileEntry = (LogFile.CopyFileEntry)args.Entry;
                                logBox.Log("fileperf2: Schreiben von Datei " + (fileEntry.FileIndex + 1) + " begonnen: " + fileEntry.TargetPath);
                                break;
                            case LogFile.Tag.COPY_FILE_END:
                                fileEntry = (LogFile.CopyFileEntry)args.Entry;
                                logBox.Log("fileperf2: Schreiben von Datei " + (fileEntry.FileIndex + 1) + " fertig: " + fileEntry.TargetPath);
                                break;
                        }
                    });

                    benchmark.LogEntryParsed += eventHandler;
                    benchmark.LogFilePath = Path.Combine(logDir, logFileName + ".write.log");
                    logFiles.Add(new KeyValuePair<string, string>("Schreiben (auf NAS)", benchmark.LogFilePath));
                    exitCode = benchmark.Copy(testFileDir, targetDir, fileGroups, singleFilePerGroup, false, deleteTargetFiles, allowSystemBuffer);
                    benchmark.LogEntryParsed -= eventHandler;

                    if (exitCode != 0)
                    {
                        // TODO: Show an error: Retry, ignore, cancel
                        logBox.Log("Fehler beim Schreiben.");
                        return;
                    }

                    WinForms.InvokeIfRequired(this, () =>
                    {
                        progressDialog.Text = "Lese Dateien";
                        progressDialog.SubProgressEnabled = false;
                        // TODO: Add real progress calculation
                        progressDialog.MainProgressLabel = "Lese " + nAllFiles + " Testdateien...";
                        progressDialog.MainProgressBar.Style = ProgressBarStyle.Marquee;
                    });

                    logBox.Log("Lese Dateien...");

                    eventHandler = new NativeBenchmark.LogEntryEventHandler((sender, args) =>
                    {
                        LogFile.CopyGroupEntry groupEntry;
                        LogFile.CopyFileEntry fileEntry;
                        switch (args.Entry.Tag)
                        {
                            case LogFile.Tag.COPY_GROUP_START:
                                groupEntry = (LogFile.CopyGroupEntry)args.Entry;
                                logBox.Log("fileperf2: Gruppe " + (groupEntry.GroupIndex + 1) + " von " + fileGroups.Count() + " begonnen.");
                                break;
                            case LogFile.Tag.COPY_GROUP_END:
                                groupEntry = (LogFile.CopyGroupEntry)args.Entry;
                                logBox.Log("fileperf2: Gruppe " + (groupEntry.GroupIndex + 1) + " von " + fileGroups.Count() + " fertig.");
                                break;
                            case LogFile.Tag.COPY_FILE_START:
                                fileEntry = (LogFile.CopyFileEntry)args.Entry;
                                logBox.Log("fileperf2: Lesen von Datei " + (fileEntry.FileIndex + 1) + " begonnen: " + fileEntry.SourcePath);
                                break;
                            case LogFile.Tag.COPY_FILE_END:
                                fileEntry = (LogFile.CopyFileEntry)args.Entry;
                                logBox.Log("fileperf2: Lesen von Datei " + (fileEntry.FileIndex + 1) + " fertig: " + fileEntry.SourcePath);
                                break;
                        }
                    });

                    benchmark.LogEntryParsed += eventHandler;
                    benchmark.LogFilePath = Path.Combine(logDir, logFileName + ".read.log");
                    logFiles.Add(new KeyValuePair<string, string>("Lesen (von NAS)", benchmark.LogFilePath));
                    exitCode = benchmark.Copy(targetDir, testFileDir, fileGroups, false, singleFilePerGroup, deleteTargetFiles, allowSystemBuffer);
                    benchmark.LogEntryParsed -= eventHandler;

                    if (exitCode != 0)
                    {
                        // TODO: Handle errors better
                        logBox.Log("Fehler beim Lesen");
                        // TODO: Show an error: Retry, ignore, cancel
                        return;
                    }

                    if (verificationMode == VerificationMode.BOTH)
                    {
                        WinForms.InvokeIfRequired(this, () =>
                        {
                            progressDialog.Text = "Überprüfe Dateien auf NAS";
                            progressDialog.SubProgressEnabled = false;
                            // TODO: Add real progress calculation
                            progressDialog.MainProgressLabel = "Überprüfe " + nAllFiles + " Dateien...";
                            progressDialog.MainProgressBar.Style = ProgressBarStyle.Marquee;
                        });

                        logBox.Log("Überprüfe Dateien auf NAS...");

                        eventHandler = new NativeBenchmark.LogEntryEventHandler((sender, args) =>
                        {
                            switch (args.Entry.Tag)
                            {
                                case LogFile.Tag.VERIFY_INIT_DONE:
                                    logBox.Log("fileperf2: Überprüfung gesendeter Testdateien begonnen.");
                                    break;
                                case LogFile.Tag.VERIFY_OKAY:
                                    logBox.Log("fileperf2: Überprüfung erfolgreich: " + ((LogFile.VerifyFileEntry)args.Entry).Path);
                                    break;
                                case LogFile.Tag.VERIFY_FAIL:
                                    testFilesOkay = false;
                                    LogFile.VerifyFailEntry failure = (LogFile.VerifyFailEntry)args.Entry;
                                    logBox.Log("fileperf2: Überprüfung fehlgeschlagen: " + failure.Path + ": " + failure.FailMessage);
                                    break;
                            }
                        });

                        benchmark.LogEntryParsed += eventHandler;
                        benchmark.LogFilePath = Path.Combine(logDir, logFileName + ".vwrite.log");
                        logFiles.Add(new KeyValuePair<string, string>("Überprüfung gesendeter Dateien", benchmark.LogFilePath));
                        exitCode = benchmark.Verify(targetDir, fileGroups, false);
                        benchmark.LogEntryParsed -= eventHandler;

                        if (exitCode != 0)
                        {
                            // TODO: Show an error: Retry, ignore, cancel
                            logBox.Log("Fehler beim Verifizieren");
                        }
                    }

                    if (verificationMode == VerificationMode.BOTH || verificationMode == VerificationMode.SIMPLE)
                    {
                        WinForms.InvokeIfRequired(this, () =>
                        {
                            progressDialog.Text = "Überprüfe lokale Dateien";
                            progressDialog.SubProgressEnabled = false;
                            // TODO: Add real progress calculation
                            progressDialog.MainProgressLabel = "Überprüfe " + nAllFiles + " Dateien...";
                            progressDialog.MainProgressBar.Style = ProgressBarStyle.Marquee;
                        });

                        logBox.Log("Überprüfe lokale Dateien...");

                        eventHandler = new NativeBenchmark.LogEntryEventHandler((sender, args) =>
                        {
                            switch (args.Entry.Tag)
                            {
                                case LogFile.Tag.VERIFY_INIT_DONE:
                                    logBox.Log("fileperf2: Überprüfung empfangener Testdateien begonnen.");
                                    break;
                                case LogFile.Tag.VERIFY_OKAY:
                                    logBox.Log("fileperf2: Überprüfung erfolgreich: " + ((LogFile.VerifyFileEntry)args.Entry).Path);
                                    break;
                                case LogFile.Tag.VERIFY_FAIL:
                                    testFilesOkay = false;
                                    LogFile.VerifyFailEntry failure = (LogFile.VerifyFailEntry)args.Entry;
                                    logBox.Log("fileperf2: Überprüfung fehlgeschlagen: " + failure.Path + ": " + failure.FailMessage);
                                    break;
                            }
                        });

                        benchmark.LogEntryParsed += eventHandler;
                        benchmark.LogFilePath = Path.Combine(logDir, logFileName + ".vread.log");
                        logFiles.Add(new KeyValuePair<string, string>("Überprüfung empfangener Dateien", benchmark.LogFilePath));
                        exitCode = benchmark.Verify(testFileDir, fileGroups, singleFilePerGroup);
                        benchmark.LogEntryParsed -= eventHandler;

                        if (exitCode != 0)
                        {
                            // TODO: Show an error: Retry, ignore, cancel
                            logBox.Log("Fehler beim Verifizieren");
                        }
                    }

                    WinForms.InvokeIfRequired(this, () =>
                    {
                        progressDialog.AllowClosing = true;
                        progressDialog.FormClosed += new FormClosedEventHandler((sender, e) =>
                        {
                            BeginInvoke((ThreadStart) delegate ()
                            {
                                new BenchmarkDoneDialog(logFiles).ShowDialog(this);
                            });
                        });
                        progressDialog.Close();
                    });
                }
                catch(Exception e)
                {
                    logBox.Log("Interner Fehler: " + e.Message);
                    logBox.Log(e.StackTrace);

                    WinForms.InvokeIfRequired(this, () =>
                    {
                        MessageBox.Show(this, "Ein interner Fehler ist während des Testvorgangs aufgetreten: " + e.Message + "\n" + e.StackTrace, "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    });
                }
                finally
                {
                    WinForms.InvokeIfRequired(this, () =>
                    {
                        setAllInputsEnabled(true);
                        if (progressDialog.Visible)
                        {
                            progressDialog.AllowClosing = true;
                            progressDialog.Close();
                        }
                    });
                }
            }).Start();

            progressDialog.AllowClosing = false;
            progressDialog.ShowDialog(this);
        }

        private void setAllInputsEnabled(bool b)
        {
            dirsGroupBox.Enabled = b;
            filesGroupBox.Enabled = b;
            optionsGroupBox.Enabled = b;
        }
    }
}
