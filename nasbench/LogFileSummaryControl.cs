using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace nasbench
{
    public partial class LogFileSummaryControl : UserControl
    {
        public LogFileSummaryControl()
        {
            InitializeComponent();
        }

        public void RebuildView(IList<LogFile.Entry> entries, IList<Performance.Group> groups)
        {
            LogFileViewerForm viewer = (LogFileViewerForm)FindForm();

            flowPanel.Controls.Clear();

            foreach(Performance.Group group in groups)
            {
                LogFileSummaryBoxControl box = new LogFileSummaryBoxControl();

                box.Title = group.FileCount + " x " + Units.FormatSize(group.FileSize);
                box.Details = Units.FormatRate(group.TransmissionRate(true, true));

                box.SetLink("Details", () =>
                {
                    viewer.FileTree.SelectGroup(group);
                    viewer.ShowFileTree();
                });

                if(flowPanel.Controls.Count % 2 == 0)
                {
                    box.BackColor = Color.LightGreen;
                }
                else
                {
                    box.BackColor = Color.LightBlue;
                }

                flowPanel.Controls.Add(box);
            }

            List<LogFile.Entry> generatedFiles;
            generatedFiles = new List<LogFile.Entry>();
            List<LogFile.Entry> verifyOkay, verifyFailed;
            verifyOkay = new List<LogFile.Entry>();
            verifyFailed = new List<LogFile.Entry>();

            foreach(LogFile.Entry entry in entries)
            {
                if(entry.Tag == LogFile.Tag.VERIFY_OKAY)
                {
                    verifyOkay.Add((LogFile.VerifyFileEntry)entry);
                }
                else if(entry.Tag == LogFile.Tag.VERIFY_FAIL)
                {
                    verifyFailed.Add((LogFile.VerifyFileEntry)entry);
                }
                else if(entry.Tag == LogFile.Tag.GENERATE_FILE_DONE)
                {
                    generatedFiles.Add((LogFile.GenerateFileEntry)entry);
                }
            }

            if (verifyOkay.Count > 0)
            {
                LogFileSummaryBoxControl box = new LogFileSummaryBoxControl();
                box.Title = verifyOkay.Count + " Datei(en) verifiziert";
                box.Details = "Der Inhalt von " + verifyOkay.Count + " Datei(en) wurde erfolgreich verifiziert.";
                box.BackColor = Color.LightGreen;

                box.SetLink("Log-Einträge", () =>
                {
                    LogFileViewerForm form = (LogFileViewerForm)FindForm();
                    form.ShowEvents();
                    form.LogFileEvents.ClearSelection();
                    form.LogFileEvents.SelectEvents(verifyOkay);
                });

                flowPanel.Controls.Add(box);
            }

            if (verifyFailed.Count > 0)
            {
                LogFileSummaryBoxControl box = new LogFileSummaryBoxControl();
                box.Title = verifyFailed.Count + " Datei(en) fehlerhaft";
                box.Details = "Der Inhalt von " + verifyFailed.Count + " Datei(en) ist fehlerhaft.";
                box.BackColor = Color.Tomato;

                box.SetLink("Log-Einträge", () =>
                {
                    LogFileViewerForm form = (LogFileViewerForm)FindForm();
                    form.ShowEvents();
                    form.LogFileEvents.ClearSelection();
                    form.LogFileEvents.SelectEvents(verifyFailed);
                });

                flowPanel.Controls.Add(box);
            }

            if(generatedFiles.Count > 0)
            {
                LogFileSummaryBoxControl box = new LogFileSummaryBoxControl();
                box.Title = generatedFiles.Count + " Datei(en) erzeugt";
                box.Details = generatedFiles.Count + " Datei(en) wurde(n) erfolgreich erzeugt.";
                box.BackColor = Color.LightGreen;

                box.SetLink("Log-Einträge", () =>
                {
                    LogFileViewerForm form = (LogFileViewerForm)FindForm();
                    form.ShowEvents();
                    form.LogFileEvents.ClearSelection();
                    form.LogFileEvents.SelectEvents(generatedFiles);
                });

                flowPanel.Controls.Add(box);
            }
        }
    }
}
