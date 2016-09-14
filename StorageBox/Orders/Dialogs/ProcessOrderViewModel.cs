using Caliburn.Micro;
using StorageBox.Contracts;
using StorageBox.Framework;
using StorageBox.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBox.Orders.Dialogs
{
    public class ProcessOrderViewModel : Screen, IDialog
    {
        private BackgroundWorker _bw = new BackgroundWorker();
        private BindableCollection<WishListItem> _orderQueue; 
        private IBoxService _boxService;
        private BindableCollection<SBTask> _sbTasks;
        private ISBTaskService _sbTaskService;
        private string _tbProgress;

        public ProcessOrderViewModel(BindableCollection<WishListItem> orderQueue, IBoxService boxService, ISBTaskService sbTaskService)
        {
            _orderQueue = orderQueue;
            _boxService = boxService;
            _sbTaskService = sbTaskService;

            SBTasks = _sbTaskService.CreateSBTasks(orderQueue, boxService);

            _bw.WorkerReportsProgress = true;
            _bw.WorkerSupportsCancellation = true;
            _bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            _bw.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
            _bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
            _bw.RunWorkerAsync();

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
            TryClose();
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


        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            int p = 0;

            foreach (SBTask sbtask in SBTasks)
            {
                if ((worker.CancellationPending == true))
                {
                    e.Cancel = true;
                    break;
                }
                else
                {
                    p++;
                    //perform long running task
                    // send command to plc (command code, row, column)
                    // recieve response
                    // wait for report that the door has opened - success
                    _boxService.Empty(sbtask.Box);

                    System.Threading.Thread.Sleep(1000);
                    worker.ReportProgress((int)(p *100.0 / SBTasks.Count));
                }
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


    }
}
