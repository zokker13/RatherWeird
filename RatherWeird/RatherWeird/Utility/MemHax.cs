using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using PFlags = RatherWeird.Pinvokes.ProcessAccessFlags;

namespace RatherWeird.Utility
{
    public class MemHax
    {
        private readonly Dictionary<Tuple<int, PFlags>, IntPtr> _procIdHandle =
            new Dictionary<Tuple<int, PFlags>, IntPtr>();

        public bool WriteBytes(Process proc, IntPtr address, byte []buffer)
        {
            return Pinvokes.WriteProcessMemory(
                GetHandle(proc,
                    PFlags.VirtualMemoryOperation |
                    PFlags.VirtualMemoryWrite), address, buffer, buffer.Length, out _);
        }

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
            if (_procIdHandle.ContainsKey(new Tuple<int, PFlags>(proc.Id, accessFlags)))
            {
                var existingHandle = _procIdHandle[new Tuple<int, PFlags>(proc.Id, accessFlags)];
                Logger.Info($"Returned existing handle to PID \"{proc.Id}\": 0x{existingHandle.ToString("X8")}");
                return existingHandle;
            }

            IntPtr handle = Pinvokes.OpenProcess(accessFlags, true, proc.Id);

            if (handle == IntPtr.Zero)
            {
                int errCode = Marshal.GetLastWin32Error();
                Logger.Error($"ER.. lock process (close handle). code: {errCode}");
            }

            Logger.Info($"Returned fresh handle to PID \"{proc.Id}\": 0x{handle.ToString("X8")}");

            _procIdHandle.Add(new Tuple<int, PFlags>(proc.Id, accessFlags), handle);
            return handle;
        }


    }
}
