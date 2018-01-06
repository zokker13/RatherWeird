using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace RatherWeird.Utility
{
    public enum LogType
    {
        Debug,
        Info,
        Warning,
        Error,
        Fatal
    };

    public static class Logger
    {
        public static void Log(LogType type, string text)
        {
            DateTime now = DateTime.Now;
            
            using (StreamWriter sw = File.AppendText(Constants.Logfile))
            {
                sw.WriteLine($"[{now.ToString(CultureInfo.InvariantCulture)} ({type})] {text}");
            }
        }

        public static void Debug(string text)
        {
            Log(LogType.Debug, text);
        }

        public static void Info(string text)
        {
            Log(LogType.Info, text);
        }

        public static void Warn(string text)
        {
            Log(LogType.Warning, text);
        }

        public static void Error(string text)
        {
            Log(LogType.Error, text);
        }

        public static void Fatal(string text)
        {
            Log(LogType.Fatal, text);
            MessageBox.Show($"Fatal Error was thrown:\n{text}\nYou shouldn't see this.", "FATAL ERROR");
        }
    }
}
