using Caliburn.Micro;
using StorageBox.Contracts;
using StorageBox.Framework;
using StorageBox.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBox.Login
{
    public class LoginViewModel : Screen, IWorkspace
    {
        static bool _continue;
        static SerialPort _serialPort;

        private ILoginService _loginService;
        private IShell _shell;
        private string _userName;
        private string _password;

        private BackgroundWorker _bw = new BackgroundWorker();

        public LoginViewModel(ILoginService loginService)
        {
            _loginService = loginService;

            //CONFIGURE SerialPort
            //start backgroundworker - rfid
            try
            {
                _serialPort = new SerialPort();
                _serialPort.PortName = "COM3";
                _serialPort.BaudRate = 9600;
                _serialPort.DataBits = 8;
                _serialPort.Parity = Parity.None;
                _serialPort.StopBits = StopBits.One;
                _serialPort.Handshake = Handshake.XOnXOff;

                _serialPort.ReadTimeout = 500;
                _serialPort.WriteTimeout = 500;

                _serialPort.Open();
                _continue = true;

                _bw.WorkerReportsProgress = false;
                _bw.WorkerSupportsCancellation = false;
                _bw.DoWork += new DoWorkEventHandler(ReadRFID);
                _bw.RunWorkerAsync();
            }
            catch
            {
                Trace.WriteLine("Problem z komunikacją z RFID (LoginViewModel constructor).");
            }
        }

        override protected void OnActivate()
        {
            try
            {
                if (_serialPort.IsOpen == false)
                {
                    _serialPort.Open();
                    _continue = true;
                    _bw.RunWorkerAsync();
                }
                else
                {
                    _bw.RunWorkerAsync();
                }
            }
            catch
            {
                Trace.WriteLine("Problem z komunikacją z RFID (LoginViewModel.OnActivate).");
            }
        }

        public IShell Shell
        {
            get { return _shell; }
            set
            {
                _shell = value;
                NotifyOfPropertyChange(() => Shell);
            }
        }

        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                NotifyOfPropertyChange(() => UserName);
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                NotifyOfPropertyChange(() => Password);
            }
        }

        public void Login(string username, string password)
        {
            SBUser sbuser = _loginService.Authenticate(username, password);
            if (sbuser != null)
            {
                UserName = "";
                Password = "";
                Shell.Orders();
                _continue = false;
            }
            else
            {
                Trace.WriteLine("Bad username or password");
            }
        }

        private static void ReadRFID(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            while (_continue)
            {
                try
                {
                    string message = _serialPort.ReadLine();
                    Console.WriteLine(message);
                }
                catch (TimeoutException) { }
            }
            _serialPort.Close();
        }

    }
}
