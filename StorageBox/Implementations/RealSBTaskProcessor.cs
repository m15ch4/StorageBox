using StorageBox.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StorageBox.Models;
using Caliburn.Micro;
using System.IO.Ports;
using System.Diagnostics;
using StorageBox.Exceptions;
using System.Threading;

namespace StorageBox.Implementations
{
    class RealSBTaskProcessor : ISBTaskProcessor
    {
        private ISBTaskService _sbTaskService;
        private IEventAggregator _eventAggregator;

        static SerialPort _serialPort;

        public RealSBTaskProcessor(ISBTaskService sbTaskService, IEventAggregator eventAggregator)
        {
            _sbTaskService = sbTaskService;
            _eventAggregator = eventAggregator;

            _serialPort = new SerialPort();
            _serialPort.PortName = Properties.Settings.Default.PLCComPort;
            _serialPort.BaudRate = 9600;
            _serialPort.DataBits = 8;
            _serialPort.Parity = Parity.None;
            _serialPort.StopBits = StopBits.One;
            _serialPort.Handshake = Handshake.RequestToSend;
            _serialPort.ReceivedBytesThreshold = 8;


        }

        public void process(SBTask sbtask)
        {
            // Próba otwarcia portu szeregowego
            try
            {
                _serialPort.Open();
                // wyczyszczenie buforów wejściowego i wyjściowego
                _serialPort.DiscardInBuffer();
                _serialPort.DiscardOutBuffer();

                // Ustaw status tasku jako przetwarzany
                _sbTaskService.SetRunning(sbtask);

                // ======== GO COMMAND ========
                // przygotowanie komendy wydania przedmiotu
                var goCommand = prepareGOCommand(sbtask.Box.AddressRow, sbtask.Box.AddressCol, sbtask.Box.BoxSize.BoxSizeID);

                _serialPort.WriteTimeout = 500;
                // wysłanie komendy wydania przedmiotu z czasem oczekiwania na wysłanie 500 ms
                _serialPort.Write(goCommand, 0, 8);

                // ======== OK RESPONSE ========
                byte[] goResponse = new byte[8];

                _serialPort.ReadTimeout = 2000;
                int bytesRead = 0;
                for (int i=0; i<(_serialPort.ReadTimeout/100); i++)
                {
                    if (_serialPort.BytesToRead == 8)
                    {
                        bytesRead = _serialPort.Read(goResponse, 0, 8);
                        break;
                    }

                    else
                        Thread.Sleep(100);
                }
                if (bytesRead == 0)
                    throw new TimeoutException();
                
                // W przypadku odebrania kodu błędu zgłoś wyjątek
                if (goResponse[0] == 'E')
                {
                    _serialPort.Close();
                    _sbTaskService.SetFailed(sbtask);

                    string message = goResponse[1].ToString();
                    Console.WriteLine(message);
                    throw new ErrorMessageException();
                }
                Console.WriteLine("*********************** " + BitConverter.ToString(goResponse) + " " + bytesRead);

                // ======== OP RESPONSE ========
                bytesRead = 0;
                byte[] opResponse = new byte[8];
                _serialPort.ReadTimeout = 30000;
                for (int i = 0; i < (_serialPort.ReadTimeout / 100); i++)
                {
                    if (_serialPort.BytesToRead == 8)
                    {
                        // Odebranie potwierdzenia otwarcia drzwiczek
                        bytesRead = _serialPort.Read(opResponse, 0, 8);
                        break;
                    }
                    else
                        Thread.Sleep(100);
                }
                if (bytesRead == 0)
                    throw new TimeoutException();

                // W przypadku odebrania kodu błędu zgłoś wyjątek
                if (opResponse[0] == 'E')
                {
                    _serialPort.Close();
                    _sbTaskService.SetFailed(sbtask);
                    string errorCode = System.Text.Encoding.UTF8.GetString(opResponse, 1, 1);
                    throw new ErrorMessageException(errorCode);
                }
                Console.WriteLine(BitConverter.ToString(opResponse) + " " + bytesRead);

                // 
                _eventAggregator.PublishOnUIThread(sbtask);
                _sbTaskService.SetCompleted(sbtask);
                //_boxService.Empty(sbtask.Box);

                // ======== CL COMMAND ========
                // przygotowanie komendy zamknięcia drzwiczek
                byte[] clCommand = prepareCLCommand(sbtask.Box.AddressRow, sbtask.Box.AddressCol, sbtask.Box.BoxSize.BoxSizeID);

                _serialPort.WriteTimeout = 500;
                // wysłanie komendy wydania przedmiotu z czasem oczekiwania na wysłanie 500 ms
                _serialPort.Write(clCommand, 0, 8);

                // ======== DN RESPONSE ========
                bytesRead = 0;
                byte[] clResponse = new byte[8];
                _serialPort.ReadTimeout = 2000;
                // Odebranie potwierdzenia otrzymania komendy wydania przedmiotu 
                // z czasem oczekiwania 2 s.
                for (int i = 0; i<(_serialPort.ReadTimeout/100); i++)
                {
                    if (_serialPort.BytesToRead == 8)
                    {
                        bytesRead = _serialPort.Read(clResponse, 0, 8);
                        break;
                    }
                    else
                        Thread.Sleep(100);
                }
                if (bytesRead == 0)
                    throw new TimeoutException();

                // W przypadku odebrania kodu błędu zgłoś wyjątek
                if (goResponse[0] == 'E')
                {
                    _serialPort.Close();
                    string errorCode = System.Text.Encoding.UTF8.GetString(clResponse, 1, 1);
                    throw new ErrorMessageException(errorCode);
                }

                // Zamknięcie portu szeregowego po zakończonym przetważaniu tasku.
                _serialPort.Close();
            }
            catch (ErrorMessageException ex)
            {
                if (_serialPort.IsOpen)
                    _serialPort.Close();
                Trace.WriteLine("*********************[RealSBTaskPocessor.process] Error Message Exception: ");
                throw new ErrorMessageException(ex.Message);
            }
            // jeśli write się nie wykona
            catch (TimeoutException e)
            {
                _serialPort.Close();
                _sbTaskService.SetFailed(sbtask);
                Trace.WriteLine("*********************[RealSBTaskPocessor.process] Timeout Exception: ");
                throw new TimeoutException();
            }
            // jeśli port szeregowy nie może być otwarty
            catch (Exception)
            {
                if (_serialPort.IsOpen)
                    _serialPort.Close();
                Trace.WriteLine("*********************Problem z otwarciem portu szeregowego: " + _serialPort.PortName);
            }
        }

        
        private byte[] prepareGOCommand(byte row, byte column, int boxSizeID)
        // args: sbtask.Box.AddressRow; sbtask.Box.AddressCol; sbtask.Box.BoxSize.BoxSizeID
        {
            byte[] toSend = { 71, 79, 0, 0, 0, 0, 0, 0 };
            toSend[3] = row;
            toSend[5] = column;
            toSend[7] = Convert.ToByte(boxSizeID);
            return toSend;
        }

        private byte[] prepareCLCommand(byte row, byte column, int boxSizeID)
        {
            byte[] toSend = { 67, 76, 0, 0, 0, 0, 0, 0 };
            toSend[3] = row;
            toSend[5] = column;
            toSend[7] = Convert.ToByte(boxSizeID);
            return toSend;
        }
    }
}
