using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBox.Models
{

    public enum SBTaskType
    {
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
    public class SBTask
    {
        public int SBTaskID { get; set; }
        public SBTaskStatus SBTaskStatus { get; set; }
        public SBTaskType SBTaskType { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? DateEnded { get; set; }

        [NotMapped]
        public ProductVariant ProductVariant { get; set; }

        public virtual Box Box { get; set; }
        public virtual ProductSKU ProductSKU { get; set; }
        public virtual SBUser SBUser { get; set; }
    }
}
