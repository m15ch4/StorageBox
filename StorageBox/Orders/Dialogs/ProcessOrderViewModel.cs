using Caliburn.Micro;
using StorageBox.Contracts;
using StorageBox.Exceptions;
using StorageBox.Framework;
using StorageBox.Implementations;
using StorageBox.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Dynamic;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace StorageBox.Orders.Dialogs
{
    public class ProcessOrderViewModel : Screen, IDialog, IHandle<SBTask>, IHandle<int>
    {
        private BackgroundWorker _bw = new BackgroundWorker();
        private BindableCollection<WishListItem> _orderQueue; 
        private IBoxService _boxService;
        private BindableCollection<SBTask> _sbTasks;
        private ISBTaskService _sbTaskService;
        private IEMailService _emailService;
        private string _tbProgress;

        static bool _continue;
        static SerialPort _serialPort;
        static bool _nextCommand;
        static byte[] response = new byte[8];
        private IWindowManager _windowManager;
        private IEventAggregator _eventAggregator;

        public ProcessOrderViewModel(IWindowManager windowManager, IEventAggregator eventAggregator, BindableCollection<WishListItem> orderQueue, IBoxService boxService, ISBTaskService sbTaskService, IEMailService emailService)
        {
            _orderQueue = orderQueue;
            _boxService = boxService;
            _sbTaskService = sbTaskService;
            _emailService = emailService;
            _windowManager = windowManager;

            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);

            SBTasks = _sbTaskService.CreateSBTasks(orderQueue, boxService);

            _bw.WorkerReportsProgress = true;
            _bw.WorkerSupportsCancellation = true;
            _bw.DoWork += new DoWorkEventHandler(bw_DoWork2);
            _bw.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
            _bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
            _bw.RunWorkerAsync();


        }

        protected override void OnActivate()
        {
            base.OnActivate();
            //Thread.Sleep(100);
            //DoWork();
        }

        public BindableCollection<SBTask> SBTasks
        {
            get { return _sbTasks; }
            set
            {
                _sbTasks = value;
                NotifyOfPropertyChange(() => SBTasks);
            }
        }


        public void CloseDialog()
        {
            TryClose(true);
        }


        public bool CanCloseDialog
        {
            get { return (tbProgress == "Done!"); }
        }


        public string tbProgress
        {
            get { return _tbProgress; }
            set
            {
                _tbProgress = value;
                NotifyOfPropertyChange(() => tbProgress);
                NotifyOfPropertyChange(() => CanCloseDialog);
            }
        }

        private bool OpenConfirmDialog()
        {
            dynamic mysettings = new ExpandoObject();
            mysettings.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            var dialog_result = _windowManager.ShowDialog(new ConfirmItemViewModel(), null, mysettings);
            if (dialog_result)
            {
                Trace.WriteLine("CONFIRMED");
            }
            else
            {
                Trace.WriteLine("NOT CONFIRMED");
            }
            return dialog_result;
        }

        private void bw_DoWork2(object sender, DoWorkEventArgs e)
        {
            // Prepare tasks passed as argument
            BindableCollection<SBTask> tasksToProcess = e.Argument as BindableCollection<SBTask>;

            // Prepare serial port
            // Speed 9600, Data bits 8, Parity none, Stop bits 1, Handshake RTS
            _serialPort = new SerialPort();
            _serialPort.PortName = Properties.Settings.Default.PLCComPort;
            _serialPort.BaudRate = 9600;
            _serialPort.DataBits = 8;
            _serialPort.Parity = Parity.None;
            _serialPort.StopBits = StopBits.One;
            _serialPort.Handshake = Handshake.RequestToSend;

            ISBTaskProcessor sbTaskProcessor;
            if (Properties.Settings.Default.Demo == false)
                sbTaskProcessor = new RealSBTaskProcessor(_sbTaskService, _eventAggregator);
            else
                sbTaskProcessor = new DemoSBTaskProcessor(_sbTaskService, _eventAggregator);

            foreach (SBTask sbtask in SBTasks)
            {
                try
                {
                    sbTaskProcessor.process(sbtask);
                    Trace.WriteLine("################### Tutaj mnie nie powinno być w przypadku kodu błędu");
                    _boxService.Empty(sbtask.Box);
                    // Pauza 1s dająca czas sterownikowi PLC na przygotowanie do przyjęcia komendy
                    Thread.Sleep(1000);
                }
                catch (ErrorMessageException ex)
                {
                    Trace.WriteLine("################## Error message recieved " + ex.Message);
                    _eventAggregator.PublishOnUIThread(1);
                    break;
                }
                catch (TimeoutException ex)
                {
                    Trace.WriteLine("##################[RealSBTaskPocessor.process] Timeout Exception: ");
                    _eventAggregator.PublishOnUIThread(2);
                    break;
                }
                catch (Exception)
                {
                    Trace.WriteLine("##################Problem z otwarciem portu szeregowego: " + _serialPort.PortName);
                    _eventAggregator.PublishOnUIThread(3);
                    break;
                }

            }
            // Prepare data and send availability warning email.
            List<ProductSKU> underThreshold = _sbTaskService.taskedSKUs(SBTasks).Where(s => s.Threshold != 0).Where(s => s.Threshold >= s.Boxes.Count).ToList();
            _emailService.sendAvailabilityWarning(underThreshold);

            Thread.Sleep(100);
            CloseDialog();
        }


        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if ((e.Cancelled == true))
            {
                tbProgress = "Canceled!";
            }

            else if (!(e.Error == null))
            {
                tbProgress = ("Error: " + e.Error.Message);
            }

            else
            {
                tbProgress = "Done!";
            }
        }

        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            tbProgress = (e.ProgressPercentage.ToString() + "%");
            Console.WriteLine(tbProgress);
        }

        public void Handle(SBTask sbtask)
        {
            Trace.WriteLine("Message recieved");
            if (sbtask.IsValid == null)
            {
                bool result = OpenConfirmDialog();
                _sbTaskService.SetValid(sbtask, result);
            }
        }

        public void Handle(int message)
        {
            string messageText = "";
            switch (message)
            {
                // Error message
                case 1:
                    messageText = "Wystąpił problem z przetwarzaniem zamówienia. Odebrano kod błędu. Proces został przerwany";
                    break;
                // Timeout
                case 2:
                    messageText = "Wystąpił problem z przetwarzaniem zamówienia. Upłynął limit czasu oczekiwania na realizację zamówienia. Proces został przerwany";
                    break;
                // General exception (error opening port)
                case 3:
                    messageText = "Wystąpił problem z przetwarzaniem zamówienia. Problem z komunikacją ze sterownikiem PLC. Proces został przerwany";
                    break;
            }
            MessageBox.Show(messageText, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
