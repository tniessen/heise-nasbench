using System;
using System.Drawing;
using System.Windows.Forms;

namespace nasbench
{
    public partial class PathTextBox : UserControl
    {
        public string Path
        {
            get
            {
                string path = rawPathTextBox.Text;
                return String.IsNullOrEmpty(path) ? null : path;
            }
            set
            {
                rawPathTextBox.Text = value;
            }
        }

        public event EventHandler Browse;

        public PathTextBox()
        {
            InitializeComponent();

            // TODO: Adapt size using docking/anchors
            AdaptSize();
            SizeChanged += new EventHandler((object sender, EventArgs args) => AdaptSize());

            browseButton.Click += new EventHandler((object sender, EventArgs args) => Browse(this, null));
        }

        private void AdaptSize()
        {
            int buttonWidth = browseButton.Size.Width;
            int space = browseButton.Location.X - rawPathTextBox.Size.Width;
            browseButton.Location = new Point(Width - buttonWidth, 0);
            rawPathTextBox.Width = Width - buttonWidth - space;
        }
    }
}
