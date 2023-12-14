using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;

namespace SerialPortTest
{
    public class SerialPortManager
    {
        private SerialPort serialPort;
        private StringBuilder dataBuffer = new StringBuilder();
        public event Action<string> DataReceived;
        private CancellationTokenSource cancellationTokenSource;

        public SerialPortManager(string portName)
        {
            serialPort = new SerialPort(portName, 38400, Parity.None, 8, StopBits.One);
            cancellationTokenSource = new CancellationTokenSource();
        }

        public void OpenPort()
        {
            try
            {
                serialPort.Open();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Fehler beim Öffnen des Ports: " + ex.Message);
            }
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
            if (serialPort.IsOpen)
            {
                serialPort.Write(command);
            }
            else
            {
                throw new InvalidOperationException("Port ist nicht geöffnet.");
            }
        }

        public async Task StartReadingAsync()
        {
            while (serialPort.IsOpen)
            {
                while (!cancellationTokenSource.Token.IsCancellationRequested)
                {
                    try
                    {
                        await Task.Delay(50);
                        string data = serialPort.ReadExisting();
                        dataBuffer.Append(data);
                        Debug.WriteLine(data);

                        if (dataBuffer.ToString().Contains("\r\n"))
                        {
                            string completeMessage = dataBuffer.ToString();
                            dataBuffer.Clear();
                            DataReceived?.Invoke(completeMessage);
                        }
                    }
                    catch (TimeoutException)
                    {
                        // Timeout-Handling oder ignoriert
                    }
                }
                
            }
        }

        public void StopReading()
        {
            cancellationTokenSource.Cancel();
        }

        public void RestartReading()
        {
            cancellationTokenSource = new CancellationTokenSource();
            _ = StartReadingAsync();
        }
    }
}
