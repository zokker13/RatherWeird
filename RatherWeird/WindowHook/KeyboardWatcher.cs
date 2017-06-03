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
    [Flags]
    public enum WM
    {
        KeyDown = 0x0100,
        KeyUp = 0x0101,
        SyskeyDown = 0x0104,
        SyskeyUp = 0x0105,
    };

    public class KeyboardInputArgs : EventArgs
    {
        // Possible to add time, too. I don't need it though..
        // Ref.: https://msdn.microsoft.com/en-us/library/windows/desktop/ms644967(v=vs.85).aspx

        public WM KeyboardMessage { get; }
        public Keys Key { get; }
        public int Flags { get; }
        public KeyboardInputArgs(long keyboardMessage, int vkCode, int flags)
        {
            KeyboardMessage = (WM)keyboardMessage;
            Key = (Keys) vkCode;
            Flags = flags;
        }
    }
    public delegate void KeyboardInputChangeHandler(object sender, KeyboardInputArgs e);

    public class KeyboardWatcher
    {
        #region DLLImport 

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr CallNextHookEx(
            IntPtr hhk
            , int nCode
            , long wParam
            , ref KBDLLHOOKSTRUCT lParam
        );

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool UnhookWindowsHookEx(
            IntPtr hhk
        );

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr SetWindowsHookEx(
            HookType hookType
            , KeyboardLLHookHandler lpfn
            , IntPtr hMod
            , uint dwThreadId
        );

        #endregion

        public event KeyboardInputChangeHandler KeyboardInputChanged;

        public bool HookRegistered { get; private set; } = false;

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

        private KeyboardLLHookHandler _del;

        private struct KBDLLHOOKSTRUCT
        {
            public int vkCode;
            int scanCode;
            public int flags;
            int time;
            int dwExtraInfo;
        }
        
        private delegate IntPtr KeyboardLLHookHandler(
            int nCode
            , long wParam
            , ref KBDLLHOOKSTRUCT lParam
        );

        private IntPtr HHook { get; set; }

        private void OnKeyboardInputChanged(object o, KeyboardInputArgs e)
        {
            KeyboardInputChanged?.Invoke(o, e);
        }

        public bool HookKeyboard()
        {
            if (HookRegistered)
                return true;

            bool result = false;

            _del = (int code, long wParam, ref KBDLLHOOKSTRUCT lParam) =>
            {
                if (code < 0)
                    return CallNextHookEx(IntPtr.Zero, code, wParam, ref lParam);

                OnKeyboardInputChanged(this, new KeyboardInputArgs(wParam, lParam.vkCode, lParam.flags));
                return CallNextHookEx(IntPtr.Zero, code, wParam, ref lParam);
            };
            
            HHook = SetWindowsHookEx(HookType.WH_KEYBOARD_LL, _del, IntPtr.Zero, 0);

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

        public bool UnhookKeyboard()
        {
            bool result = false;

            UnhookWindowsHookEx(HHook);

            return result;
        }
    }
}
