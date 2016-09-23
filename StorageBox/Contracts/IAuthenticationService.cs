using StorageBox.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBox.Contracts
{
    public interface IAuthenticationService
    {
        SBUser Authenticate(string username, string password);
        SBUser Authenticate(string rfid);

    }
}
