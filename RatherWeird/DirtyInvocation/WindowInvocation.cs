using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DirtyInvocation
{
    public static class WindowInvocation
    {
        #region PInvokes 

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool ClipCursor(ref RECT lpRect);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool GetClipCursor(out RECT lpRect);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern long GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "SetWindowLong", SetLastError = true)]
        private static extern int SetWindowLong32(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr", SetLastError = true)]
        private static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, long dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowPos", SetLastError = true)]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int cx, int cy,
            uint wFlags);

        [DllImport("user32.dll")]
        private static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

        #endregion

        // This static method is required because legacy OSes do not support
        // SetWindowLongPtr
        public static IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, long dwNewLong)
        {
            if (IntPtr.Size == 8)
                return SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
            
            return new IntPtr(SetWindowLong32(hWnd, nIndex, (int)dwNewLong));
        }


        public static void LockToProcess(Process process)
        {
            RECT windowRect, clientRect;
            GetWindowRect(process.MainWindowHandle, out windowRect);
            GetClientRect(process.MainWindowHandle, out clientRect);

            int borderThickness = ((windowRect.Right - windowRect.Left) - clientRect.Right) / 2;
            int topBorderThickness = (windowRect.Bottom - windowRect.Top) - clientRect.Bottom;

            RECT lockPosition;
            lockPosition.Left = windowRect.Left + borderThickness;
            lockPosition.Right = clientRect.Right + windowRect.Left + borderThickness;
            lockPosition.Top = windowRect.Top + topBorderThickness - borderThickness; 
            lockPosition.Bottom = clientRect.Bottom + windowRect.Top + topBorderThickness - borderThickness;

            ClipCursor(ref lockPosition);

        }

        public static void DropBorder(Process process)
        {
            long style = GetWindowLong(process.MainWindowHandle, (int)GwlIndex.GWL_STYLE);

            style &= ~((long) WindowStyles.WS_CAPTION | (long) WindowStyles.WS_MAXIMIZE | (long) WindowStyles.WS_MINIMIZE |
                       (long) WindowStyles.WS_SYSMENU);

            SetWindowLongPtr(process.MainWindowHandle, (int) GwlIndex.GWL_STYLE, style);
        }

        public static void ResizeWindow(Process process)
        {
            // TODO: Drop this reference to winforms and PInvoke it?
            Screen currentOccupiedScreen = Screen.FromHandle(process.MainWindowHandle);
            RECT procRect;
            GetWindowRect(process.MainWindowHandle, out procRect);

            // TODO: Repair this: It seems to have a bit too much size like 1926x1102 which makes everything broken.
            int width = procRect.Right - procRect.Left;
            int height = procRect.Bottom - procRect.Top;

            SetWindowPos(
                process.MainWindowHandle
                , 0
                , currentOccupiedScreen.Bounds.X
                , currentOccupiedScreen.Bounds.Y
                , currentOccupiedScreen.Bounds.Width
                , currentOccupiedScreen.Bounds.Height
                , (uint) WindowSizing.SWP_FRAMECHANGED
            );
        }

        public static Process GetForegroundProcess()
        {
            IntPtr handle = GetForegroundWindow();

            GetWindowThreadProcessId(handle, out var procId);

            if (procId == -1)
                return null;

            return Process.GetProcessById(procId);
        }
    }

    public enum GwlIndex
    {
        GWL_EXSTYLE = -20,
        GWL_HINSTANCE = -6,
        GWL_HWNDPARENT = -8,
        GWL_ID = -12,
        GWL_STYLE = -16,
        GWL_USERDATA = -21,
        GWL_WNDPROC = -4,
    }

    public enum WindowSizing : uint
    {
        SWP_FRAMECHANGED = 0x0020
    }

    public enum WindowStyles : long
    {
        WS_BORDER = 0x00800000L,
        WS_MINIMIZE = 0x20000000,
        WS_MAXIMIZE = 0x01000000,
        WS_CAPTION = 0x00C00000,    
        WS_SYSMENU = 0x00080000,
        WS_THICKFRAME = 0x00040000,
        WS_MINIMIZEBOX = 0x00020000,
        WS_MAXIMIZEBOX = 0x00010000,
        WS_SIZEBOX = 0x00040000,
        WS_OVERLAPPED = 00000000,
    };

    public struct RECT
    {
        #region Variables.
        /// <summary>
        /// Left position of the rectangle.
        /// </summary>
        public int Left;
        /// <summary>
        /// Top position of the rectangle.
        /// </summary>
        public int Top;
        /// <summary>
        /// Right position of the rectangle.
        /// </summary>
        public int Right;
        /// <summary>
        /// Bottom position of the rectangle.
        /// </summary>
        public int Bottom;
        
        #endregion

        #region Constructor.
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="left">Horizontal position.</param>
        /// <param name="top">Vertical position.</param>
        /// <param name="right">Right most side.</param>
        /// <param name="bottom">Bottom most side.</param>
        public RECT(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public override string ToString()
        {
            return $"Left: {Left}\n" +
                   $"Right: {Right}\n" +
                   $"Top: {Top}\n" +
                   $"Bottom: {Bottom}\n";

        }

        #endregion
    }

}
