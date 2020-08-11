using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RatherWeird
{
    public static class Constants
    {
        #region Meta Stuff

        public static readonly string SettingsFile = "settings.xml";
        public static readonly string Ra3ProcessName = "ra3_1.12.game";
        public static readonly string Logfile = "ratherweird_log.txt";
        public static readonly string ApplicationVersion = "0.7.0";
        public static readonly string CncOnlinePlayerInfo = "https://info.server.cnc-online.net/";

        #endregion

        #region Addresses

        public static readonly IntPtr Ra3ChatBuffer = (IntPtr)0x00CF5A00;
        public static readonly uint Ra3CharBufferSize = 269;

        #endregion

        public static readonly int Ra3InnerScrollBorderSize = 2;
    }
}
