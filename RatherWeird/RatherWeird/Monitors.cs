using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RatherWeird
{
    public class Monitors
    {
        public MonitorState MonitorState { get; set; }
        public int MonitorIndex { get; set; }

        public static Monitors Default()
        {
            var monitor = new Monitors();
            monitor.MonitorIndex = -1;
            monitor.MonitorState = MonitorState.Primary;

            return monitor;
        } 
    }

    public enum MonitorState : int
    {
        Primary = 0,
        ByIndex = 1,
    }
}
