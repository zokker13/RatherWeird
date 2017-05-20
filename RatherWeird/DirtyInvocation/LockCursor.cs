using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DirtyInvocation
{
    public static class LockCursor
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);

        [DllImport("user32.dll")]
        static extern bool ClipCursor(ref RECT lpRect);

        [DllImport("user32.dll")]
        static extern bool GetClipCursor(out RECT lpRect);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern long GetWindowLong(IntPtr hWnd, int nIndex);

        public static void LockToProcess(Process process)
        {
            long style = GetWindowLong(process.MainModule.BaseAddress, (int)GwlIndex.GWL_STYLE);
            Console.WriteLine($"Style: {style}");

            RECT procRect;
            GetWindowRect(process.MainWindowHandle, out procRect);
            
            Console.WriteLine(procRect);

            ClipCursor(ref procRect);

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
    };

    public enum Styles : long
    {
        GWL_STYLE = 0xFFFFFFF0,
        WS_BORDER = 0x00800000L,
        WS_MINIMIZE = 0x20000000,
        WS_MAXIMIZE = 0x01000000,
        WS_CAPTION = 0x00C00000,
        WS_SYSMENU = 0x00080000,
        WS_THICKFRAME = 0x00040000,
        SWP_NOOWNERZORDER = 0x00000200,
        SWP_FRAMECHANGED = 0x00000020,
        SWP_NOZORDER = 0x00000004,
        SWP_NOMOVE = 0x00000002,
        SWP_NOSIZE = 0x00000001
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
