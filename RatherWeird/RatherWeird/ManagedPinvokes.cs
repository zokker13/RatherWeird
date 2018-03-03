using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace RatherWeird
{
    public static class ManagedPinvokes
    {
        public static IntPtr OpenProcess(Pinvokes.ProcessAccessFlags processAccess, bool bInheritHandle, int processId)
        {
            IntPtr handle = Pinvokes.OpenProcess(processAccess, bInheritHandle, processId);
            if (handle == IntPtr.Zero)
                throw new Win32Exception(Marshal.GetLastWin32Error());
            
            
            return handle;
        }

        public static bool CloseHandle(IntPtr handle)
        {
           bool result = Pinvokes.CloseHandle(handle);

            if (!result)
                throw new Win32Exception(Marshal.GetLastWin32Error());

            return true;
        }

        public static uint WriteProcessMemory(IntPtr handle, IntPtr address, byte[] buffer, int size)
        {
            bool result = Pinvokes.WriteProcessMemory(handle, address, buffer, size, out var numberOfBytesWritten);

            if (!result)
                throw new Win32Exception(Marshal.GetLastWin32Error());

            return numberOfBytesWritten;
        }
    }
}
