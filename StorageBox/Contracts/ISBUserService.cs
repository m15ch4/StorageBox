using StorageBox.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBox.Contracts
{
    public interface ISBUserService
    {
        SBUser Get(string username);
        SBUser GetByRFID(string rfid);
    }
}
