using Caliburn.Micro;
using StorageBox.Contracts;
using StorageBox.Framework;
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
    public class ProcessOrderViewModel : Screen, IDialog, IHandle<SBTask>
    {
        private BackgroundWorker _bw = new BackgroundWorker();
        private BindableCollection<WishListItem> _orderQueue; 
        private IBoxService _boxService;
        private BindableCollection<SBTask> _sbTasks;
        private ISBTaskService _sbTaskService;
        private string _tbProgress;

        static bool _continue;
        static SerialPort _serialPort;
        static bool _nextCommand;
        static byte[] response = new byte[8];
        private IWindowManager _windowManager;
        private IEventAggregator _eventAggregator;

        public ProcessOrderViewModel(IWindowManager windowManager, IEventAggregator eventAggregator, BindableCollection<WishListItem> orderQueue, IBoxService boxService, ISBTaskService sbTaskService)
        {
            _orderQueue = orderQueue;
            _boxService = boxService;
            _sbTaskService = sbTaskService;
            _windowManager = windowManager;
            _eventAggregator = eventAggregator;

            _eventAggregator.Subscribe(this);

            SBTasks = _sbTaskService.CreateSBTasks(orderQueue, boxService);

            _bw.WorkerReportsProgress = true;
            _bw.WorkerSupportsCancellation = true;
            _bw.DoWork += new DoWorkEventHandler(bw_DoWork);
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



        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {

            BackgroundWorker worker = sender as BackgroundWorker;

            Thread readThread = new Thread(Read);
            _serialPort = new SerialPort();

            _serialPort = new SerialPort();
            _serialPort.PortName = Properties.Settings.Default.PLCComPort;
            _serialPort.BaudRate = 9600;
            _serialPort.DataBits = 8;
            _serialPort.Parity = Parity.None;
            _serialPort.StopBits = StopBits.One;
            _serialPort.Handshake = Handshake.XOnXOff;

            _serialPort.ReadTimeout = 500;
            _serialPort.WriteTimeout = 500;


            if (Properties.Settings.Default.Demo == false)
            {
                try
                {
                    // UNCOMMENT !!!!!!!!!!!!!!!
                    _serialPort.Open();
                    _continue = true;
                    _nextCommand = true;
                    // UNCOMMENT !!!!!!!!!!!!!!!
                    readThread.Start();

                    byte[] toSend = { 71, 79, 0, 0, 0, 0, 0, 0 };

                    int progress = 0;

                    foreach (SBTask sbtask in SBTasks)
                    {
                        if ((worker.CancellationPending == true))
                        {
                            e.Cancel = true;
                            break;
                        }
                        else
                        {
                            // COMMENT !!!!!!!!!!!!!!!
                            //response[0] = 0;
                            //response[1] = 0;
                            //response[2] = 0;
                            //response[3] = 0;
                            //response[4] = 0;
                            //response[5] = 0;
                            //response[6] = 0;
                            //response[7] = 0;
                            // Update Task status and set DateStarted
                            _sbTaskService.SetRunning(sbtask);
                            progress++;
                            //perform long running task
                            // send command to plc (command code, row, column)
                            // recieve response
                            // wait for report that the door has opened - success
                            toSend[3] = sbtask.Box.AddressRow;
                            toSend[5] = sbtask.Box.AddressCol;
                            toSend[7] = Convert.ToByte(sbtask.Box.BoxSize.BoxSizeID);

                            // UNCOMMENT !!!!!!!!!!!!!!!
                            _serialPort.Write(toSend, 0, 8);
                            Console.WriteLine("Sending: " + BitConverter.ToString(toSend));
                            // UNCOMMENT !!!!!!!!!!!!!!!
                            _nextCommand = false;

                            Console.WriteLine("Command sent.");

                            Thread.Sleep(2000);

                            while (!_nextCommand)
                            {

                            }
                            if ((response[0] == 'E') && (response[1] == 'R'))
                            {
                                _sbTaskService.SetFailed(sbtask);
                            }
                            else
                            {
                                _sbTaskService.SetCompleted(sbtask);
                                _boxService.Empty(sbtask.Box);


                                _eventAggregator.PublishOnUIThread(sbtask);
                                //dynamic mysettings = new ExpandoObject();
                                //mysettings.WindowStartupLocation = WindowStartupLocation.CenterOwner;

                                //var dialog_result = _windowManager.ShowDialog(new ConfirmItemViewModel(), null, mysettings);
                                //if (dialog_result)
                                //{
                                //    Trace.WriteLine("CONFIRMED");
                                //}
                                //else
                                //{
                                //    Trace.WriteLine("NOT CONFIRMED");
                                //}
                            }



                            //.SBTaskStatusImage = new BitmapImage(new Uri("E:/completed.png"));
                            //NotifyOfPropertyChange(() => sbtask.SBTaskStatusImage);
                            //NotifyOfPropertyChange(() => SBTasks);

                            //char c = toSend[0];
                            //int i = (int)c;
                            //toSend[0]++;
                            //toSend[0] = (char)i;
                            //byte[] response = new byte[8];
                            //int bytesRead = _serialPort.Read(response, 0, 8);
                            //Trace.WriteLine(BitConverter.ToString(response));
                            //SBTasks.Remove(sbtask);
                            worker.ReportProgress((int)(progress * 100.0 / SBTasks.Count));
                        }
                    }

                    _continue = false;
                    // UNCOMMENT !!!!!!!!!!!!!!!!!!!!!!!!
                    readThread.Join();
                    // UNCOMMENT !!!!!!!!!!!!!!!!!!!!!!!!
                    _serialPort.Close();

                    //SEND EMAIL

                    List<ProductSKU> underThreshold = _sbTaskService.taskedSKUs(SBTasks).Where(s => s.Threshold != 0).Where(s => s.Threshold >= s.Boxes.Count).ToList();

                    if (underThreshold.Count > 0)
                    {

                        SmtpClient smtpClient = new SmtpClient();
                        NetworkCredential basicCredential =
                            new NetworkCredential(Properties.Settings.Default.ServiceEmail, Properties.Settings.Default.EmailPassword);
                        MailMessage message = new MailMessage();
                        MailAddress fromAddress = new MailAddress(Properties.Settings.Default.ServiceEmail);

                        smtpClient.Host = Properties.Settings.Default.SMTPServer;
                        smtpClient.UseDefaultCredentials = false;
                        smtpClient.Credentials = basicCredential;
                        //smtpClient.EnableSsl = true;

                        message.From = fromAddress;
                        message.Subject = "Powiadomienie o wyczerpywanych zasobach";
                        //Set IsBodyHtml to true means you can send HTML email.
                        message.IsBodyHtml = true;

                        message.Body = "<h1>Powiadomienie</h1>";
                        message.Body += "<ul>";
                        foreach (ProductSKU productsku in underThreshold)
                        {
                            message.Body += "<li>" + productsku.Product.ProductName + " [" + productsku.Sku + "] - Dostępnych: " + productsku.Boxes.Count + "</li>";
                        }
                        message.Body += "</ul>";

                        message.To.Add(Properties.Settings.Default.RecipientEmail);

                        Trace.WriteLine("Sending...");

                        try
                        {
                            smtpClient.Send(message);
                            Trace.WriteLine("Sent.");
                        }
                        catch (Exception ex)
                        {
                            //Error, could not send the message
                            Trace.Write(ex.Message);
                        }
                    }

                }
                catch
                {
                    Trace.WriteLine("Błąd połączenia ze sterownikiem PLC.");

                }
            }
            else
            {
                ////// start demo
                try
                {
                    // UNCOMMENT !!!!!!!!!!!!!!!
                    //_serialPort.Open();
                    _continue = true;
                    _nextCommand = true;
                    // UNCOMMENT !!!!!!!!!!!!!!!
                    //readThread.Start();

                    byte[] toSend = { 71, 79, 0, 0, 0, 0, 0, 0 };

                    int progress = 0;

                    foreach (SBTask sbtask in SBTasks)
                    {
                        if ((worker.CancellationPending == true))
                        {
                            e.Cancel = true;
                            break;
                        }
                        else
                        {
                            // COMMENT !!!!!!!!!!!!!!!
                            //response[0] = 0;
                            //response[1] = 0;
                            //response[2] = 0;
                            //response[3] = 0;
                            //response[4] = 0;
                            //response[5] = 0;
                            //response[6] = 0;
                            //response[7] = 0;
                            // Update Task status and set DateStarted
                            _sbTaskService.SetRunning(sbtask);
                            progress++;
                            //perform long running task
                            // send command to plc (command code, row, column)
                            // recieve response
                            // wait for report that the door has opened - success
                            toSend[3] = sbtask.Box.AddressRow;
                            toSend[5] = sbtask.Box.AddressCol;
                            toSend[7] = Convert.ToByte(sbtask.Box.BoxSize.BoxSizeID);

                            // UNCOMMENT !!!!!!!!!!!!!!!
                            //_serialPort.Write(toSend, 0, 8);
                            Console.WriteLine("Sending: " + BitConverter.ToString(toSend));
                            // UNCOMMENT !!!!!!!!!!!!!!!
                            //_nextCommand = false;

                            Console.WriteLine("Command sent.");

                            //Thread.Sleep(2000);

                            while (!_nextCommand)
                            {

                            }
                            if ((response[0] == 'E') && (response[1] == 'R'))
                            {
                                _sbTaskService.SetFailed(sbtask);
                            }
                            else
                            {
                                _sbTaskService.SetCompleted(sbtask);
                                _boxService.Empty(sbtask.Box);

                                Console.WriteLine("Sending Message");
                                _eventAggregator.PublishOnUIThread(sbtask);
                                //dynamic mysettings = new ExpandoObject();
                                //mysettings.WindowStartupLocation = WindowStartupLocation.CenterOwner;

                                //var dialog_result = _windowManager.ShowDialog(new ConfirmItemViewModel(), null, mysettings);
                                //if (dialog_result)
                                //{
                                //    Trace.WriteLine("CONFIRMED");
                                //}
                                //else
                                //{
                                //    Trace.WriteLine("NOT CONFIRMED");
                                //}
                            }



                            //.SBTaskStatusImage = new BitmapImage(new Uri("E:/completed.png"));
                            //NotifyOfPropertyChange(() => sbtask.SBTaskStatusImage);
                            //NotifyOfPropertyChange(() => SBTasks);

                            //char c = toSend[0];
                            //int i = (int)c;
                            //toSend[0]++;
                            //toSend[0] = (char)i;
                            //byte[] response = new byte[8];
                            //int bytesRead = _serialPort.Read(response, 0, 8);
                            //Trace.WriteLine(BitConverter.ToString(response));
                            //SBTasks.Remove(sbtask);
                            worker.ReportProgress((int)(progress * 100.0 / SBTasks.Count));
                        }
                    }

                    _continue = false;
                    // UNCOMMENT !!!!!!!!!!!!!!!!!!!!!!!!
                    //readThread.Join();
                    // UNCOMMENT !!!!!!!!!!!!!!!!!!!!!!!!
                    //_serialPort.Close();

                    //SEND EMAIL

                    List<ProductSKU> underThreshold = _sbTaskService.taskedSKUs(SBTasks).Where(s => s.Threshold != 0).Where(s => s.Threshold >= s.Boxes.Count).ToList();

                    if (underThreshold.Count > 0)
                    {

                        SmtpClient smtpClient = new SmtpClient();
                        NetworkCredential basicCredential =
                            new NetworkCredential(Properties.Settings.Default.ServiceEmail, Properties.Settings.Default.EmailPassword);
                        MailMessage message = new MailMessage();
                        MailAddress fromAddress = new MailAddress(Properties.Settings.Default.ServiceEmail);

                        smtpClient.Host = Properties.Settings.Default.SMTPServer;
                        smtpClient.UseDefaultCredentials = false;
                        smtpClient.Credentials = basicCredential;
                        //smtpClient.EnableSsl = true;

                        message.From = fromAddress;
                        message.Subject = "Powiadomienie o wyczerpywanych zasobach";
                        //Set IsBodyHtml to true means you can send HTML email.
                        message.IsBodyHtml = true;

                        message.Body = "<h1>Powiadomienie</h1>";
                        message.Body += "<ul>";
                        foreach (ProductSKU productsku in underThreshold)
                        {
                            message.Body += "<li>" + productsku.Product.ProductName + " [" + productsku.Sku + "] - Dostępnych: " + productsku.Boxes.Count + "</li>";
                        }
                        message.Body += "</ul>";

                        message.To.Add(Properties.Settings.Default.RecipientEmail);

                        Trace.WriteLine("Sending...");

                        try
                        {
                            smtpClient.Send(message);
                            Trace.WriteLine("Sent.");
                        }
                        catch (Exception ex)
                        {
                            //Error, could not send the message
                            Trace.Write(ex.Message);
                        }
                    }

                }
                catch
                {
                    Trace.WriteLine("Błąd podczas wykonywania programu demo.");
                }
                ////// koniec demo
            }

        }

        public static void Read()
        {
            
            while (_continue)
            {
                try
                {
                    if (_serialPort.BytesToRead == 8)
                    {
                        int bytesRead = _serialPort.Read(response, 0, 8);
                        //Console.WriteLine(new string(response) + " " + bytesRead);
                        _nextCommand = true;
                    }
                }
                catch (TimeoutException) { }
            }
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
    }
}
