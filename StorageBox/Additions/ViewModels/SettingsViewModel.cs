using Caliburn.Micro;
using StorageBox.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBox.Additions.ViewModels
{
    public class SettingsViewModel : Screen, IAddition
    {
        private string _plcComPort;
        private string _serviceEmail;
        private string _recipientEmail;
        private string _smtpServer;
        private string _emailPassword;

        public SettingsViewModel()
        {

        }

        protected override void OnActivate()
        {
            base.OnActivate();

            PLCComPort = Properties.Settings.Default.PLCComPort;

            ServiceEmail = Properties.Settings.Default.ServiceEmail;
            EmailPassword = Properties.Settings.Default.EmailPassword;
            SMTPServer = Properties.Settings.Default.SMTPServer;
            RecipientEmail = Properties.Settings.Default.RecipientEmail;
        }

        public string PLCComPort
        {
            get { return _plcComPort; }
            set
            {
                _plcComPort = value;
                NotifyOfPropertyChange(() => PLCComPort);
            }
        }

        public string ServiceEmail
        {
            get { return _serviceEmail; }
            set
            {
                _serviceEmail = value;
                NotifyOfPropertyChange(() => ServiceEmail);
            }
        }

        public string EmailPassword
        {
            get { return _emailPassword; }
            set
            {
                _emailPassword = value;
                NotifyOfPropertyChange(() => EmailPassword);
            }
        }

        public string RecipientEmail
        {
            get { return _recipientEmail; }
            set
            {
                _recipientEmail = value;
                NotifyOfPropertyChange(() => RecipientEmail);
            }
        }

        public string SMTPServer
        {
            get { return _smtpServer; }
            set
            {
                _smtpServer = value;
                NotifyOfPropertyChange(() => SMTPServer);
            }
        }

        public void SaveSettings()
        {
            Properties.Settings.Default.PLCComPort = PLCComPort;
            Properties.Settings.Default.ServiceEmail = ServiceEmail;
            Properties.Settings.Default.EmailPassword = EmailPassword;
            Properties.Settings.Default.SMTPServer = SMTPServer;
            Properties.Settings.Default.RecipientEmail = RecipientEmail;
            Properties.Settings.Default.Save();
        }

    }
}
