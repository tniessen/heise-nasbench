using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace nasbench
{
    public partial class LogicalDriveDropdownList : UserControl
    {
        public enum SystemDriveFilter
        {
            ALL, UNUSED, USED, NETWORK
        }

        public bool DroppedDown
        {
            get { return driveComboBox.DroppedDown; }
        }

        public char? SelectedDrive
        {
            get
            {
                string selectedItem = (string)driveComboBox.SelectedItem;
                if (selectedItem == null)
                {
                    return null;
                }
                else
                {
                    return selectedItem[0];
                }
            }
            set
            {
                if (value == null)
                {
                    driveComboBox.SelectedIndex = -1;
                }
                else
                {
                    for (int i = 0; i < driveComboBox.Items.Count; i++)
                    {
                        if (((string)driveComboBox.Items[i])[0] == value)
                        {
                            driveComboBox.SelectedIndex = i;
                            break;
                        }
                    }
                }
            }
        }

        public LogicalDriveDropdownList()
        {
            InitializeComponent();

            driveComboBox.Size = Size;
            SizeChanged += new EventHandler((object sender, EventArgs args) => driveComboBox.Size = Size);
        }

        public void SelectFirstIfNothingSelected()
        {
            if (SelectedDrive == null && driveComboBox.Items.Count > 0)
            {
                driveComboBox.SelectedIndex = 0;
            }
        }

        public void ShowSystemDrives(SystemDriveFilter filter)
        {
            int mask = 0;
            switch (filter)
            {
                case SystemDriveFilter.ALL:
                    mask = ~0;
                    break;
                case SystemDriveFilter.NETWORK:
                case SystemDriveFilter.USED:
                    mask = Drives.GetLogicalDrives();
                    break;
                case SystemDriveFilter.UNUSED:
                    mask = ~Drives.GetLogicalDrives();
                    break;
            }

            char? curDrive = SelectedDrive;
            driveComboBox.Items.Clear();

            for (char letter = 'A'; letter <= 'Z'; letter++)
            {
                int letterBit = 1 << (letter - 'A');
                if ((mask & letterBit) != 0)
                {
                    if(filter == SystemDriveFilter.NETWORK)
                    {
                        try
                        {
                            string networkPath = Drives.GetNetworkShare(letter);
                            driveComboBox.Items.Add(letter + ": " + networkPath);
                        }
                        catch(Win32Exception)
                        {
                            // Not a network drive
                        }
                    }
                    else
                    {
                        driveComboBox.Items.Add(letter + ":");
                    }
                }
            }

            SelectedDrive = curDrive;
        }
    }
}
