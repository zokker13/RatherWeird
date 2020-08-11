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

namespace RatherWeird
{
    /// <summary>
    /// Interaktionslogik für MonitorDetector.xaml
    /// </summary>
    public partial class MonitorDetector : Window
    {
        public MonitorDetector()
        {
            InitializeComponent();
        }

        public int MonitorId { get; set; }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lblMonitor.Content = $"Monitor No: {this.MonitorId}";

        }
    }
}
