using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using PFlags = RatherWeird.Pinvokes.ProcessAccessFlags;

namespace RatherWeird.Utility
{

    public class InvalidHandle : Exception
    {

    }

    public delegate void MemoryWatchChangeHandler(object sender, MemoryWatchArgs e);

    public class MemHax
    {
        private readonly Dictionary<Tuple<int, IntPtr>, Thread> _memoryWatching =
            new Dictionary<Tuple<int, IntPtr>, Thread>();
        private readonly Dictionary<Tuple<int, PFlags>, IntPtr> _procIdHandle =
            new Dictionary<Tuple<int, PFlags>, IntPtr>();

        public event MemoryWatchChangeHandler MemoryWatchChanged;

        public void CleanHandles()
        {
            foreach (var intPtr in _procIdHandle)
            {
                Pinvokes.CloseHandle(intPtr.Value);
            }



            _procIdHandle.Clear();
        }

        private IntPtr GetHandle(Process proc, PFlags accessFlags)
        {
            Tuple<int, PFlags> accessor = new Tuple<int, PFlags>(proc.Id, accessFlags);

            if (_procIdHandle.ContainsKey(accessor))
            {
                var existingHandle = _procIdHandle[accessor];
                Logger.Info($"Returned existing handle to PID/flags \"{accessor}\": 0x{existingHandle.ToString("X8")}");
                return existingHandle;
            }

            IntPtr handle = Pinvokes.OpenProcess(accessFlags, true, proc.Id);

            if (handle == IntPtr.Zero)
            {
                int errCode = Marshal.GetLastWin32Error();
                Logger.Error($"ER.. unlock process (OpenProcess). code: {errCode}");
            }

            Logger.Info($"Returned fresh handle to PID/flags \"{accessor}\": 0x{handle.ToString("X8")}");

            _procIdHandle.Add(accessor, handle);
            return handle;
        }

        public bool WriteBytes(Process proc, IntPtr address, byte[] buffer)
        {
            return Pinvokes.WriteProcessMemory(
                GetHandle(proc,
                    PFlags.VirtualMemoryOperation |
                    PFlags.VirtualMemoryWrite), address, buffer, buffer.Length, out _);
        }

        public byte[] ReadBytes(Process proc, IntPtr address, int size)
        {
            byte[] buffer = new byte[size];

            Pinvokes.ReadProcessMemory(GetHandle(proc, PFlags.VirtualMemoryRead), address, buffer, size,
                out _);
            
            return buffer;
        }

        public void WatchAddress(Process proc, IntPtr address, int size, int interval)
        {
            Tuple<int, IntPtr> accessor = new Tuple<int, IntPtr>(proc.Id, address);

            if (_memoryWatching.ContainsKey(accessor))
            {
                Logger.Warn("Early exist from WatchAddress - address already watched!");
                return;
            }

            Thread thrTemp = new Thread(() =>
            {
                byte[] oldBuffer = new byte[size];

                while (true)
                {
                    Thread.Sleep(interval);

                    byte[] buffer = ReadBytes(proc, address, size);
                    if (!buffer.SequenceEqual(oldBuffer))
                    {
                        oldBuffer = buffer;
                        OnWatchChanged(new MemoryWatchArgs(address, size, buffer));
                    }
                }
            });

            _memoryWatching.Add(accessor, thrTemp);
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
