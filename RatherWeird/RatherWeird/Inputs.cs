using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using RatherWeird.Utility;

namespace RatherWeird
{
    public static class Inputs
    {
        [Flags]
        public enum WM
        {
            KeyDown = 0x0100,
            KeyUp = 0x0101,
            Char = 0x0102,
            SyskeyDown = 0x0104,
            SyskeyUp = 0x0105,
            MouseMove = 0x0200,
            RButtonDown = 0x0204,
            RButtonUp = 0x0205,
        };

        public static int SendMessage(IntPtr hwnd, int msg, uint wParam, uint lParam)
        {
            int lastError = 0;
            if (Pinvokes.SendMessage(hwnd, msg, wParam, lParam) != IntPtr.Zero)
            {
                lastError = Marshal.GetLastWin32Error();
                string lastErrorMessage = new Win32Exception(lastError).Message;
                Logger.Error($"SendMessage failed with {lastError}: \"{lastErrorMessage}\"");
            }
            
            return lastError;
        }

        public static IntPtr SimulateAltKeyPress(IntPtr handle)
        {
            Pinvokes.SendMessage(handle, (int)WM.SyskeyDown, 0x12, 0x20380001);
            Pinvokes.SendMessage(handle, (int)WM.SyskeyUp, 0x12, 0xC0380001);

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
            return Pinvokes.SendMessage(handle, (int)WM.KeyDown, key, 0);
        }

        public static IntPtr InvokeKeyUp(IntPtr handle, uint key)
        {
            return Pinvokes.SendMessage(handle, (int)WM.KeyUp, key, 0);
        }

        public static IntPtr InvokeSysKeyPress(IntPtr handle, uint key)
        {
            InvokeSysKeyDown(handle, key);
            InvokeSysKeyUp(handle, key);

            return IntPtr.Zero;
        }

        public static IntPtr InvokeSysKeyDown(IntPtr handle, uint key)
        {
            return Pinvokes.SendMessage(handle, (int)WM.SyskeyDown, key, 1);
        }

        public static IntPtr InvokeSysKeyUp(IntPtr handle, uint key)
        {
            return Pinvokes.SendMessage(handle, (int)WM.SyskeyUp, key, 1);
        }

        public static IntPtr InvokeMouse(IntPtr handle, int msg, uint x, uint y)
        {
            return Pinvokes.SendMessage(handle, msg, 0, MakeLParam(x, y));
        }

        private static uint MakeLParam(uint x, uint y)
        {
            return (y << 16) | (x & 0xffff);
        }
    }
}
