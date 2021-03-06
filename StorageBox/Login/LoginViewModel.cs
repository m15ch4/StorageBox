﻿using Caliburn.Micro;
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
using System.Windows;

namespace StorageBox.Login
{
    public class LoginViewModel : Screen, IWorkspace
    {
        static bool _continue;
        static SerialPort _serialPort;

        private IAuthenticationService _authenticateService;
        private IShell _shell;
        private string _userName;
        private string _password;

        private BackgroundWorker _bw = new BackgroundWorker();

        public LoginViewModel(IAuthenticationService authenticateService)
        {
            _authenticateService = authenticateService;

            _serialPort = new SerialPort();
            _serialPort.PortName = "COM3";
            _serialPort.BaudRate = 9600;
            _serialPort.DataBits = 8;
            _serialPort.Parity = Parity.None;
            _serialPort.StopBits = StopBits.One;
            _serialPort.Handshake = Handshake.XOnXOff;

            _serialPort.ReadTimeout = 500;
            _serialPort.WriteTimeout = 500;

            _bw.WorkerReportsProgress = false;
            _bw.WorkerSupportsCancellation = false;
            _bw.DoWork += new DoWorkEventHandler(ReadRFID);
        }


        override protected void OnActivate()
        {
            base.OnActivate();
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
                    Console.WriteLine("---------------> Tutaj nigdy nie powinienem trafić ---------------");
                }
            }
            catch
            {
                MessageBox.Show("Wystąpił problem podczas inicjalizacji czytnika RFID. Skontaktuj się z serwisem.", "Problem", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        protected override void OnDeactivate(bool close)
        {
            _continue = false;
            base.OnDeactivate(close);

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
            SBUser sbuser = _authenticateService.Authenticate(username, password);
            if (sbuser != null)
            {
                UserSession.sbuser = sbuser;
                UserSession.beginDate = DateTime.Now;
                UserName = "";
                Password = "";
                _shell.setUserName();

                if (sbuser.Role.SBRoleID == 1)
                {
                    Shell.Orders();
                }
                else
                {
                    Shell.Additions();
                }

                _continue = false;
            }
            else
            {
                UserName = "";
                Password = "";
            }
        }

        public void Login(string rfid)
        {
            SBUser sbuser = _authenticateService.Authenticate(rfid);
            if (sbuser != null)
            {
                UserSession.sbuser = sbuser;
                UserSession.beginDate = DateTime.Now;
                _shell.setUserName();
                if (sbuser.Role.SBRoleID == 1)
                {
                    Shell.Orders();
                }
                else
                {
                    Shell.Additions();
                }
                _continue = false;
            }
        }

        private void ReadRFID(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            byte[] array = new byte[4];
            while (_continue)
            {
                try
                {
                    _serialPort.Read(array, 0, 4);
                    _serialPort.Read(array, 0, 4);
                    string hex = BitConverter.ToString(array);
                    Console.WriteLine(hex);
                    SBUser sbuser = _authenticateService.Authenticate(hex);
                    if (sbuser != null)
                    {
                        UserSession.sbuser = sbuser;
                        UserSession.beginDate = DateTime.Now;
                        _shell.setUserName();
                        _continue = false;
                        Shell.Orders();
                    }
                    array[0] = 0;
                }
                catch (TimeoutException) { }
            }
            _serialPort.Close();
        }

    }
}
