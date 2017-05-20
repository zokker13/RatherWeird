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
        
        [Flags]
        public enum WM
        {
            KeyDown= 0x0100,
            KeyUp = 0x0101,
            SyskeyDown= 0x0104,
            SyskeyUp = 0x0105,
            MouseMove = 0x0200,
            RButtonDown = 0x0204,
            RButtonUp = 0x0205,
        };

        public static IntPtr InvokeKeyPress(IntPtr handle, int key)
        {
            InvokeKeyDown(handle, key);
            InvokeKeyUp(handle, key);

            return IntPtr.Zero;
        }

        public static IntPtr InvokeKeyDown(IntPtr handle, int key)
        {
            return SendMessage(handle, (int)WM.KeyDown, (IntPtr)key, IntPtr.Zero);
        }

        public static IntPtr InvokeKeyUp(IntPtr handle, int key)
        {
            return SendMessage(handle, (int)WM.KeyUp, (IntPtr)key, IntPtr.Zero);
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
