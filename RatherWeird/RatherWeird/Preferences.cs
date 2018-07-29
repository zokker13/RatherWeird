using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Xml.Serialization;

namespace RatherWeird
{
    public class Preferences
    {
        public static SettingEntries Load()
        {
            SettingEntries settings = new SettingEntries();
            
            if (!File.Exists(Constants.SettingsFile))
                return settings;

            XmlSerializer serial = new XmlSerializer(settings.GetType());
            settings = (SettingEntries) serial.Deserialize(new StreamReader(Constants.SettingsFile));

            return settings;
        }

        public static bool Write(SettingEntries settings)
        {
            bool result = false;

            try
            {
                XmlSerializer serial = new XmlSerializer(settings.GetType());
                serial.Serialize(new StreamWriter(Constants.SettingsFile), settings);

                result = true;
            }
            catch (IOException)
            {
                // TODO log or whatever
            }
            catch (Exception)
            {
                // TODO: Same 
            }

            return result;
        }
    }

    public class SettingEntries
    {
        public bool LockCursor;
        public bool InvokeAltUp;
        public bool RemoveBorder;
        public bool LaunchRa3Windowed;
        public bool LaunchRa3Ui;
        public string Ra3ExecutablePath;
        public bool RefreshPathToRa3;
        public bool HookNumpadEnter;
        public bool SwapHealthbarLogic;
        public bool DisableWinKey;
        public int SleepTime;

        public SettingEntries()
        {
            LockCursor = true;
            InvokeAltUp = true;
            LaunchRa3Windowed = true;
            LaunchRa3Ui = false;
            RefreshPathToRa3 = true;
            RemoveBorder = true;
            HookNumpadEnter = true;
            SwapHealthbarLogic = false;
            DisableWinKey = false;
            SleepTime = 16;
        }
    }


}
