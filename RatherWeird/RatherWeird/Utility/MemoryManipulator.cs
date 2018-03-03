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
       
        private Dictionary<IntPtr, Process> Ra3ProcessHandle { get; set; } = new Dictionary<IntPtr, Process>();
        private IntPtr Handle { get; set; } = IntPtr.Zero;
        private bool _processUnlocked = false;

        public bool UnlockProcess(Process ra3Process, Pinvokes.ProcessAccessFlags flags)
        {
            LockProcess();
            
            Logger.Debug($"attempt to unlock process (open process). using pid {ra3Process.Id}");
            IntPtr tmpHandle = Pinvokes.OpenProcess(
                flags
                , false
                , ra3Process.Id
            );

            if (tmpHandle != IntPtr.Zero)
            {
                _processUnlocked = true;
                Handle = tmpHandle;
                Ra3ProcessHandle.Add(tmpHandle, ra3Process);
                Logger.Debug("OK.. unlock process (open process)");
                return true;
            }

            _processUnlocked = false;
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
                    Ra3ProcessHandle.Remove(Handle);
                    Handle = IntPtr.Zero;
                    _processUnlocked = false;
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
            if (Handle == IntPtr.Zero || !_processUnlocked)
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
    }
}
