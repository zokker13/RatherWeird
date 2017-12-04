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
    /// Interaktionslogik für DebugWindow.xaml
    /// </summary>
    public partial class DebugWindow : Window
    {
        public DebugWindow()
        {
            InitializeComponent();
        }

        public void InsertText(string text)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var buffer = Encoding.ASCII.GetBytes(text);
                string bufferS = String.Empty;
                foreach (var buf in buffer)
                {
                    bufferS += $"{buf:X2} ";
                }

                lstPeerchat.Items.Insert(0, "=============");
                lstPeerchat.Items.Insert(0, text);
                lstPeerchat.Items.Insert(0, bufferS);
            });

        }
    }
}
