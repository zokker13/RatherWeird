using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;

namespace RatherWeird.Utility
{
    public enum ProcessState
    {
        Started,
        Stopped
    }

    public class Ra3ProcessArgs : EventArgs
    {
        public int ProcessId { get; }
        public ProcessState State { get; }

        public Ra3ProcessArgs(int processId, ProcessState state)
        {
            ProcessId = processId;
            State = state;
        }
    }

    public delegate void Ra3ProcessChangeHandler(object sender, Ra3ProcessArgs e);

    public class Ra3ProcessWatcher
    {
        public event Ra3ProcessChangeHandler Ra3ProcessChanged;

        readonly ManagementEventWatcher _processStartEvent = new ManagementEventWatcher("SELECT * FROM Win32_ProcessStartTrace");
        readonly ManagementEventWatcher _processStopEvent = new ManagementEventWatcher("SELECT * FROM Win32_ProcessStopTrace");

        public Ra3ProcessWatcher()
        {
            _processStartEvent.EventArrived += ProcessStartEvent_EventArrived;
            _processStopEvent.EventArrived += ProcessStopEvent_EventArrived;

            _processStartEvent.Start();
            _processStopEvent.Start();
        }

        private void ProcessStopEvent_EventArrived(object sender, EventArrivedEventArgs e)
        {
            string processName = e.NewEvent.Properties["ProcessName"].Value.ToString();
            int processId = Convert.ToInt32(e.NewEvent.Properties["ProcessId"].Value);

            if (processName == "ra3_1.12.game")
            {
                OnRa3ProcessChanged(this, new Ra3ProcessArgs(processId, ProcessState.Started));
            }
        }

        private void ProcessStartEvent_EventArrived(object sender, EventArrivedEventArgs e)
        {
            string processName = e.NewEvent.Properties["ProcessName"].Value.ToString();
            int processId = Convert.ToInt32(e.NewEvent.Properties["ProcessId"].Value);

            if (processName == "ra3_1.12.game")
            {
                OnRa3ProcessChanged(this, new Ra3ProcessArgs(processId, ProcessState.Stopped));
            }
        }

        private void OnRa3ProcessChanged(object o, Ra3ProcessArgs e)
        {
            Ra3ProcessChanged?.Invoke(o, e);
        }

        public void Stop()
        {
            _processStartEvent.Stop();
            _processStopEvent.Stop();
        }
    }
}
