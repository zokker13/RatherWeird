using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RatherWeird
{

    public static class WindowInvocation
    {
        // This static method is required because legacy OSes do not support
        // SetWindowLongPtr
        public static IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, long dwNewLong)
        {
            if (IntPtr.Size == 8)
                return Pinvokes.SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
            
            return new IntPtr(Pinvokes.SetWindowLong32(hWnd, nIndex, (int)dwNewLong));
        }


        public static bool LockToProcess(Process process)
        {
            Pinvokes.RECT windowRect, clientRect;
            Pinvokes.GetWindowRect(process.MainWindowHandle, out windowRect);
            Pinvokes.GetClientRect(process.MainWindowHandle, out clientRect);

            int borderThickness = ((windowRect.Right - windowRect.Left) - clientRect.Right) / 2;
            int topBorderThickness = (windowRect.Bottom - windowRect.Top) - clientRect.Bottom;

            Pinvokes.RECT lockPosition;
            lockPosition.Left = windowRect.Left + borderThickness;
            lockPosition.Right = clientRect.Right + windowRect.Left + borderThickness;
            lockPosition.Top = windowRect.Top + topBorderThickness - borderThickness; 
            lockPosition.Bottom = clientRect.Bottom + windowRect.Top + topBorderThickness - borderThickness;

            return Pinvokes.ClipCursor(ref lockPosition);
        }
        
        public static void DropBorder(Process process)
        {
            long style = Pinvokes.GetWindowLong(process.MainWindowHandle, (int)GwlIndex.GWL_STYLE);

            style &= ~((long) WindowStyles.WS_CAPTION | (long) WindowStyles.WS_MAXIMIZE | (long) WindowStyles.WS_MINIMIZE |
                       (long) WindowStyles.WS_SYSMENU);
            

            /* long standardStyle = (long) WindowStyles.WS_POPUP | (long) WindowStyles.WS_MINIMIZE | (long) WindowStyles.WS_VISIBLE |
                     (long) WindowStyles.WS_CLIPSIBLINGS | (long) WindowStyles.WS_SYSMENU | 8;

             long extendedStyle = (long) WindowStyles.WS_EX_LEFT | (long) WindowStyles.WS_EX_LTRREADING |
                                  (long) WindowStyles.WS_EX_RIGHTSCROLLBAR | (long) WindowStyles.WS_EX_TOPMOST;

            long standardStyle = (long)0xb4080008;
            long extendedStyle = (long)8;
            SetWindowLongPtr(process.MainWindowHandle, (int)GwlIndex.GWL_STYLE, standardStyle);
            SetWindowLongPtr(process.MainWindowHandle, (int)GwlIndex.GWL_EXSTYLE, extendedStyle);*/

            SetWindowLongPtr(process.MainWindowHandle, (int) GwlIndex.GWL_STYLE, style);
        }

        public static void ResizeWindow(Process process)
        {
            // TODO: Drop this reference to winforms and PInvoke it?
            Screen currentOccupiedScreen = Screen.FromHandle(process.MainWindowHandle);
            Pinvokes.RECT procRect;
            Pinvokes.GetWindowRect(process.MainWindowHandle, out procRect);

            // TODO: Repair this: It seems to have a bit too much size like 1926x1102 which makes everything broken.
            int width = procRect.Right - procRect.Left;
            int height = procRect.Bottom - procRect.Top;

            Pinvokes.SetWindowPos(
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
            IntPtr handle = Pinvokes.GetForegroundWindow();

            Pinvokes.GetWindowThreadProcessId(handle, out var procId);

            if (procId == -1)
                return null;

            return Process.GetProcessById(procId);
        }

        public static Size GetClientSize(Process proc)
        {
            RECT rect;
            GetWindowRect(proc.MainWindowHandle, out rect);
            
            Size size = new Size(rect);

            return size;
        }
    }

    public class Size
    {
        public int Left { get; set; }
        public int Right { get; set; }
        public int Bottom { get; set; }
        public int Top { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }

        public Size(RECT rect) : this(rect.Left, rect.Right, rect.Top, rect.Bottom)
        {
            
        }

        public Size(int left, int right, int top, int bottom)
        {
            Left = left;
            Right = right;
            Top = top;
            Bottom = bottom;

            Width = Right - Left;
            Height = Bottom - Top;
        }

        public bool IsPointInArea(int x, int y)
        {
            if (x < Left)
                return false;
            if (x > Right)
                return false;
            if (y < Top)
                return false;
            if (y > Bottom)
                return false;

            return true;
        }

        public override string ToString()
        {
            return $"L: {Left} R: {Right} T: {Top} B: {Bottom} - W: {Width} H: {Height}";
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
        WS_POPUP = 0x80000000L,
        WS_VISIBLE = 0x10000000L,
        WS_CLIPSIBLINGS = 0x04000000L,
        WS_EX_LEFT = 0x00000000L,
        WS_EX_LTRREADING = 0x00000000L,
        WS_EX_RIGHTSCROLLBAR = 0x00000000L,
        WS_EX_TOPMOST = 0x00000008L,
    };

}
