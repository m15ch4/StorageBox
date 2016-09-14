using StorageBox.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StorageBox.Models;

namespace StorageBox.Implementations
{
    class SBUserService : ISBUserService
    {
        private MyDBContext _context;
        public SBUserService(MyDBContext context)
        {
            _context = context;
        }

        public SBUser Get(string username)
        {
            return _context.SBUsers.Where(u => u.UserName == username).FirstOrDefault();
        }

        public SBUser GetByRFID(string rfid)
        {
            return _context.SBUsers.Where(u => u.RFID == rfid).FirstOrDefault();
        }
    }
}
