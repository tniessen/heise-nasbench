using System;
using System.Windows.Forms;

namespace nasbench
{
    public partial class LogFileNameInputDialog : Form
    {
        public string LogFileName
        {
            get { return nameTextBox.Text; }
            set { nameTextBox.Text = value; }
        }

        public LogFileNameInputDialog()
        {
            InitializeComponent();

            nameTextBox.TextChanged += new EventHandler((sender, args) => UpdateUI());
            confirmButton.Click += new EventHandler((sender, args) => SanitizeName());

            UpdateUI();
        }

        private void UpdateUI()
        {
            confirmButton.Enabled = (Sanitize(LogFileName).Length > 0);
        }

        private void SanitizeName()
        {
            LogFileName = Sanitize(LogFileName);
        }

        private string Sanitize(string name)
        {
            return name.Replace("\\", "").Replace("/", "_").Trim();
        }
    }
}
