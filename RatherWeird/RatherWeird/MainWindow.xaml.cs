using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using WindowHook;
using DirtyInvocation;

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

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            _foregroundWatcher.HookForeground();
            _keyboardWatcher.HookKeyboard();

            _foregroundWatcher.ForegroundChanged += ForegroundWatcher_ForegroundChanged;

            SetupControls();

        }

        private void ForegroundWatcher_ForegroundChanged(object sender, ForegroundArgs e)
        {
            Title = e.Process.ProcessName;

            if (e.Process.ProcessName!= "ra3_1.12.game")
                return;

            WindowInvocation.DropBorder(e.Process);
            WindowInvocation.ResizeWindow(e.Process);

            if (settings.LockCursor)
                WindowInvocation.LockToProcess(e.Process);
            
            if (settings.InvokeAltUp)
                Messaging.InvokeKeyUp(e.Process.MainWindowHandle, 0x12);    // ALT key


            
            

            //Messaging.SendMessage(e.Process.MainWindowHandle, Messaging.WM_KEYUP, (IntPtr)0x12, IntPtr.Zero);
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
        }
        
    }
}
