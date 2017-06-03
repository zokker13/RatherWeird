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
        // LPARAM = unsigned int
        // WPARAM = long
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int msg, uint wParam, long lParam);
        
        [Flags]
        public enum WM
        {
            KeyDown= 0x0100,
            KeyUp = 0x0101,
            Char = 0x0102,
            SyskeyDown= 0x0104,
            SyskeyUp = 0x0105,
            MouseMove = 0x0200,
            RButtonDown = 0x0204,
            RButtonUp = 0x0205,
        };

        public static IntPtr SimulateAltKeyPress(IntPtr handle)
        {
            SendMessage(handle, (int)WM.SyskeyDown, 0x12, 0x20380001);
            SendMessage(handle, (int)WM.SyskeyUp, 0x12, 0xC0380001);

            return IntPtr.Zero;
        }

        public static IntPtr InvokeKeyPress(IntPtr handle, uint key)
        {
            InvokeKeyDown(handle, key);
            InvokeKeyUp(handle, key);

            return IntPtr.Zero;
        }

        public static IntPtr InvokeKeyDown(IntPtr handle, uint key)
        {
            return SendMessage(handle, (int)WM.KeyDown, key, 0);
        }

        public static IntPtr InvokeKeyUp(IntPtr handle, uint key)
        {
            return SendMessage(handle, (int)WM.KeyUp, key, 0);
        }

        public static IntPtr InvokeSysKeyPress(IntPtr handle, uint key)
        {
            InvokeSysKeyDown(handle, key);
            InvokeSysKeyUp(handle, key);

            return IntPtr.Zero;
        }

        public static IntPtr InvokeSysKeyDown(IntPtr handle, uint key)
        {
            return SendMessage(handle, (int)WM.SyskeyDown, key, 1);
        }

        public static IntPtr InvokeSysKeyUp(IntPtr handle, uint key)
        {
            return SendMessage(handle, (int)WM.SyskeyUp, key, 1);
        }

        public static IntPtr InvokeMouse(IntPtr handle, int msg, uint x, uint y)
        {
            return SendMessage(handle, msg, 0, MakeLParam(x, y));
        }

        private static uint MakeLParam(uint x, uint y)
        {
            return (y << 16) | (x & 0xffff);
        }
    }
}
