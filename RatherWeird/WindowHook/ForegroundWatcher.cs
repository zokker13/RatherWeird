using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace WindowHook
{
    public class ForegroundWatcher
    {
        #region DllImport

        [DllImport("user32.dll")]
        static extern IntPtr SetWinEventHook(
            uint eventMin
            , uint eventMax
            , IntPtr hmodWinEventProc
            , WinEventDelegate lpfnWinEventProc
            , uint idProcess
            , uint idThread
            , uint dwFlags
        );

        [DllImport("user32.dll")]
        static extern bool UnhookWinEvent(IntPtr hWinEventHook);

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        #endregion

        const uint WINEVENT_OUTOFCONTEXT = 0x0000; // Events are ASYNC
        const uint EVENT_SYSTEM_FOREGROUND = 0x0003;
        
        public event ForegroundChangeHandler ForegroundChanged;

        private IntPtr WndHook { get; set; } = IntPtr.Zero;

        private WinEventDelegate _del;

        private delegate void WinEventDelegate(
            IntPtr hWinEventHook
            , uint eventType
            , IntPtr hwnd
            , int idObject
            , int idChild
            , uint dwEventThread
            , uint dwmsEventTime
            );

        public bool HookForeground()
        {
            _del = (hWinEventHook, eventType, hwnd, idObject, idChild, dwEventThread, dwmsEventTime) =>
            {
                if (eventType == EVENT_SYSTEM_FOREGROUND)
                {
                    GetWindowThreadProcessId(hwnd, out uint processId);
                    var process = Process.GetProcessById((int)processId);

                    OnForegroundChange(this, new ForegroundArgs(process));
                }
                    
            };

            WndHook = SetWinEventHook(
                EVENT_SYSTEM_FOREGROUND
                , EVENT_SYSTEM_FOREGROUND
                , IntPtr.Zero
                , _del
                , 0
                , 0
                , WINEVENT_OUTOFCONTEXT
            );

            return WndHook == IntPtr.Zero;
        }
       
        public void Unhook()
        {
            UnhookWinEvent(WndHook);
        }

        private void OnForegroundChange(object o, ForegroundArgs e)
        {
            ForegroundChanged?.Invoke(o, e);
        }
    }

    public delegate void ForegroundChangeHandler(object sender, ForegroundArgs e);
    public class ForegroundArgs : EventArgs
    {
        public Process Process { get; }
        public ForegroundArgs(Process process)
        {
            Process = process;
        }
    }
}
