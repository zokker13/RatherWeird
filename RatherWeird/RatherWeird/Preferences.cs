﻿using System;
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
            settings = (SettingEntries)serial.Deserialize(new StreamReader(Constants.SettingsFile));

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
        private MainWindow context;

        public bool LockCursor;
        public bool InvokeAltUp;
        public bool RemoveBorder;
        public bool LaunchRa3Windowed;
        public string Ra3ExecutablePath;
        public bool RefreshPathToRa3;
        public bool HookNumpadEnter
        {
            get
            {
                return hookNumpadEnter;
            }
            set
            {
                if (context!=null)
                {
                    context.OnKeyboardHookStatusChange(value);
                    hookNumpadEnter = value;
                }
            }
        }
        private bool hookNumpadEnter;

        public SettingEntries()
        {
            context = null;

            LockCursor = true;
            InvokeAltUp = true;
            LaunchRa3Windowed = true;
            RefreshPathToRa3 = true;
            RemoveBorder = true;
            HookNumpadEnter = true;
        }

        public void SetContext(MainWindow context)
        {
            this.context = context;
        }
    }


}
