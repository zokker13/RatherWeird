using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace WindowHook
{
    public delegate void ProcessStartedHandler(object sender, ProcessArgs e);
    public delegate void ProcessFinishedHandler(object sender, ProcessArgs e);

    public class ProcessWatcher
    {
        public event ProcessStartedHandler ProcessStarted;
        public event ProcessFinishedHandler ProcessFinished;
        
        public bool IsRunning { get; private set; }
        public int RefreshInterval { get; set; } = 1000;

        private ProcessWrap[] _oldProcesses = {};
        
        
        private void OnProcessStartedChange(object sender, ProcessArgs e)
        {
            ProcessStarted?.Invoke(sender, e);
        }
        
        private void OnProcessFinishedChange(object sender, ProcessArgs e)
        {
            ProcessFinished?.Invoke(sender, e);
        }

        public void CheckProcesses()
        {
            Process[] procs = Process.GetProcesses();
            ProcessWrap[] currentProcesses = new ProcessWrap[procs.Length];

            for (int i = 0; i < procs.Length; i++)
                currentProcesses[i] = new ProcessWrap(procs[i]);

            var newEntries = currentProcesses.Except(_oldProcesses);
            var oldEntries = _oldProcesses.Except(currentProcesses);

            foreach (var newEntry in newEntries)
            {
                OnProcessStartedChange(this, new ProcessArgs(newEntry.Process));
            }

            foreach (var oldEntry in oldEntries)
            {
                OnProcessFinishedChange(this, new ProcessArgs(oldEntry.Process));
            }

            _oldProcesses = currentProcesses;
        }

        public void Hook()
        {
            if (IsRunning)
                return;

            IsRunning = true;

            new Thread(() =>
            {
                while (IsRunning)
                {
                    CheckProcesses();

                    Thread.Sleep(RefreshInterval);
                }
            }).Start();
        }

        public void Unhook()
        {
            IsRunning = false;
        }
    }

    public class ProcessWrap 
    {
        public Process Process { get; }
        public ProcessWrap(Process proc)
        {
            Process = proc;
        }

        public override int GetHashCode()
        {
            return Process.MachineName.GetHashCode() ^ Process.ProcessName.GetHashCode() ^ Process.Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = obj as ProcessWrap;

            if (other == null)
                return false;
            
            if (obj == this)
                return true;

            if (!other.Process.MachineName.Equals(Process.MachineName))
                return false;
            
            if (!other.Process.Id.Equals(Process.Id))
                return false;

            if (!other.Process.ProcessName.Equals(Process.ProcessName))
                return false;
           

            return true;
        }
        
    }
}
