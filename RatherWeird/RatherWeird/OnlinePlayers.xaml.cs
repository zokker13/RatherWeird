using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using RatherWeird.Utility;

namespace RatherWeird
{
    /// <summary>
    /// Interaktionslogik für OnlinePlayers.xaml
    /// </summary>
    public partial class OnlinePlayers : Window
    {
        private bool _stopClosing = true;
        public OnlinePlayers()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ra3Players.StartReoccuringTask();
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            ra3Players.StopReoccuringTask();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_stopClosing)
            {
                e.Cancel = true;
                Hide();
                Logger.Debug("Closing of playerwindow was supressed");
            }
        }

        public void ProperlyClose()
        {
            _stopClosing = false;

            Close();
        }
    }
}
