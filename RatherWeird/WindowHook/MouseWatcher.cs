using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

// Sources:
// Deserializing of KBDLLHOOKSTRUCT: https://www.codeproject.com/articles/14485/low-level-windows-api-hooks-from-c-to-stop-unwante

namespace WindowHook
{
    public enum HookType : int
    {
        WH_JOURNALRECORD = 0,
        WH_JOURNALPLAYBACK = 1,
        WH_KEYBOARD = 2,
        WH_GETMESSAGE = 3,
        WH_CALLWNDPROC = 4,
        WH_CBT = 5,
        WH_SYSMSGFILTER = 6,
        WH_MOUSE = 7,
        WH_HARDWARE = 8,
        WH_DEBUG = 9,
        WH_SHELL = 10,
        WH_FOREGROUNDIDLE = 11,
        WH_CALLWNDPROCRET = 12,
        WH_KEYBOARD_LL = 13,
        WH_MOUSE_LL = 14
    }

    public struct POINT
    {
        public int X;
        public int Y;

        public override string ToString()
        {
            return $"X: {X}, Y:{Y}";
        }
    };

    public class MouseInputArgs : EventArgs
    {
        // Possible to add time, too. I don't need it though..
        // Ref.: https://msdn.microsoft.com/en-us/library/windows/desktop/ms644967(v=vs.85).aspx

        public WM KeyboardMessage { get; }
        public POINT Point { get; }
        public int Flags { get; }
        public MouseInputArgs(long keyboardMessage, POINT point, int flags)
        {
            KeyboardMessage = (WM)keyboardMessage;
            Point = point;
            Flags = flags;
        }
    }
    public delegate void MouseInputChangeHandler(object sender, MouseInputArgs e);

    public class MouseWatcher
    {
        #region DLLImport 

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr CallNextHookEx(
            IntPtr hhk
            , int nCode
            , long wParam
            , ref MSLLHOOKSTRUCT lParam
        );

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool UnhookWindowsHookEx(
            IntPtr hhk
        );

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr SetWindowsHookEx(
            HookType hookType
            , MouseLLHookHandler lpfn
            , IntPtr hMod
            , uint dwThreadId
        );

        #endregion
       
        public event MouseInputChangeHandler MouseInputChanged;

        public bool HookRegistered { get; private set; }
        
        private MouseLLHookHandler _del;

        [StructLayout(LayoutKind.Sequential)]
        public struct MSLLHOOKSTRUCT
        {
            public POINT pt;
            public int mouseData; // be careful, this must be ints, not uints (was wrong before I changed it...). regards, cmew.
            public int flags;
            public int time;
            public UIntPtr dwExtraInfo;
        }

        private delegate IntPtr MouseLLHookHandler(
            int nCode
            , long wParam
            , ref MSLLHOOKSTRUCT lParam
        );

        private IntPtr HHook { get; set; }

        private void OnMouseInputChanged(object o, MouseInputArgs e)
        {
            MouseInputChanged?.Invoke(o, e);
        }

        public bool HookMouse()
        {
            if (HookRegistered)
                return true;

            bool result = false;

            _del = (int code, long wParam, ref MSLLHOOKSTRUCT lParam) =>
            {
                // If the code is less than 0, Windows want it to be processed without touching..
                // https://msdn.microsoft.com/en-us/library/windows/desktop/ms644985(v=vs.85).aspx
                if (code < 0)
                    return CallNextHookEx(IntPtr.Zero, code, wParam, ref lParam);

                OnMouseInputChanged(this, new MouseInputArgs(wParam, lParam.pt, lParam.flags));
                return CallNextHookEx(IntPtr.Zero, code, wParam, ref lParam);

            };

            HHook = SetWindowsHookEx(HookType.WH_MOUSE_LL, _del, IntPtr.Zero, 0);

            if (HHook == IntPtr.Zero)
            {
                // Write to file?
                var myerror = Marshal.GetLastWin32Error();
            }
            else
            {
                result = true;
                HookRegistered = true;
            }

            return result;
        }

        public bool UnhookMouse()
        {
            bool result = false;

            UnhookWindowsHookEx(HHook);

            return result;
        }
    }
}
