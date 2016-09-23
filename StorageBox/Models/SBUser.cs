using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBox.Models
{


    public class SBUser
    {
        public int SBUserID { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string RFID { get; set; }

        public virtual SBRole Role { get; set; }

        public virtual ICollection<SBTask> SBTasks { get; set; }

        public string DisplayName
        {
            get
            {
                return FirstName + " " + LastName.ToUpper() + " [" + UserName + "]";
            }
        }
    }
}
