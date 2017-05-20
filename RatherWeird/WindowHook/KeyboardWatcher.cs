using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace WindowHook
{
    public class KeyboardWatcher
    {
        #region DLLImport 

        [DllImport("user32.dll")]
        static extern IntPtr CallNextHookEx(
            IntPtr hhk
            , int nCode
            , IntPtr wParam
            , IntPtr lParam
        );

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool UnhookWindowsHookEx(
            IntPtr hhk
        );

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr SetWindowsHookEx(
            HookType hookType
            , HookProc lpfn
            , IntPtr hMod
            , uint dwThreadId
        );

        #endregion

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

        private HookProc _del;

        private delegate IntPtr HookProc(
            int code
            , IntPtr wParam
            , IntPtr lParam
        );

        private IntPtr HHook { get; set; }

        public bool HookKeyboard()
        {
            bool result = false;

            _del = (code, wParam, lParam) =>
            {
                if (code < 0)
                    return CallNextHookEx(IntPtr.Zero, code, wParam, lParam);

                Console.WriteLine($"code: {code}");
                Console.WriteLine($"wParam: {wParam}");
                Console.WriteLine($"lParam: {lParam}");
                return CallNextHookEx(IntPtr.Zero, code, wParam, lParam);
            };

            using (Process process = Process.GetCurrentProcess())
            {
                HHook = SetWindowsHookEx(HookType.WH_KEYBOARD, _del, process.MainModule.BaseAddress, 0);
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
