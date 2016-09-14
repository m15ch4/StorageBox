using StorageBox.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StorageBox.Models;

namespace StorageBox.Implementations
{
    public class LoginService : ILoginService
    {
        private MyDBContext _context;

        public LoginService(MyDBContext context)
        {
            _context = context;
        }

        public SBUser Authenticate(string rfid)
        {
            throw new NotImplementedException();
        }

        public SBUser Authenticate(string username, string password)
        {
            SBUser sbuser = _context.SBUsers.Where(u => (u.UserName == username) && (u.Password == password)).FirstOrDefault();
            return sbuser;
        }
    }
}
