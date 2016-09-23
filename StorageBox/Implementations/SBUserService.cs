using StorageBox.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StorageBox.Models;
using Caliburn.Micro;

namespace StorageBox.Implementations
{
    class SBUserService : ISBUserService
    {
        private MyDBContext _context;
        public SBUserService(MyDBContext context)
        {
            _context = context;
        }

        public bool Create(string userName, string firstName, string lastName, string password, string rfid, SBRole sbRole)
        {
            try
            {
                SBUser sbuser = new Models.SBUser() { UserName = userName, FirstName = firstName, LastName = lastName, Password = password, RFID = rfid, Role = sbRole };
                _context.SBUsers.Add(sbuser);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public SBUser Get(string username)
        {
            return _context.SBUsers.Where(u => u.UserName == username).FirstOrDefault();
        }

        public BindableCollection<SBUser> GetAll()
        {
            List<SBUser> sbusers = _context.SBUsers.ToList();
            return new BindableCollection<SBUser>(sbusers);
        }

        public BindableCollection<SBRole> GetAllRoles()
        {
            List<SBRole> sbroles = _context.SBRoles.ToList();
            return new BindableCollection<SBRole>(sbroles);
        }

        public SBUser GetByRFID(string rfid)
        {
            return _context.SBUsers.Where(u => u.RFID == rfid).FirstOrDefault();
        }

        public void RemoveUser(SBUser sbuser)
        {
            _context.SBUsers.Remove(sbuser);
            _context.SaveChanges();
        }
    }
}
