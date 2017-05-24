using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using WindowHook;
using DirtyInvocation;
using Microsoft.Win32;

namespace RatherWeird
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ForegroundWatcher _foregroundWatcher = new ForegroundWatcher();
        private readonly KeyboardWatcher _keyboardWatcher = new KeyboardWatcher();
        private SettingEntries settings;

        public MainWindow()
        {
            InitializeComponent();   
        }

        private void ForegroundWatcher_ForegroundChanged(object sender, ForegroundArgs e)
        {
            Title = e.Process.ProcessName;

            if (e.Process.ProcessName!= "ra3_1.12.game")
                return;

            if (settings.RemoveBorder)
            {
                WindowInvocation.DropBorder(e.Process);
                WindowInvocation.ResizeWindow(e.Process);
            }

            if (settings.LockCursor)
                WindowInvocation.LockToProcess(e.Process);
            
            if (settings.InvokeAltUp)
                Messaging.InvokeKeyUp(e.Process.MainWindowHandle, 0x12);    // ALT key

            if (settings.RefreshPathToRa3)
            {
                // Dirty?
                string manualPath = Path.Combine(
                    Path.GetDirectoryName(Path.GetDirectoryName(e.Process.MainModule.FileName))
                    , "RA3.exe"
                );
                txtRa3Path.Text = manualPath;
            }
        }

        private void chLockCursor_Click(object sender, RoutedEventArgs e)
        {
            var adhocSender = sender as CheckBox;
            settings.LockCursor = adhocSender?.IsChecked == true;
        }

        private void chInvokeAltUp_Click(object sender, RoutedEventArgs e)
        {
            var adhocSender = sender as CheckBox;
            settings.InvokeAltUp = adhocSender?.IsChecked == true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            settings = Preferences.Load();
            SetupControls();

            _foregroundWatcher.HookForeground();
            _keyboardWatcher.HookKeyboard();

            _foregroundWatcher.ForegroundChanged += ForegroundWatcher_ForegroundChanged;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            _foregroundWatcher.Unhook();
            _keyboardWatcher.UnhookKeyboard();

            Preferences.Write(settings);
        }

        private void SetupControls()
        {
            chInvokeAltUp.IsChecked = settings.InvokeAltUp;
            chLockCursor.IsChecked = settings.LockCursor;
            chLaunchRa3Windowed.IsChecked = settings.LaunchRa3Windowed;
            chRefreshPathToRa3.IsChecked = settings.RefreshPathToRa3;
            chRemoveBorders.IsChecked = settings.RemoveBorder;

            txtRa3Path.Text = GetRa3Executable();
        }

        private void chLaunchRa3Windowed_Click(object sender, RoutedEventArgs e)
        {
            var adhocSender = sender as CheckBox;
            settings.LaunchRa3Windowed = adhocSender?.IsChecked == true;
        }

        private bool CheckFileExistence(string fileToCheck)
        {
            try
            {
                return File.Exists(fileToCheck);
            }
            catch (Exception)
            {
                return false;
            }
        }

        private string GetRa3ExecutableFromRegistry()
        {
            string pathToRa3 = "";
            try
            {
                pathToRa3 = (string) Registry.GetValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\RA3.exe"
                    , null
                    , null
                );
            }
            catch (Exception)
            {
                // TODO: Report..?
            }

            return pathToRa3;
        }

        private string GetRa3Executable()
        {
            if (CheckFileExistence(settings.Ra3ExecutablePath))
                return settings.Ra3ExecutablePath;

            string pathFromRegistry = GetRa3ExecutableFromRegistry();
            if (CheckFileExistence(pathFromRegistry))
                return pathFromRegistry;

            return "";
        }

        private void chRefreshPathToRa3_Click(object sender, RoutedEventArgs e)
        {
            var adhocSender = sender as CheckBox;
            settings.RefreshPathToRa3 = adhocSender?.IsChecked == true;
        }

        private void btnLaunchRa3_Click(object sender, RoutedEventArgs e)
        {
            string pathToRa3 = GetRa3Executable();
            if (pathToRa3 == "")
                return; // TODO: Call message!?

            string arguments = settings.LaunchRa3Windowed
                ? " -win"
                : "";

            Process.Start(pathToRa3, arguments);
        }

        private void txtRa3Path_TextChanged(object sender, TextChangedEventArgs e)
        {
            var adhocSender = sender as TextBox;
            settings.Ra3ExecutablePath = adhocSender?.Text;
        }

        private void chRemoveBorders_Click(object sender, RoutedEventArgs e)
        {
            var adhocSender = sender as CheckBox;
            settings.RemoveBorder = adhocSender?.IsChecked == true;
        }
    }
}
