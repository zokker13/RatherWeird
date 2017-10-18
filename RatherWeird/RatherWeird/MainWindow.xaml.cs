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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Threading;
using WindowHook;
using DirtyInvocation;
using Microsoft.Win32;
using CheckBox = System.Windows.Controls.CheckBox;
using TextBox = System.Windows.Controls.TextBox;
using System.Collections.ObjectModel;

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
            this.DataContext = this;
        }

        private void ForegroundWatcher_ForegroundChanged(object sender, ForegroundArgs e)
        {
            if (e.Process.ProcessName != "ra3_1.12.game")
                return;

            Log("RA3 Window found");
            latestRa3 = e.Process;

            if (settings.RemoveBorder)
            {
                if (!WindowInvocation.DropBorder(e.Process)) Log("Remove Window Border failed.");
                if (!WindowInvocation.ResizeWindow(e.Process)) Log("Resize Window failed.");
            }

            if (settings.LockCursor)
                WindowInvocation.ClipCursor(e.Process);

            if (settings.InvokeAltUp)
            {
                Task.Delay(100).ContinueWith(_ =>
                {
                    Messaging.SimulateAltKeyPress(e.Process.MainWindowHandle);
                    // Messaging.InvokeSysKeyPress(e.Process.MainWindowHandle, (uint) Keys.Menu);
                    // Messaging.InvokeSysKeyPress(e.Process.MainWindowHandle, (int)Keys.Menu); // ALT key
                });

            }

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

        private Process latestRa3 = null;

        private DispatcherTimer listConnectionsTmr;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            settings = Preferences.Load();
            settings.SetContext(this);
            SetupControls();

            if (_foregroundWatcher.HookForeground())
            {
                Log("ForegroundWatcher started. Waiting for RA3 game window...");
            }
            else
            {
                Log("ForegroundWatcher failed.");
            }

            _foregroundWatcher.ForegroundChanged += ForegroundWatcher_ForegroundChanged;
            _keyboardWatcher.KeyboardInputChanged += _keyboardWatcher_KeyboardInputChanged;

            listConnectionsTmr = new DispatcherTimer();
            listConnectionsTmr.Tick += ListConnectionsTmr_Tick;
            listConnectionsTmr.Interval = new TimeSpan(0, 0, 0, 1);
        }

        private void _keyboardWatcher_KeyboardInputChanged(object sender, KeyboardInputArgs e)
        {
            if (settings.HookNumpadEnter == false)
                return;

            // Manual filter..
            if (e.Key != Keys.Enter)
                return;

            // Check for first bit which tells that this key is extended:
            // Ref.: https://msdn.microsoft.com/en-us/library/windows/desktop/ms644967(v=vs.85).aspx
            if (e.KeyboardMessage == WM.KeyDown
                && (e.Flags & 1) == 1)
            {

                if (latestRa3 != null)
                    InvokeEnter(latestRa3.MainWindowHandle);
            }
        }

        private void ListConnectionsTmr_Tick(object sender, EventArgs e)
        {
            if (latestRa3 == null)
                return;

            lstConn.ItemsSource =
                Utility.Networking.GetAllTCPConnections()
                .Where(connection => connection.owningPid == latestRa3.Id)
                .Select(connection => String.Format("{0}:{1}", connection.RemoteAddress, connection.RemotePort));

            /*
            foreach (var mibTcprowOwnerPid in ra3Connections)
            {
                Console.WriteLine(mibTcprowOwnerPid.RemoteAddress.ToString());
            }
            */
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
            chHookNumpadEnter.IsChecked = settings.HookNumpadEnter;

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
                pathToRa3 = (string)Registry.GetValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\RA3.exe"
                    , null
                    , null
                );
            }
            catch (Exception)
            {
                Log("Failed to load RA3 path from registry. Please manually run the game once for me to get it.");
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
            Task.Run(() =>
            {
                string pathToRa3 = GetRa3Executable();
                if (pathToRa3 == "")
                {
                    Log("No RA3 path. Please manually run the game once.");
                    return;
                }

                string arguments = settings.LaunchRa3Windowed
                    ? " -win"
                    : "";

                Process.Start(pathToRa3, arguments);
            });
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

        private void InvokeEnter(IntPtr handle)
        {
            // The following describes the lPARAM for KEYDOWN:
            // https://msdn.microsoft.com/en-us/library/windows/desktop/ms646280(v=vs.85).aspx
            // lParam needs to have it's OEM value set (probnably 00011100, I didn't change it) and extended type to 0.
            // I copied the the repeat of 1.
            Messaging.SendMessage(handle, (int)Messaging.WM.KeyDown, (uint)Keys.Enter, 0x1C0001);
            Messaging.SendMessage(handle, (int)Messaging.WM.Char, (uint)Keys.Enter, 0x1C0001);
            Messaging.SendMessage(handle, (int)Messaging.WM.KeyUp, (uint)Keys.Enter, 0xC01C0001);
        }


        private void chHookNumpadEnter_Click(object sender, RoutedEventArgs e)
        {
            var adhocSender = sender as CheckBox;
            settings.HookNumpadEnter = adhocSender?.IsChecked == true;
        }

        private void Log(string log)
        {
            if (this.Dispatcher.CheckAccess())
            {
                this.txtLog.AppendText(log + "\n");
                this.txtLog.ScrollToEnd();
            }
            else
            {
                this.Dispatcher.Invoke(() => Log(log));
            }
        }

        private void chkConn_Click(object sender, RoutedEventArgs e)
        {
            var adhocSender = sender as CheckBox;
            if (adhocSender?.IsChecked == true)
            {
                listConnectionsTmr.Start();
            }
            else
            {
                listConnectionsTmr.Stop();
                //TODO: Think carefully whether to clear the list. Lol JK. 
                //lstConn.ItemsSource = null;
            }
        }

        public void OnKeyboardHookStatusChange(bool value)
        {
            if (value)
            {
                Log(_keyboardWatcher.HookKeyboard() ?
                    "KeyboardWatcher started." :
                    "KeyboardWatcher failed.");
            }
            else
            {
                _keyboardWatcher.UnhookKeyboard();
                Log("KeyboardWatcher stopped.");
            }
        }
    }
}
