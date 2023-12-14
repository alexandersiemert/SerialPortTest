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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO.Ports;
using System.Threading;
using System.Diagnostics;

namespace SerialPortTest
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SerialPortManager serialPortManager;

        public MainWindow()
        {
            InitializeComponent();
            serialPortManager = new SerialPortManager("COM3"); // Ersetzen Sie "COMx" mit Ihrer COM-Port-Nummer
            serialPortManager.DataReceived += OnDataReceived;
            serialPortManager.OpenPort();
            _ = serialPortManager.StartReadingAsync();
        }

        private void btnSendCommand_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                serialPortManager.SendCommand("I");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OnDataReceived(string data)
        {
            Dispatcher.Invoke(() =>
            {
                txtResponse.Text = data;
            });
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            serialPortManager.StopReading();
            serialPortManager.ClosePort();
        }
    }



}
