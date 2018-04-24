using System.Windows.Forms;

namespace nasbench
{
    public partial class BenchmarkProgressDialog : Form
    {
        public bool AllowClosing;

        public bool SubProgressEnabled
        {
            get { return subProgressBar.Visible; }
            set {subProgressBar.Visible = subProgressLabel.Visible = value; }
        }

        public ProgressBar MainProgressBar
        {
            get { return mainProgressBar; }
        }

        public string MainProgressLabel
        {
            get { return mainProgressLabel.Text; }
            set { mainProgressLabel.Text = value; }
        }

        public ProgressBar SubProgressBar
        {
            get { return subProgressBar; }
        }

        public string SubProgressLabel
        {
            get { return subProgressLabel.Text; }
            set { subProgressLabel.Text = value; }
        }

        public BenchmarkProgressDialog()
        {
            InitializeComponent();

            FormClosing += new FormClosingEventHandler((sender, args) =>
            {
                if (!AllowClosing) args.Cancel = true;
            });
        }
    }
}
