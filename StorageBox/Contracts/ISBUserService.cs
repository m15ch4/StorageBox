using Caliburn.Micro;
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
        BindableCollection<SBUser> GetAll();
        BindableCollection<SBUser> GetAllActive();
        void RemoveUser(SBUser sbuser);

        bool Create(string userName, string firstName, string lastName, string password, string rfid, SBRole sbRole);

        // Roles
        BindableCollection<SBRole> GetAllRoles();
    }
}
