using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace nasbench
{
    public partial class BenchmarkDoneDialog : Form
    {
        public BenchmarkDoneDialog(List<KeyValuePair<string, string>> entries)
        {
            InitializeComponent();

            foreach (KeyValuePair<string, string> entry in entries)
            {
                Console.WriteLine("Label: " + entry.Key);
                LinkLabel ll = new LinkLabel
                {
                    Text = entry.Key,
                    AutoSize = true,
                    Padding = new Padding(0, 5, 0, 5)
                };
                ll.Tag = entry.Value;
                toolTip.SetToolTip(ll, entry.Value);
                ll.LinkClicked += new LinkLabelLinkClickedEventHandler(LinkClicked);
                linkContainer.Controls.Add(ll);
            }
        }

        private void LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string path = (string)(sender as Control).Tag;
            LogFileViewerForm logForm = new LogFileViewerForm();
            logForm.Show();
            AppContext.Instance.Register(logForm);
            logForm.OpenFile(path);
        }
    }
}
