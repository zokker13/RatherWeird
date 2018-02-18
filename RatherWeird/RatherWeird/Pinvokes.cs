using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RatherWeird
{
    public static class Pinvokes
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int msg, uint wParam, long lParam);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool ClipCursor(ref RECT lpRect);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetClipCursor(out RECT lpRect);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern long GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "SetWindowLong", SetLastError = true)]
        public static extern int SetWindowLong32(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr", SetLastError = true)]
        public static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, long dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowPos", SetLastError = true)]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int cx, int cy,
            uint wFlags);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

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
}
