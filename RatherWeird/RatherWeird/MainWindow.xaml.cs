using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
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
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Threading;
using WindowHook;
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;
using RatherWeird.Utility;
using CheckBox = System.Windows.Controls.CheckBox;
using FileDialog = Microsoft.Win32.FileDialog;
using TextBox = System.Windows.Controls.TextBox;

namespace RatherWeird
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [DllImport("user32.dll")]
        static extern bool ScreenToClient(IntPtr hWnd, ref POINT lpPoint);

        [DllImport("user32.dll")]
        static extern uint MapVirtualKeyEx(uint uCode, uint uMapType, IntPtr dwhkl);

        const uint MAPVK_VK_TO_VSC = 0x00;
        const uint MAPVK_VSC_TO_VK = 0x01;
        const uint MAPVK_VK_TO_CHAR = 0x02;
        const uint MAPVK_VSC_TO_VK_EX = 0x03;
        const uint MAPVK_VK_TO_VSC_EX = 0x04;

        private readonly ProcessWatcher _processWatcher = new ProcessWatcher();
        private readonly SystemWatcher _systemWatcher = new SystemWatcher();
        private readonly KeyboardWatcher _keyboardWatcher = new KeyboardWatcher();
        private readonly MouseWatcher _mouseWatcher = new MouseWatcher();
        private readonly MemoryManipulator _memoryManipulator = new MemoryManipulator();
        private readonly MemHax _memHax = new MemHax();

        private readonly OnlinePlayers _onlinePlayers = new OnlinePlayers();
        
        private Process _latestRa3 = null;
        DispatcherTimer tmr = new DispatcherTimer();

        private Process LatestRa3
        {
            get { return _latestRa3; }
            set
            {
                if (value.Id != _latestRa3?.Id)
                {
                    _latestRa3 = value;

                    btnLaunchRa3.Dispatcher.Invoke(() => { btnLaunchRa3.Content = "RA3 launched"; });

                    SwapHealthbarLogic();
                }
            }
        }

        private SettingEntries settings;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void SystemWatcherSystemChanged(object sender, ProcessArgs e)
        {
            if (e.Process.ProcessName!= Constants.Ra3ProcessName)
                return;
            
            if (LatestRa3 == null || LatestRa3.HasExited)
                LatestRa3 = e.Process;

            if (settings.LaunchRa3Windowed)
                _mouseWatcher.WatchCursor(settings.SleepTime);
            
            if (settings.RemoveBorder)
            {
                WindowInvocation.DropBorder(e.Process);
                WindowInvocation.ResizeWindow(e.Process);
                Logger.Info("OK.. drop border and resize");
            }

            if (settings.LockCursor)
            {
                bool success = WindowInvocation.LockToProcess(e.Process);
                Logger.Info($"OK.. lock cursor to ra3 window. success: {success}");
            }

            if (settings.InvokeAltUp)
            {
                Task.Delay(100).ContinueWith(_ =>
                {
                    Inputs.SimulateAltKeyPress(e.Process.MainWindowHandle);
                    // Messaging.InvokeSysKeyPress(e.Process.MainWindowHandle, (uint) Keys.Menu);
                    // Messaging.InvokeSysKeyPress(e.Process.MainWindowHandle, (int)Keys.Menu); // ALT key
                    Logger.Info("OK.. invoke alt keypress after ra3 has gained focus");
                });

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

        private void FindRa3Process()
        {
            var ra3Procs = Process.GetProcessesByName(Constants.Ra3ProcessName);

            if (ra3Procs.Length <= 0)
            {
                Logger.Debug("RA3 *not* found at startup.");
                return;
            }

            if (Pinvokes.IsWindow(ra3Procs[0].MainWindowHandle))
            {
                Logger.Debug("RA3 found at startup.");
                LatestRa3 = ra3Procs[0];
                return;
            }

            Task.Delay(1000).ContinueWith(_ =>
            {
                Logger.Debug("RA3 found but no window yet. Retry.");
                FindRa3Process();
            });
        }
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RemoveLog();
            Utility.Utility.LogSystemInformation();
            
            settings = Preferences.Load();

            SetupControls();

            _systemWatcher.ForegroundChanged += SystemWatcherSystemChanged;
            _keyboardWatcher.KeyboardInputChanged += _keyboardWatcher_KeyboardInputChanged;
            _mouseWatcher.CursorPositionChanged += MouseWatcherOnCursorPositionChanged;
            _processWatcher.ProcessStarted += ProcessWatcherOnProcessStarted;
            _processWatcher.ProcessFinished += ProcessWatcherOnProcessFinished;

            _systemWatcher.Hook();
            _keyboardWatcher.HookKeyboard();
            _processWatcher.Hook();

            FindRa3Process();
            
            tmr.Tick += Tmr_Tick;
            tmr.Interval = new TimeSpan(0, 0, 0, 10);

            Logger.Info("OK.. application launch");
            
            // tmr.Start();
        }

        private void ProcessWatcherOnProcessFinished(object sender, ProcessArgs e)
        {
            Console.WriteLine($"[{e.Process.ProcessName}] FINISHED");

            if (Utility.Utility.IsProperRa3Process(e.Process))
            {
                btnLaunchRa3.Dispatcher.Invoke(() =>
                {
                    btnLaunchRa3.Content = "Launch RA3";
                    btnLaunchRa3.IsEnabled = true;
                });
            }
        }

        private void ProcessWatcherOnProcessStarted(object sender, ProcessArgs e)
        {
            Console.WriteLine($"[{e.Process.ProcessName}] LAUNCHED");

            if (e.Process.ProcessName != Constants.Ra3ProcessName)
                return;

            Logger.Debug($"Found ra3 process - Has Window: {Pinvokes.IsWindow(e.Process.MainWindowHandle)}");

            // Right after process creation, ra3 doesnt have a window and it can be assumed that the process is not fully loaded.
            // This is a workaround which will probably remain here forever :/
            if (!Pinvokes.IsWindow(e.Process.MainWindowHandle))
            {
                Task.Delay(1000).ContinueWith(_ => { ProcessWatcherOnProcessStarted(sender, e); });
                return;
            }
            
            if (LatestRa3 == null || LatestRa3.HasExited)
                LatestRa3 = e.Process;
        }



        private void MouseWatcherOnCursorPositionChanged(object sender, MouseInputArgs mouseInputArgs)
        {
            if (LatestRa3 == null)
                return;

            // Still need performance improvements since it's really bad right now :(
            SimulateBorderScrolling(mouseInputArgs.Point);
        }

        private void RemoveLog()
        {
            try
            {
                using (var fs = File.Create(Constants.Logfile))
                {
                }
            }
            catch (IOException)
            {
                // Good
            }
        }

        // Im fucking ashamed :(
        private bool needsKeyUp1 = false;
        private bool needsKeyUp2 = false;
        private bool needsKeyUp3 = false;
        private bool needsKeyUp4 = false;

        private void SimulateBorderScrolling(POINT origin)
        {
            POINT point = origin;
            ScreenToClient(LatestRa3.MainWindowHandle, ref point);
            Size size = WindowInvocation.GetClientSize(LatestRa3);
            

            if (!size.IsPointInArea(origin.X, origin.Y))
                return;

            // I wanted to make it cool but it somewhat looks not so cool. :(
            void Check(bool condition, Keys key, ref bool keyNeedsUp)
            {
                uint scanCode = MapVirtualKeyEx((uint)key, MAPVK_VK_TO_VSC, IntPtr.Zero);
                uint goodScanCode = scanCode << 16; // scancode is from 16-23, byte
                uint extendedMessage = goodScanCode
                                       | 1          // Key repeating
                                       | 1 << 24;   // is extended key = true

                if (condition)
                {
                    Pinvokes.PostMessage(LatestRa3.MainWindowHandle, (int) WM.KeyDown, 0, extendedMessage);
                    keyNeedsUp = true;
                }
                else
                {
                    if (keyNeedsUp)
                    {
                        Pinvokes.PostMessage(LatestRa3.MainWindowHandle, (int) WM.KeyUp, 0, extendedMessage);
                        keyNeedsUp = false;
                    }
                }
                /*
                Inputs.SendMessage(LatestRa3.MainWindowHandle, (int)WM.KeyDown, 0x25, 0x14b0001);
                Inputs.SendMessage(LatestRa3.MainWindowHandle, (int)WM.KeyUp, 0x25, 0xc14b0001);
                //Messaging.InvokeKeyPress(LatestRa3.MainWindowHandle, (uint) Keys.Left);
                Console.WriteLine("Invoked keypress");*/
            }

            // Left
            Check(point.X <= Constants.Ra3InnerScrollBorderSize, Keys.Left, ref needsKeyUp1);   

            // Right
            Check(size.Width - point.X <= Constants.Ra3InnerScrollBorderSize, Keys.Right, ref needsKeyUp2);

            // Up
            Check(point.Y <= Constants.Ra3InnerScrollBorderSize, Keys.Up, ref needsKeyUp3);

            // Down
            Check(size.Height - point.Y <= Constants.Ra3InnerScrollBorderSize, Keys.Down, ref needsKeyUp4);

        }

        private void _keyboardWatcher_KeyboardInputChanged(object sender, KeyboardInputArgs e)
        {
            HookNumpadEnter(e);
            // HookSpace(e);
        }

        private void HookSpace(KeyboardInputArgs e)
        {
            if (e.Key != Keys.Space)
                return;
            
            if (LatestRa3 == null)
                return;

            //Console.WriteLine(e.Flags);
            //Messaging.SendMessage(LatestRa3.MainWindowHandle, (int)Messaging.WM.Char, 0x20, 0x390001);
        }
        
        private void HookNumpadEnter(KeyboardInputArgs e)
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

                if (LatestRa3 != null)
                {
                    InvokeEnter(LatestRa3.MainWindowHandle);
                    Logger.Info("OK.. invoke enter after numpad enter hook");
                }
            }
        }

        private async void Tmr_Tick(object sender, EventArgs e)
        {/*
            if (LatestRa3 == null)
                return;

            var ra3Connections = Utility.Networking.GetAllTCPConnections()
                .Where(connection => connection.owningPid == LatestRa3.Id);

            foreach (var mibTcprowOwnerPid in ra3Connections)
            {
                Console.WriteLine(mibTcprowOwnerPid.RemoteAddress.ToString());
            }*/

            // _processWatcher.CheckProcesses();

            // Sample Code...
            string guid = await GuidGenerator.Generate();
            await PutReport("http://127.0.0.1:1337/report", guid);

        }

        private Task PutReport(string url, string guid)
        {
            return Task.Run(() =>
            {
                var request = (HttpWebRequest) WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/json";
                string payload = $"{{\"client_id\": \"{guid}\"}}";

                using (var reqStream = request.GetRequestStream())
                {
                    reqStream.Write(Encoding.UTF8.GetBytes(payload), 0, payload.Length);
                }

                HttpWebResponse res = (HttpWebResponse) request.GetResponse();
                string statusCode = res.StatusCode.ToString();
            });
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            _processWatcher.Unhook();
            _systemWatcher.Unhook();
            _keyboardWatcher.UnhookKeyboard();
            _mouseWatcher.UnwatchCursor();
            //_memoryManipulator.LockProcess();

            Preferences.Write(settings);
        }

        private void SetupControls()
        {
            chInvokeAltUp.IsChecked = settings.InvokeAltUp;
            chLockCursor.IsChecked = settings.LockCursor;
            chLaunchRa3Windowed.IsChecked = settings.LaunchRa3Windowed;
            chRemoveBorders.IsChecked = settings.RemoveBorder;
            chHookNumpadEnter.IsChecked = settings.HookNumpadEnter;
            chSwapHealthbarLogic.IsChecked = settings.SwapHealthbarLogic;
            chDisableWinKey.IsChecked = settings.DisableWinKey;
            chLaunchWithUi.IsChecked = settings.LaunchRa3Ui;

            txtRa3Path.Text = GetRa3Executable();
            lblVersion.Content = $"Version: {Constants.ApplicationVersion}";

            Logger.Debug("OK.. setup of controls");
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
            catch (Exception ex)
            {
                Logger.Error($"could not read from registry. {ex.Message}");
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
        
        private void btnLaunchRa3_Click(object sender, RoutedEventArgs e)
        {
            var s = sender as System.Windows.Controls.Button;
            Task.Run(() =>
            {
                string pathToRa3 = GetRa3Executable();
                if (pathToRa3 == "")
                {
                    Logger.Error("could not get path to ra3 executable");
                    System.Windows.MessageBox.Show("Please set the path of the RA3 path. We won't go further",
                        "RA3 path missing", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                string arguments = settings.LaunchRa3Windowed
                    ? " -win"
                    : "";

                if (settings.LaunchRa3Ui)
                {
                    arguments += " -ui";
                }

                Process.Start(pathToRa3, arguments);
                s?.Dispatcher.Invoke(() =>
                {
                    s.Content = "RA3 is launching...";
                    s.IsEnabled = false;
                });
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
            Inputs.SendMessage(handle, (int)Inputs.WM.KeyDown, (uint)Keys.Enter, 0x1C0001);
            Inputs.SendMessage(handle, (int)Inputs.WM.Char, (uint)Keys.Enter, 0x1C0001);
            Inputs.SendMessage(handle, (int)Inputs.WM.KeyUp, (uint)Keys.Enter, 0xC01C0001);
        }

        private void chHookNumpadEnter_Click(object sender, RoutedEventArgs e)
        {
            var adhocSender = sender as CheckBox;
            settings.HookNumpadEnter = adhocSender?.IsChecked == true;
        }

        private void chSwapHealthbarLogic_Click(object sender, RoutedEventArgs e)
        {
            var adhocSender = sender as CheckBox;
            settings.SwapHealthbarLogic = adhocSender?.IsChecked == true;

            SwapHealthbarLogic();
        }

        private void SwapHealthbarLogic()
        {
            if (LatestRa3 == null)
                return;

            Console.WriteLine("Swapping called");

            byte byteToWrite = settings.SwapHealthbarLogic ? (byte)116 : (byte)117;
            //bool success = _memoryManipulator.WriteByte((IntPtr)(0x12EB93 + (int)LatestRa3?.MainModule?.BaseAddress), byteToWrite);
            bool success = _memHax.WriteBytes(LatestRa3, (IntPtr) (0x12EB93 + (int) LatestRa3?.MainModule?.BaseAddress),
                new[] {byteToWrite});
            
            Logger.Info($"swap healthbar logic successful: {success}");
            tmr.Stop();
            
        }

        private void SwapWinKeyState(bool disableKey)
        {
            if (disableKey)
            {
                _keyboardWatcher.DisableKey(() =>
                {
                    Process foregroundProcess = WindowInvocation.GetForegroundProcess();

                    if (foregroundProcess == null)
                        return false;

                    return foregroundProcess.ProcessName == Constants.Ra3ProcessName;
                }, Keys.LWin, Keys.RWin);
            }
            else
            {
                _keyboardWatcher.EnableKey(Keys.LWin, Keys.RWin);
            }
        }

        private void chDisableWinKey_Click(object sender, RoutedEventArgs e)
        {
            var adhocSender = sender as CheckBox;
            settings.DisableWinKey = adhocSender?.IsChecked == true;
        }

        private void chDisableWinKey_Checked(object sender, RoutedEventArgs e)
        {
            SwapWinKeyState(true);
        }

        private void chDisableWinKey_Unchecked(object sender, RoutedEventArgs e)
        {
            SwapWinKeyState(false);
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog diag = new Microsoft.Win32.OpenFileDialog();
            diag.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
            diag.Filter = "Red Alert 3 Executable|RA3.exe";

            bool? result = diag.ShowDialog();
            if (result == true)
            {
                settings.Ra3ExecutablePath = diag.FileName;
                txtRa3Path.Text = diag.FileName;
                settings.RefreshPathToRa3 = false;
            }

        }
        
        private void btnShowPlayers_Click(object sender, RoutedEventArgs e)
        {
            _onlinePlayers.Show();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _onlinePlayers.ProperlyClose();
            _memHax.CleanHandles();
        }

        private void chLaunchWithUi_Click(object sender, RoutedEventArgs e)
        {
            var adhocSender = sender as CheckBox;
            settings.LaunchRa3Ui = adhocSender?.IsChecked == true;
        }
    }
}
