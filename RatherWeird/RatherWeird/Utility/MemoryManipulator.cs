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
using System.Security.Cryptography;
using System.Threading;

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

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool ReadProcessMemory(
            IntPtr hProcess,
            IntPtr lpBaseAddress,
            [Out] byte[] lpBuffer,
            int dwSize,
            out IntPtr lpNumberOfBytesRead);
        
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


        public event ProcessLockChangeHandler ProcessUnlocked;
        public event ProcessLockChangeHandler ProcessLocked;
        public event MemoryWatchChangeHandler MemoryWatchChanged;
        private Dictionary<IntPtr, Thread> MemoryWatching { get; set; } = new Dictionary<IntPtr, Thread>();
        private Dictionary<IntPtr, Process> Ra3ProcessHandle { get; set; } = new Dictionary<IntPtr, Process>();
        private IntPtr Handle { get; set; } = IntPtr.Zero;

        public bool UnlockProcess(Process ra3Process, ProcessAccessFlags flags)
        {
            LockProcess();

            Handle = OpenProcess(
                flags
                , false
                , ra3Process.Id
            );

            if (Handle != IntPtr.Zero)
            {
                ProcessUnlocked?.Invoke(this, new ProcessHandleArgs(Handle, ra3Process));
                Ra3ProcessHandle.Add(Handle, ra3Process);
                return true;
            }

            return false;
        }

        public bool LockProcess()
        {
            if (Handle != IntPtr.Zero &&
                Ra3ProcessHandle.ContainsKey(Handle) &&
                Ra3ProcessHandle[Handle].HasExited == false)
            {
                if (CloseHandle(Handle))
                {
                    ProcessLocked?.Invoke(this, new ProcessHandleArgs(Handle, Ra3ProcessHandle[Handle]));
                    Ra3ProcessHandle.Remove(Handle);
                    Handle = IntPtr.Zero;
                }
            }

            return true;
        }

        public bool WriteByte(IntPtr address, byte value)
        {
            if (Handle == IntPtr.Zero)
            {
                throw new InvalidHandle();
            }

            byte[] bytesToWrite = {value};

            return WriteProcessMemory(Handle, address, bytesToWrite, 1, out _);
        }

        public byte[] ReadBytes(IntPtr address, int size)
        {
            byte[] buffer = new byte[size];

            if (Handle == IntPtr.Zero)
            {
                throw new InvalidHandle();
            }

            ReadProcessMemory(Handle, address, buffer, size, out _);

            return buffer;
        }

        public void UnwatchAddress(IntPtr address)
        {
            // ??
            // Properly  finish the thread without abort
        }

        private void OnWatchChanged(MemoryWatchArgs e)
        {
            if (MemoryWatchChanged == null)
                return;

            var eventListeners = MemoryWatchChanged.GetInvocationList();
            for (int index = 0; index < eventListeners.Count(); index++)
            {
                var methodToInvoke = (MemoryWatchChangeHandler)eventListeners[index];
                methodToInvoke.BeginInvoke(this, e, EndAsyncEvent, null);
            }
        }

        private void EndAsyncEvent(IAsyncResult iar)
        {
            var ar = (System.Runtime.Remoting.Messaging.AsyncResult) iar;
            var invokedMethod = (MemoryWatchChangeHandler) ar.AsyncDelegate;

            try
            {
                invokedMethod.EndInvoke(iar);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"sux {ex.Message}");
            }
        }

        public void WatchAddress(IntPtr address, int size)
        {
            if (!MemoryWatching.ContainsKey(address))
            {
                Thread thrTemp = new Thread(() =>
                {
                    byte[] oldBuffer = new byte[size];

                    while (true)
                    {
                        byte[] buffer = ReadBytes(address, size);

                        if (!buffer.SequenceEqual(oldBuffer))
                        {
                            oldBuffer = buffer;
                            OnWatchChanged(new MemoryWatchArgs(address, size, buffer));
                            //MemoryWatchChanged?.Invoke(this, new MemoryWatchArgs(address, size, buffer));
                        }

                        Thread.Sleep(Constants.MemoryWatcherSleep);
                    }

                });
                MemoryWatching.Add(address, thrTemp);
                thrTemp.Start();
            }
        }
    }

    public class MemoryWatchArgs : EventArgs
    {
        public IntPtr Address { get; }
        public int Size { get; }
        public byte[] Buffer { get; }

        public MemoryWatchArgs(IntPtr address, int size, byte[] buffer)
        {
            Address = address;
            Size = size;
            Buffer = buffer;
        }
        public override string ToString()
        {
            return $"{Address.ToString("X4")} [{Size}] Length: {Buffer.Length}";
        }
    }

    public delegate void MemoryWatchChangeHandler(object sender, MemoryWatchArgs e);

    public class ProcessHandleArgs : EventArgs
    {
        public IntPtr Handle;
        public Process Process;
        public ProcessHandleArgs(IntPtr handle, Process process)
        {
            Handle = handle;
            Process = process;
        }

    }

    public delegate void ProcessLockChangeHandler(object sender, ProcessHandleArgs e);
}
