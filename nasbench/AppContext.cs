using System.Collections.Generic;
using System.Windows.Forms;

namespace nasbench
{
    public class AppContext : ApplicationContext
    {
        private static AppContext instance;

        public static AppContext Instance
        {
            get
            {
                if (instance == null) instance = new AppContext();
                return instance;
            }
        }

        private readonly IList<Form> openForms;
        public string CurrentLogFolder;

        private AppContext()
        {
            openForms = new List<Form>();
            CurrentLogFolder = null;
        }

        public void Register(Form form)
        {
            openForms.Add(form);
            form.FormClosed += new FormClosedEventHandler(FormClosed);
        }

        private void FormClosed(object sender, FormClosedEventArgs args)
        {
            openForms.Remove((Form)sender);
            if(openForms.Count == 0)
            {
                ExitThread();
            }
        }
    }
}
