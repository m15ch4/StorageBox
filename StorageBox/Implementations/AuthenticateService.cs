using StorageBox.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StorageBox.Models;

namespace StorageBox.Implementations
{
    public class AuthenticateService : IAuthenticationService
    {
        private MyDBContext _context;

        public AuthenticateService(MyDBContext context)
        {
            _context = context;
        }

        public SBUser Authenticate(string rfid)
        {
            SBUser sbuser = _context.SBUsers.Where(u => u.RFID == rfid).FirstOrDefault();
            return sbuser;
        }

        public SBUser Authenticate(string username, string password)
        {
            SBUser sbuser = _context.SBUsers.Where(u => (u.UserName == username) && (u.Password == password)).FirstOrDefault();
            return sbuser;
        }
    }
}
