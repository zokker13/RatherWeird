using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Security;

namespace RatherWeird.Utility
{
    public class InvalidHandle : Exception
    {
        
    }

    public class MemoryManipulator
    {
        // Stolen from pinvoke: http://www.pinvoke.net/default.aspx/kernel32/OpenProcess.html
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr OpenProcess(ProcessAccessFlags processAccess, bool bInheritHandle, int processId);

        // Stolen from: http://www.pinvoke.net/default.aspx/kernel32/CloseHandle.html
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool WriteProcessMemory(
            IntPtr hProcess,
            IntPtr lpBaseAddress,
            byte[] lpBuffer,
            uint nSize,
            out uint lpNumberOfBytesWritten);
        

        // Stolen from pinvoke: http://www.pinvoke.net/default.aspx/kernel32/OpenProcess.html
        [Flags]
        public enum ProcessAccessFlags : uint
        {
            All = 0x001F0FFF,
            Terminate = 0x00000001,
            CreateThread = 0x00000002,
            VirtualMemoryOperation = 0x00000008,
            VirtualMemoryRead = 0x00000010,
            VirtualMemoryWrite = 0x00000020,
            DuplicateHandle = 0x00000040,
            CreateProcess = 0x000000080,
            SetQuota = 0x00000100,
            SetInformation = 0x00000200,
            QueryInformation = 0x00000400,
            QueryLimitedInformation = 0x00001000,
            Synchronize = 0x00100000
        }

        private Dictionary<IntPtr, Process> Ra3ProcessHandle { get; set; } = new Dictionary<IntPtr, Process>();
        private IntPtr Handle { get; set; } = IntPtr.Zero;

        public bool UnlockProcess(Process ra3Process, ProcessAccessFlags flags)
        {
            LockProcess();

            Logger.Debug($"attempt to unlock process (open process). using pid {ra3Process.Id}");
            Handle = OpenProcess(
                flags
                , false
                , ra3Process.Id
            );

            if (Handle != IntPtr.Zero)
            {
                Ra3ProcessHandle.Add(Handle, ra3Process);
                Logger.Debug("OK.. unlock process (open process)");
                return true;
            }

            int errCode = Marshal.GetLastWin32Error();

            Logger.Error($"ER.. unlock process (open process). code: {errCode}");
            return false;
        }

        public bool LockProcess()
        {
            if (Handle != IntPtr.Zero &&
                Ra3ProcessHandle.ContainsKey(Handle) &&
                Ra3ProcessHandle[Handle].HasExited == false)
            {
                Logger.Debug("attempt to lock process (close handle)");
                if (CloseHandle(Handle))
                {
                    Ra3ProcessHandle.Remove(Handle);
                    Handle = IntPtr.Zero;
                    Logger.Debug("OK.. lock process (close handle)");
                }
                else
                {
                    int errCode = Marshal.GetLastWin32Error();
                    Logger.Error($"ER.. lock process (close handle). code: {errCode}");
                }
            }

            Logger.Debug("no process or handle found that could be closed");
            return true;
        }

        public bool WriteByte(IntPtr address, byte value)
        {
            if (Handle == IntPtr.Zero)
            {
                throw new InvalidHandle();
            }

            Logger.Debug($"attempt to write byte to {address.ToString("X8")}");

            byte[] bytesToWrite = {value};

            bool success = WriteProcessMemory(Handle, address, bytesToWrite, 1, out _);
            if (success)
            {
                Logger.Debug("OK.. write byte");
            }
            else
            {
                int errCode = Marshal.GetLastWin32Error();
                Logger.Error($"ER.. write byte. code: {errCode}");
            }

            return success;
        }
    }
}
