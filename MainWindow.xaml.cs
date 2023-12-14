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
        private SerialPort serialPort;
        private string receivedData = "";

        public MainWindow()
        {
            InitializeComponent();

            // Initialisieren Sie den COM-Port mit den entsprechenden Einstellungen
            serialPort = new SerialPort("COM3", 38400, Parity.None, 8, StopBits.One);

            serialPort.DataReceived += SerialPort_DataReceived;


            try
            {
                serialPort.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Öffnen des COM-Ports: " + ex.Message);
            }
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            // Daten vom COM-Port lesen und an den empfangenen Datenstring anhängen
            receivedData += serialPort.ReadExisting();
            Debug.WriteLine(receivedData);

            // Überprüfen, ob die Daten mit CR LF enden
            if (receivedData.EndsWith("\r\n"))
            {
                // Verarbeiten Sie die empfangenen Daten hier, z.B. anzeigen Sie sie in einem TextBox-Steuerelement
                Dispatcher.Invoke(() =>
                {
                    txtResponse.Text = receivedData;
                });

                // Zurücksetzen des empfangenen Datenstrings
                receivedData = "";
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            // Beim Schließen der Anwendung den COM-Port schließen
            if (serialPort.IsOpen)
            {
                serialPort.Close();
            }
        }

        private void btnSendCommand_Click(object sender, RoutedEventArgs e)
        {
            if (serialPort.IsOpen)
            {
                serialPort.Write("S");
            }
            else
            {
                throw new InvalidOperationException("Port ist nicht geöffnet.");
            }
        }
    }



}
