using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBox.Models
{
    public class SBRole
    {
        public SBRole()
        {

        }

        public int SBRoleID { get; set; }
        public string SBRoleName { get; set; }

        public virtual ICollection<SBUser> SBUsers { get; set; }

    }
}
