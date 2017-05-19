using System;
using System.Collections.Generic;
using System.Linq;
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
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            _foregroundWatcher.HookForeground();

            _foregroundWatcher.ForegroundChanged += ForegroundWatcher_ForegroundChanged;
        }

        private void ForegroundWatcher_ForegroundChanged(object sender, ForegroundArgs e)
        {
            if (e.Process.MainModule.ModuleName != "RA3_1.12.game")
                return;

            Messaging.InvokeKeyUp(e.Process.MainWindowHandle, 0x12);    // ALT key
            //Messaging.SendMessage(e.Process.MainWindowHandle, Messaging.WM_KEYUP, (IntPtr)0x12, IntPtr.Zero);
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            _foregroundWatcher.Unhook();
        }
    }
}
