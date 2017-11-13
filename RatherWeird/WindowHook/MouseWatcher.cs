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
    [StructLayout(LayoutKind.Explicit)]
    public struct Point
    {
        [FieldOffset(0)] public Int32 X;
        [FieldOffset(4)] public Int32 Y;
    }

    [Flags]
    public enum MouseFlags
    {
        Injected = 1,
        LowerIlInjected = 2
    }

    [Flags]
    public enum WMMouseMessage
    {
        MouseMove = 0x200,
        LButtonDown = 0x201,
        LButtonUp = 0x202,
        LButtonDoubleClick = 0x203,
        RButtonDown = 0x204,
        RButtonUp = 0x205,
        RButtonDoubleClick = 0x206,
        MButtonDown = 0x207,
        MButtonUp = 0x208,
        MButtonDoubleClick = 0x209,
        MouseWheel = 0x20A,
        MouseHWheel = 0x20E,
    };

    [StructLayout(LayoutKind.Explicit)]
    public struct MouseDelta
    {
        [FieldOffset(2)] public Int16 Delta;
    }

    public class MouseInputArgs : EventArgs
    {
        // Possible to add time, too. I don't need it though..
        // Ref.: https://msdn.microsoft.com/en-us/library/windows/desktop/ms644967(v=vs.85).aspx

        public WMMouseMessage MouseMessage { get; }
        public Point Point{ get; }
        public MouseFlags Flags { get; }
        public int Delta { get; }
        public MouseInputArgs(long mouseMessage, ref MouseWatcher.MSDLLHOOKSTRUCT lParam)
        {
            MouseMessage = (WMMouseMessage)mouseMessage;
            Point = lParam.Point;
            Flags = (MouseFlags)lParam.Flags;
            Delta = lParam.MouseData.Delta;
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
            , ref MSDLLHOOKSTRUCT lParam
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

        private MouseLLHookHandler _del;

        

        [StructLayout(LayoutKind.Explicit)]
        public struct MSDLLHOOKSTRUCT
        {
            [FieldOffset(0)] public Point Point;
            [FieldOffset(0x8)] public MouseDelta MouseData;
            [FieldOffset(0x12)] public UInt32 Flags;
            [FieldOffset(0x10)] public UInt32 Timestamp; 
            [FieldOffset(0x14)] public UInt32 ExtraInfo; // Not documented
        }

        private delegate IntPtr MouseLLHookHandler(
            int nCode
            , long wParam
            , ref MSDLLHOOKSTRUCT lParam
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

            _del = (int code, long wParam, ref MSDLLHOOKSTRUCT lParam) =>
            {
                // If the code is less than 0, Windows want it to be processed without touching..
                // https://msdn.microsoft.com/en-us/library/windows/desktop/ms644985(v=vs.85).aspx
                if (code < 0)
                    return CallNextHookEx(IntPtr.Zero, code, wParam, ref lParam);
                

                OnMouseInputChanged(this, new MouseInputArgs(wParam, ref lParam));
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
