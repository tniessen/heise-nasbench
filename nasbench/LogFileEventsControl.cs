using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace nasbench
{
    public partial class LogFileEventsControl : UserControl
    {
        public LogFileEventsControl()
        {
            InitializeComponent();
        }

        public void ClearSelection()
        {
            eventGridView.ClearSelection();
        }

        public bool SelectEvents(IEnumerable<LogFile.Entry> entries)
        {
            foreach(LogFile.Entry entry in entries)
            {
                if (!SelectEventInternal(entry)) return false;
            }
            eventGridView.FirstDisplayedScrollingRowIndex = eventGridView.SelectedRows[0].Index;
            return true;
        }

        public bool SelectEvent(LogFile.Entry entry)
        {
            bool b = SelectEventInternal(entry);
            eventGridView.FirstDisplayedScrollingRowIndex = eventGridView.SelectedRows[0].Index;
            return b;
        }

        private bool SelectEventInternal(LogFile.Entry entry)
        {
            foreach (DataGridViewRow row in eventGridView.Rows)
            {
                if (entry.Equals(row.Tag))
                {
                    row.Selected = true;
                    return true;
                }
            }
            return false;
        }

        public void RebuildView(IList<LogFile.Entry> entries)
        {
            WinForms.EnableDrawing(this, false);
            eventGridView.Rows.Clear();

            foreach(LogFile.Entry entry in entries)
            {
                string details = null;
                switch(entry.Tag)
                {
                    case LogFile.Tag.GENERATE_INIT_DONE:
                    case LogFile.Tag.VERIFY_INIT_DONE:
                        details = "(no details)";
                        break;
                    case LogFile.Tag.VERIFY_OKAY:
                        LogFile.VerifyFileEntry verifyFileEntry = (LogFile.VerifyFileEntry)entry;
                        details = "Gruppe: " + verifyFileEntry.GroupIndex + ", Datei: " + verifyFileEntry.FileIndex + ", Größe: " + verifyFileEntry.FileSize + ", Pfad: " + verifyFileEntry.Path;
                        break;
                    case LogFile.Tag.VERIFY_FAIL:
                        LogFile.VerifyFailEntry verifyFailEntry = (LogFile.VerifyFailEntry)entry;
                        details = "Gruppe: " + verifyFailEntry.GroupIndex + ", Datei: " + verifyFailEntry.FileIndex + ", Größe: " + verifyFailEntry.FileSize + ", Pfad: " + verifyFailEntry.Path + ", Nachricht: " + verifyFailEntry.FailMessage;
                        break;
                    case LogFile.Tag.GENERATE_FILE_DONE:
                        LogFile.GenerateFileEntry generateFileEntry = (LogFile.GenerateFileEntry)entry;
                        details = "Gruppe: " + generateFileEntry.GroupIndex + ", Datei: " + generateFileEntry.FileIndex + ", Größe: " + generateFileEntry.FileSize + ", Pfad: " + generateFileEntry.Path;
                        break;
                    case LogFile.Tag.COPY_GROUP_START:
                    case LogFile.Tag.COPY_GROUP_END:
                        LogFile.CopyGroupEntry copyGroupEntry = (LogFile.CopyGroupEntry)entry;
                        details = "Gruppe: " + copyGroupEntry.GroupIndex + ", Dateien: " + copyGroupEntry.FileCount + ", Größe: " + copyGroupEntry.FileSize;
                        break;
                    case LogFile.Tag.COPY_FILE_START:
                    case LogFile.Tag.COPY_FILE_END:
                        LogFile.CopyFileEntry copyFileEntry = (LogFile.CopyFileEntry)entry;
                        details = "Gruppe: " + copyFileEntry.GroupIndex + ", Datei: " + copyFileEntry.FileIndex + ", Größe: " + copyFileEntry.FileSize + ", From: " + copyFileEntry.SourcePath + ", To: " + copyFileEntry.TargetPath;
                        break;
                }

                if(details != null)
                {
                    eventGridView.Rows.Add(new string[] {
                        String.Format("{0:0000.000000}", entry.Timestamp / 1000000.0),
                        System.Enum.GetName(typeof(LogFile.Tag), entry.Tag),
                        details
                    });
                    eventGridView.Rows[eventGridView.Rows.Count - 1].Tag = entry;
                }
            }

            WinForms.EnableDrawing(this, true);
        }
    }
}
