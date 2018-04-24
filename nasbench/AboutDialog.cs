using System.Reflection;
using System.Windows.Forms;

namespace nasbench
{
    public partial class AboutDialog : Form
    {
        public AboutDialog()
        {
            InitializeComponent();

            Assembly assembly = Assembly.GetEntryAssembly();
            versionLabel.Text = "Version " + assembly.GetName().Version;
        }
    }
}
