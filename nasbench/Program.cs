using System;
using System.Windows.Forms;

namespace nasbench
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if(args.Length == 0)
            {
                MainForm mainForm = new MainForm();
                mainForm.Show();
                AppContext.Instance.Register(mainForm);
            }
            else
            {
                LogFileViewerForm logForm = new LogFileViewerForm();
                logForm.Show();
                AppContext.Instance.Register(logForm);
                logForm.OpenFile(args[0]);
            }

            Application.Run(AppContext.Instance);
        }
    }
}
