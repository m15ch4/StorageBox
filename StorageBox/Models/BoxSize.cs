using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBox.Models
{
    public class BoxSize
    {
        public int BoxSizeID { get; set; }
        public string BoxSizeName { get; set; }

        public virtual ICollection<Box> Boxes { get; set; }
    }
}
