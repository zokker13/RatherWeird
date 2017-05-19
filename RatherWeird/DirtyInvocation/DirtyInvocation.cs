using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;


namespace DirtyInvocation
{
    public static class Messaging
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        public const int WM_KEYFIRST = 0x0100;
        public const int WM_KEYDOWN = 0x0100;
        public const int WM_KEYUP = 0x0101;
        public const int WM_CHAR = 0x0102;
        public const int WM_DEADCHAR = 0x0103;
        public const int WM_SYSKEYDOWN = 0x0104;
        public const int WM_SYSKEYUP = 0x0105;
        public const int WM_SYSCHAR = 0x0106;
        public const int WM_SYSDEADCHAR = 0x0107;

        public const int WM_MOUSEMOVE = 0x0200;
        public const int WM_RBUTTONDOWN = 0x0204;
        public const int WM_RBUTTONUP = 0x0205;

        public static IntPtr InvokeKeyPress(IntPtr handle, int key)
        {
            InvokeKeyDown(handle, key);
            InvokeKeyUp(handle, key);

            return IntPtr.Zero;
        }

        public static IntPtr InvokeKeyDown(IntPtr handle, int key)
        {
            return SendMessage(handle, WM_KEYDOWN, (IntPtr)key, IntPtr.Zero);
        }

        public static IntPtr InvokeKeyUp(IntPtr handle, int key)
        {
            return SendMessage(handle, WM_KEYUP, (IntPtr)key, IntPtr.Zero);
        }


        public static IntPtr InvokeMouse(IntPtr handle, int msg, uint x, uint y)
        {
            return SendMessage(handle, msg, IntPtr.Zero, MakeLParam(x, y));
        }

        private static IntPtr MakeLParam(uint x, uint y)
        {
            return (IntPtr)((y << 16) | (x & 0xffff));
        }
    }
}
