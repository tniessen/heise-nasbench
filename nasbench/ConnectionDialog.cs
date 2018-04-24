using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace nasbench
{
    public partial class ConnectionDialog : Form
    {
        public char? LastConnectedDrive;

        public ConnectionDialog()
        {
            InitializeComponent();

            // Update drive lists when becoming visible
            VisibleChanged += new EventHandler((object sender, EventArgs args) => MaybeUpdateDriveLists());

            // Periodically update drive lists
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = 2500;
            timer.Tick += new EventHandler((object sender, EventArgs args) => MaybeUpdateDriveLists());
            timer.Start();

            connectButton.Click += new EventHandler((object sender, EventArgs args) => Connect());
            disconnectButton.Click += new EventHandler((object sender, EventArgs args) => Disconnect());
        }

        void MaybeUpdateDriveLists()
        {
            if(Visible)
            {
                if (!driveList.DroppedDown)
                {
                    driveList.ShowSystemDrives(LogicalDriveDropdownList.SystemDriveFilter.UNUSED);
                    driveList.SelectFirstIfNothingSelected();
                }
                if(!connectionList.DroppedDown)
                {
                    connectionList.ShowSystemDrives(LogicalDriveDropdownList.SystemDriveFilter.NETWORK);
                    connectionList.SelectFirstIfNothingSelected();
                }
            }
        }

        void Connect()
        {
            // TODO: Should we let Windows manage credentials?
            string address = shareTextBox.Text.Trim();
            if (address.Length == 0)
            {
                MessageBox.Show(this, "Bitte geben Sie den Netzwerkpfad an.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            char? selectedDrive = driveList.SelectedDrive;
            if (selectedDrive == null)
            {
                MessageBox.Show(this, "Bitte wählen Sie ein Laufwerk aus.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string username = usernameTextBox.Text;
            string password = passwordTextBox.Text;

            try
            {
                Drives.ConnectNetworkShare((char)selectedDrive, address, username, password);
            }
            catch (Win32Exception e)
            {
                MessageBox.Show(this, e.Message, "Verbindung fehlgeschlagen", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            LastConnectedDrive = selectedDrive;

            MaybeUpdateDriveLists();
        }

        void Disconnect()
        {
            char? selectedDrive = connectionList.SelectedDrive;
            if (selectedDrive == null)
            {
                MessageBox.Show(this, "Bitte wählen Sie eine Verbindung aus.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                Drives.DisconnectNetworkShare((char)selectedDrive, true);
            }
            catch(Win32Exception e)
            {
                MessageBox.Show(this, e.Message, "Trennen fehlgeschlagen", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            MaybeUpdateDriveLists();
        }
    }
}
