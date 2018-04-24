using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Text;

namespace nasbench
{
    public class Drives
    {
        public static void ConnectNetworkShare(char letter, string remoteName, string user, string password)
        {
            NETRESOURCE netResource = new NETRESOURCE();
            netResource.dwType = ResourceType.RESOURCETYPE_DISK;
            netResource.lpLocalName = letter + ":";
            netResource.lpRemoteName = remoteName;
            int error = WNetAddConnection2(netResource, password, user, ConnectFlags.CONNECT_INTERACTIVE);
            if (error != 0)
            {
                throw new Win32Exception(error);
            }
        }

        public static void DisconnectNetworkShare(char letter, bool force)
        {
            int error = WNetCancelConnection2(letter + ":", 0, force);
            if (error != 0)
            {
                throw new Win32Exception(error);
            }
        }

        public static string GetNetworkShare(char letter)
        {
            string localName = letter + ":";
            StringBuilder remoteName = new StringBuilder(32768);
            int len = remoteName.Capacity;
            int error = WNetGetConnection(localName, remoteName, ref len);
            if(error != 0)
            {
                throw new Win32Exception(error);
            }
            return remoteName.ToString();
        }

        public enum ResourceScope
        {
            RESOURCE_CONNECTED = 1,
            RESOURCE_GLOBALNET,
            RESOURCE_REMEMBERED,
            RESOURCE_RECENT,
            RESOURCE_CONTEXT
        }

        public enum ResourceType
        {
            RESOURCETYPE_ANY      = 0,
            RESOURCETYPE_DISK     = 1,
            RESOURCETYPE_PRINT    = 2,
            RESOURCETYPE_RESERVED = 8,
            RESOURCETYPE_UNKNOWN  = -1
        }

        public enum ResourceUsage
        {
            RESOURCEUSAGE_CONNECTABLE   = 0x00000001,
            RESOURCEUSAGE_CONTAINER     = 0x00000002,
            RESOURCEUSAGE_NOLOCALDEVICE = 0x00000004,
            RESOURCEUSAGE_SIBLING       = 0x00000008,
            RESOURCEUSAGE_ATTACHED      = 0x00000010,
            RESOURCEUSAGE_ALL           = (RESOURCEUSAGE_CONNECTABLE | RESOURCEUSAGE_CONTAINER | RESOURCEUSAGE_ATTACHED),
        }

        public enum ResourceDisplayType
        {
            RESOURCEDISPLAYTYPE_GENERIC,
            RESOURCEDISPLAYTYPE_DOMAIN,
            RESOURCEDISPLAYTYPE_SERVER,
            RESOURCEDISPLAYTYPE_SHARE,
            RESOURCEDISPLAYTYPE_FILE,
            RESOURCEDISPLAYTYPE_GROUP,
            RESOURCEDISPLAYTYPE_NETWORK,
            RESOURCEDISPLAYTYPE_ROOT,
            RESOURCEDISPLAYTYPE_SHAREADMIN,
            RESOURCEDISPLAYTYPE_DIRECTORY,
            RESOURCEDISPLAYTYPE_TREE,
            RESOURCEDISPLAYTYPE_NDSCONTAINER
        }

        public class ConnectFlags
        {
            public static readonly int CONNECT_UPDATE_PROFILE = 0x00000001;
            public static readonly int CONNECT_UPDATE_RECENT  = 0x00000002;
            public static readonly int CONNECT_TEMPORARY      = 0x00000004;
            public static readonly int CONNECT_INTERACTIVE    = 0x00000008;
            public static readonly int CONNECT_PROMPT         = 0x00000010;
            public static readonly int CONNECT_REDIRECT       = 0x00000080;
            public static readonly int CONNECT_CURRENT_MEDIA  = 0x00000200;
            public static readonly int CONNECT_COMMANDLINE    = 0x00000800;
            public static readonly int CONNECT_CMD_SAVECRED   = 0x00001000;
            public static readonly int CONNECT_CRED_RESET     = 0x00002000;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private class NETRESOURCE
        {
            public ResourceScope dwScope = 0;
            public ResourceType dwType = 0;
            public ResourceDisplayType dwDisplayType = 0;
            public ResourceUsage dwUsage = 0;
            public string lpLocalName = null;
            public string lpRemoteName = null;
            public string lpComment = null;
            public string lpProvider = null;
        }

        [DllImport("mpr.dll", CharSet=CharSet.Unicode)]
        private static extern int WNetAddConnection2([In] NETRESOURCE netResource, string password, string username, int flags);

        [DllImport("mpr.dll", CharSet = CharSet.Unicode)]
        private static extern int WNetCancelConnection2([In] string lpName, int dwFlags, bool fForce);

        [DllImport("mpr.dll", CharSet = CharSet.Unicode)]
        private static extern int WNetGetConnection([In] string localName, [Out] StringBuilder remoteName, ref int length);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern int GetLogicalDrives();
    }
}
