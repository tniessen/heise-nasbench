using System;
using System.Windows.Forms;

namespace nasbench
{
    public partial class LogFileSummaryBoxControl : UserControl
    {
        public string Title
        {
            get { return titleLabel.Text; }
            set { titleLabel.Text = value; }
        }

        public string Details
        {
            get { return detailsLabel.Text; }
            set { detailsLabel.Text = value; }
        }

        public LogFileSummaryBoxControl()
        {
            InitializeComponent();

            linkLabel.Visible = false;
        }

        public void SetLink(string text, Action action)
        {
            linkLabel.Text = text;
            linkLabel.Click += new EventHandler((sender, args) => action());
            linkLabel.Visible = true;
        }
    }
}
