using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace nasbench
{
    public partial class LogFileViewerForm : Form
    {
        public LogFileViewerForm()
        {
            InitializeComponent();

            newViewerToolStripMenuItem.Click += new EventHandler((sender, args) =>
            {
                LogFileViewerForm newForm = new LogFileViewerForm();
                AppContext.Instance.Register(newForm);
                newForm.Show();
            });

            openToolStripMenuItem.Click += new EventHandler((sender, args) => OpenFile());
            exitToolStripMenuItem.Click += new EventHandler((sender, args) => Close());
            infoToolStripMenuItem.Click += new EventHandler((sender, args) => new AboutDialog().ShowDialog(this));
        }

        public void ShowFileTree()
        {
            mainTabControl.SelectedTab = fileTreeTabPage;
        }

        public void ShowEvents()
        {
            mainTabControl.SelectedTab = eventsTabPage;
        }

        public void OpenFile()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Log files (*.log)|*.log|All files (*.*)|*.*";
            dialog.FilterIndex = 0;
            if (AppContext.Instance.CurrentLogFolder != null)
                dialog.InitialDirectory = AppContext.Instance.CurrentLogFolder;

            if(dialog.ShowDialog() == DialogResult.OK)
            {
                string path = dialog.FileName;
                OpenFile(path);
            }
        }

        public void OpenFile(string path)
        {
            List<LogFile.Entry> entries;
            List<Performance.Group> groups;
            try
            {
                entries = LogFile.ParseFile(path).ToList();
                groups = Performance.ParseLog(entries).ToList();
            }
            catch (Exception e)
            {
                MessageBox.Show(this, e.Message, "Fehler beim Lesen der Datei", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Text = path;
            AppContext.Instance.CurrentLogFolder = Path.GetDirectoryName(path);

            RebuildView(entries, groups);
        }

        private void RebuildView(IList<LogFile.Entry> entries, IList<Performance.Group> groups)
        {
            SummaryPanel.RebuildView(entries, groups);
            LogFileEvents.RebuildView(entries);
            FileTree.RebuildView(groups);
        }
    }
}
