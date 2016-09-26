using Caliburn.Micro;
using StorageBox.Contracts;
using StorageBox.Framework;
using StorageBox.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBox.Additions.ViewModels
{
    public class UsersViewModel : Screen, IAddition
    {
        private ISBUserService _sbUserService;
        private string _userName;
        private string _firstName;
        private string _lastName;
        private string _password;
        private string _rfid;
        private BindableCollection<SBRole> _sbRoles;
        private SBRole _sbRolesSelectedItem;
        private string _passwordConfirm;
        private BindableCollection<SBUser> _sbUsers;

        public UsersViewModel(ISBUserService sbUserService)
        {
            _sbUserService = sbUserService;
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            SBRoles = _sbUserService.GetAllRoles();
            SBUsers = _sbUserService.GetAll();
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

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                NotifyOfPropertyChange(() => FirstName);
            }
        }

        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                NotifyOfPropertyChange(() => LastName);
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

        public string PasswordConfirm
        {
            get { return _passwordConfirm; }
            set
            {
                _passwordConfirm = value;
                NotifyOfPropertyChange(() => PasswordConfirm);
            }
        }

        public string RFID
        {
            get { return _rfid; }
            set
            {
                _rfid = value;
                NotifyOfPropertyChange(() => RFID);
            }
        }

        public BindableCollection<SBRole> SBRoles
        {
            get { return _sbRoles; }
            set
            {
                _sbRoles = value;
                NotifyOfPropertyChange(() => SBRoles);
            }
        }

        public SBRole SBRolesSelectedItem
        {
            get { return _sbRolesSelectedItem; }
            set
            {
                _sbRolesSelectedItem = value;
                NotifyOfPropertyChange(() => SBRolesSelectedItem);
            }
        }

        public void AddUser(string userName, string firstName, string lastName, string password, string passwordConfirm, string rfid, SBRole sbRole)
        {
            if ((userName == "") || (firstName == "") || (lastName == "") || (password == "") || (sbRole == null))
            {
                return;
            }
            if (password != passwordConfirm)
            {
                Password = "";
                PasswordConfirm = "";
                return;
            }
            if (_sbUserService.Get(userName) == null)
            {
                _sbUserService.Create(userName, firstName, lastName, password, rfid, sbRole);
                UserName = "";
                FirstName = "";
                LastName = "";
                Password = "";
                PasswordConfirm = "";
                RFID = "";
                SBUsers = _sbUserService.GetAll();
            }
            else
            {
                UserName = "";
            }
        }

        public BindableCollection<SBUser> SBUsers
        {
            get { return _sbUsers; }
            set
            {
                _sbUsers = value;
                NotifyOfPropertyChange(() => SBUsers);
            }
        }

        public void RemoveUser(SBUser sbuser)
        {
            if (sbuser != null)
            {
                _sbUserService.RemoveUser(sbuser);
                SBUsers = _sbUserService.GetAll();
            }
        }

        public void ReadRFIDCode()
        {
            byte[] array = new byte[4];
            SerialPort _serialPort = new SerialPort();
            _serialPort.PortName = "COM3";
            _serialPort.BaudRate = 9600;
            _serialPort.DataBits = 8;
            _serialPort.Parity = Parity.None;
            _serialPort.StopBits = StopBits.One;
            _serialPort.Handshake = Handshake.XOnXOff;

            _serialPort.ReadTimeout = 5000;
            _serialPort.WriteTimeout = 500;

            try
            {
                _serialPort.Open();

                _serialPort.Read(array, 0, 4);
                _serialPort.Read(array, 0, 4);
                string hex = BitConverter.ToString(array);
                RFID = hex;
                _serialPort.Close();
            }
            catch
            {
                Trace.WriteLine("Błąd portu COM3");
            }
        }
    }
}
