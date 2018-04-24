using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace nasbench
{
    class WinForms
    {
        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, UInt32 wMsg, UIntPtr wParam, IntPtr lParam);

        public static void EnableDrawing(Control parent, bool b)
        {
            SendMessage(parent.Handle, 11 /* WM_SETREDRAW */, (UIntPtr)(b ? 1 : 0), (IntPtr)0);
            if (b) parent.Refresh();
        }

        public static bool RowIsEmpty(DataGridViewRow row)
        {
            foreach (DataGridViewCell cell in row.Cells)
            {
                if (!String.IsNullOrEmpty((string)cell.Value)) return false;
            }
            return true;
        }

        public static void InvokeIfRequired(Control control, Delegate d, params object[] args)
        {
            if(control.InvokeRequired)
            {
                control.Invoke(d, args);
            }
            else
            {
                d.DynamicInvoke(args);
            }
        }

        public static void InvokeIfRequired(Control control, Action act)
        {
            InvokeIfRequired(control, new MethodInvoker(act));
        }
    }
}
