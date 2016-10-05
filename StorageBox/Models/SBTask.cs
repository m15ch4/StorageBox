using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace StorageBox.Models
{

    public enum SBTaskType
    {
        [Description("Pobranie")]
        Order,
        Return,
        Fill
    }

    public enum SBTaskStatus
    {
        Queued,
        Running,
        Completed,
        Failed
    }
    public class SBTask : PropertyChangedBase
    {
        public int SBTaskID { get; set; }
        public SBTaskStatus SBTaskStatus { get; set; }
        public SBTaskType SBTaskType { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? DateStarted { get; set; }
        public DateTime? DateEnded { get; set; }
        public bool IsValid { get; set; }

        public virtual Box Box { get; set; }
        public virtual ProductSKU ProductSKU { get; set; }
        public virtual SBUser SBUser { get; set; }


        public SBTaskStatus SetStatus
        {
            get { return SBTaskStatus; }
            set
            {
                SBTaskStatus = value;
                NotifyOfPropertyChange(() => SetStatus);
                NotifyOfPropertyChange(() => SBTaskStatusImage);
            }
        }

        public string IsValidDescription
        {
            get
            {
                string result = "";
                switch (IsValid)
                {
                    case true:
                        result = "Tak";
                        break;
                    case false:
                        result = "Nie";
                        break;
                }
                return result;
            }
        }

        public string SBTaskStatusDescription
        {
            get { return SBTaskStatus.ToString(); }
        }


        public string SBTaskTypeDescription
        {
            get
            {
                string result = "";
                switch (SBTaskType)
                {
                    case SBTaskType.Order:
                        result = "Pobranie";
                        break;
                    case SBTaskType.Return:
                        result = "Zwrot";
                        break;
                    case SBTaskType.Fill:
                        result = "Napełnienie";
                        break;
                }
                return result;
            }
        }


        public string SBTaskStatusDescriptionPL
        {
            get
            {
                string result = "";
                switch (SBTaskStatus)
                {
                    case SBTaskStatus.Queued:
                        result = "W kolejce";
                        break;
                    case SBTaskStatus.Running:
                        result = "Przetwarzanie";
                        break;
                    case SBTaskStatus.Failed:
                        result = "Błąd";
                        break;
                    case SBTaskStatus.Completed:
                        result = "Zakończone";
                        break;
                }
                return result;
            }
        }

        public BitmapImage SBTaskStatusImage
        {
            get
            {
                BitmapImage statusImage;
                switch (SBTaskStatus)
                {
                    case SBTaskStatus.Queued:
                        statusImage = new BitmapImage(new Uri("E:/queued.png"));
                        break;
                    case SBTaskStatus.Running:
                        statusImage = new BitmapImage(new Uri("E:/running.jpg"));
                        break;
                    case SBTaskStatus.Completed:
                        statusImage = new BitmapImage(new Uri("E:/completed.png"));
                        break;
                    case SBTaskStatus.Failed:
                        statusImage = new BitmapImage(new Uri("E:/failed.png"));
                        break;
                    default:
                        statusImage = new BitmapImage();
                        break;
                }
                return statusImage;
            }
        }
    }
}
