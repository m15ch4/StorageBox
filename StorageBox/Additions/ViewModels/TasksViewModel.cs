using Caliburn.Micro;
using Microsoft.Win32;
using StorageBox.Contracts;
using StorageBox.Framework;
using StorageBox.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBox.Additions.ViewModels
{
    public class TasksViewModel : Screen, IAddition
    {
        private BindableCollection<SBTask> _sbtasks;
        private ISBTaskService _sbTaskService;

        public TasksViewModel(ISBTaskService sbTaskService)
        {
            _sbTaskService = sbTaskService;

            
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            SBTasks = _sbTaskService.GetAll();
        }

        public BindableCollection<SBTask> SBTasks
        {
            get { return _sbtasks; }
            set
            {
                _sbtasks = value;
                NotifyOfPropertyChange(() => SBTasks);
            }
        }

        public void ExportCSV()
        {
            string filename = "";
            SaveFileDialog fileDialog = new SaveFileDialog();

            fileDialog.Filter = "txt files (*.csv)|*.csv|All files (*.*)|*.*";
            fileDialog.AddExtension = true;
            fileDialog.DefaultExt = "csv";

            if (fileDialog.ShowDialog() == true)
            {

                filename = fileDialog.FileName;

                var csv = new StringBuilder();

                foreach (SBTask sbtask in SBTasks)
                {
                    var sbTaskTypeDescription = sbtask.SBTaskTypeDescription;
                    var sbTaskStatusDescription = sbtask.SBTaskStatusDescription;
                    var category = sbtask.CategoryName;
                    var product = sbtask.ProductName;
                    var sku = sbtask.SKU;
                    var dateAdded = sbtask.DateAdded.ToString();
                    var dateStarted = sbtask.DateStarted.ToString();
                    var dateEnded = sbtask.DateEnded.ToString();
                    var user = (sbtask.SBUser != null) ? sbtask.SBUser.DisplayName : "";
                    var isvalid = sbtask.IsValidDescription;

                    var newLine = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}", sbTaskTypeDescription, sbTaskStatusDescription, category, product, sku, dateAdded, dateStarted, dateEnded, user, isvalid);
                    csv.AppendLine(newLine);
                }

                File.WriteAllText(filename, csv.ToString(), Encoding.UTF8);
            }
        }

    }
}
