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
using System.Threading;

namespace RatherWeird.Utility
{
    public class InvalidHandle : Exception
    {
        
    }


    public delegate void MemoryWatchChangeHandler(object sender, MemoryWatchArgs e);
    public delegate void ProcessLockChangeHandler(object sender, ProcessHandleArgs e);

    public class MemoryManipulator
    {

        public event ProcessLockChangeHandler ProcessUnlocked;
        public event ProcessLockChangeHandler ProcessLocked;
        public event MemoryWatchChangeHandler MemoryWatchChanged;

        private Dictionary<IntPtr, Thread> MemoryWatching { get; set; } = new Dictionary<IntPtr, Thread>();
        private Dictionary<IntPtr, Process> Ra3ProcessHandle { get; set; } = new Dictionary<IntPtr, Process>();
        private IntPtr Handle { get; set; } = IntPtr.Zero;

        public bool UnlockProcess(Process ra3Process, Pinvokes.ProcessAccessFlags flags)
        {
            LockProcess();

            Logger.Debug($"attempt to unlock process (open process). using pid {ra3Process.Id}");
            Handle = Pinvokes.OpenProcess(
                flags
                , false
                , ra3Process.Id
            );

            if (Handle != IntPtr.Zero)
            {
                ProcessUnlocked?.Invoke(this, new ProcessHandleArgs(Handle, ra3Process));
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
                if (Pinvokes.CloseHandle(Handle))
                {
                    ProcessLocked?.Invoke(this, new ProcessHandleArgs(Handle, Ra3ProcessHandle[Handle]));
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

            bool success = Pinvokes.WriteProcessMemory(Handle, address, bytesToWrite, 1, out _);
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

        public byte[] ReadBytes(IntPtr address, int size)
        {
            byte[] buffer = new byte[size];

            if (Handle == IntPtr.Zero)
                throw new InvalidHandle();

            Pinvokes.ReadProcessMemory(Handle, address, buffer, size, out _);

            return buffer;
        }

        public void UnwatchAddress(IntPtr address)
        {
            // TODO: close thread or we
        }

        


        public void WatchAddress(IntPtr address, int size)
        {
            if (MemoryWatching.ContainsKey(address))
            {
                Logger.Warn("Early exist from WatchAddress - address already watched!");
                return;
            }

            Thread thrTemp = new Thread(() =>
            {
                byte[] oldBuffer = new byte[size];

                while (true)
                {
                    Thread.Sleep(Constants.MemoryWatcherSleep);
                    
                    byte[] buffer = ReadBytes(address, size);
                    if (!buffer.SequenceEqual(oldBuffer))
                    {
                        oldBuffer = buffer;
                        OnWatchChanged(new MemoryWatchArgs(address, size, buffer));
                    }
                }
            });

            MemoryWatching.Add(address, thrTemp);
            thrTemp.Start();
        }

        private void OnWatchChanged(MemoryWatchArgs e)
        {
            if (MemoryWatchChanged == null)
                return;

            var eventListeners = MemoryWatchChanged.GetInvocationList();

            for (int i = 0; i < eventListeners.Length; i++)
            {
                var methodToInvoke = (MemoryWatchChangeHandler) eventListeners[i];
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
                Logger.Error($"Couldnt end invoke or whatever {ex.Message}");
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

    public class ProcessHandleArgs : EventArgs
    {
        public IntPtr Handle { get; }
        public Process Process { get; }

        public ProcessHandleArgs(IntPtr handle, Process process)
        {
            Handle = handle;
            Process = process;
        }
    }
}
