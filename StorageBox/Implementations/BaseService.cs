using StorageBox.Models;

namespace StorageBox.Implementations
{
    public class BaseService
    {
        protected MyDBContext _context;
        public BaseService()
        {
            _context = new MyDBContext();
        }
    }
}
