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
            serialPortManager = new SerialPortManager("COM3");
            serialPortManager.DataReceived += OnDataReceived;
            serialPortManager.OpenPort();
        
        }

        

        /*################################# EVENTHANDLER ##################################################*/

        //Evenhandler für DataReceived
        private void OnDataReceived(string data)
        {
            Dispatcher.Invoke(() =>
            {
                txtResponse.Text = data; // Angenommen, txtResponse ist Ihre TextBox
            });
        }

        //Evenhandler wenn Fenster geschlossen wird
        private void Window_Closed(object sender, EventArgs e)
        {
            // Beim Schließen der Anwendung den COM-Port schließen
            if (serialPortManager.IsOpen())
            {
                serialPortManager.ClosePort();
            }
        }

        //Eventhandler für Button 
        private void btnSendCommand_Click(object sender, RoutedEventArgs e)
        {
            serialPortManager.SendCommand("G");
        }
    }



}
