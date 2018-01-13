using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace RatherWeird.Utility
{
    public class Utility
    {
        public static Dictionary<string, string> SystemInformation()
        {
            Dictionary<string, string> systemInfo = new Dictionary<string, string>();
            
            
            systemInfo.Add("osversion", Environment.OSVersion.ToString());
            systemInfo.Add("clr_version", Environment.Version.ToString());
            systemInfo.Add("is_64bit_process", Environment.Is64BitProcess.ToString());
            systemInfo.Add("is_64bit_os", Environment.Is64BitOperatingSystem.ToString());
            systemInfo.Add("current_directory", Environment.CurrentDirectory);

            return systemInfo;
        }

        public static void LogSystemInformation()
        {
            var systemInfo = SystemInformation();

            Logger.Info(Logger.Fillerline("=", 20));
            Logger.Info("System Information");
            Logger.Info(Logger.Fillerline("-", 20));
            foreach (var element in systemInfo)
            {
                Logger.Info($"{element.Key}: {element.Value}");
            }
            Logger.Info(Logger.Fillerline("=", 20));
        }
        
    }
}
