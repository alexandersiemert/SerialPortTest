using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.Windows.Threading;
using System.Windows;

namespace SerialPortTest
{
    public class SerialPortManager
    {
        private SerialPort serialPort;
        private string receivedData = "";
        public event Action<string> DataReceived;

        public SerialPortManager(string portName)
        {
            serialPort = new SerialPort(portName, 38400, Parity.None, 8, StopBits.One);
            serialPort.DataReceived += new SerialDataReceivedEventHandler(SerialPort_DataReceived);
        }

        public string GetPortName()
        {
            return serialPort.PortName;
        }

        public void OpenPort()
        {
            try
            {
                serialPort.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error has occurred: " + serialPort.PortName + " not found", "COM-Port Error", MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
        }

        public bool IsOpen()
        {
            return serialPort.IsOpen;
        }

        public void ClosePort()
        {
            if (serialPort.IsOpen)
            {
                serialPort.Close();
            }
        }

        public void SendCommand(string command)
        {
            try
            {
                serialPort.Write(command);
            }
            catch(Exception ex)
            {
                MessageBox.Show("An error has occurred: " + serialPort.PortName + " not open", "COM-Port Error", MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }        
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                // Daten vom COM-Port lesen und an den empfangenen Datenstring anhängen
                receivedData += serialPort.ReadExisting();
                Debug.WriteLine(receivedData);

                // Überprüfen, ob die Daten mit CR LF enden
                if (receivedData.EndsWith("\r\n"))
                {
                    // Speichere das Datenpaket ohne CR LF am Ende ab
                    string completeMessage = receivedData.Substring(0, receivedData.Length - "\r\n".Length);
                    // Zurücksetzen des empfangenen Datenstrings
                    receivedData = "";
                    // Sende das komplette Datenpaket an das Received Event der jeweiligen Instanz des SerialPorts
                    DataReceived?.Invoke(completeMessage);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("An error has occurred: " + serialPort.PortName + ": " + ex.Message, "COM -Port Error", MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
        }

    }
}
