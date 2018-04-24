using System;
using System.Windows.Forms;

namespace nasbench
{
    public partial class LogBox : UserControl
    {
        private readonly Object dataLock = new Object();
        private string pendingData;

        public LogBox()
        {
            InitializeComponent();

            pendingData = "";
            updateTimer.Tick += OnUpdate;
        }

        private void OnUpdate(object sender, EventArgs e)
        {
            lock (dataLock)
            {
                if (pendingData != "")
                {
                    string d = pendingData;
                    WinForms.InvokeIfRequired(this, () => textBox.AppendText(d));
                    pendingData = "";
                }
            }
        }

        /**
         * Thread-safe logging method, can be called from non-GUI threads.
         */
        public void Log(string text)
        {
            text = "[" + DateTime.Now.ToString("HH:mm:ss") + "] " + text + "\r\n";
            lock (dataLock)
            {
                pendingData += text;
            }
        }
    }
}
