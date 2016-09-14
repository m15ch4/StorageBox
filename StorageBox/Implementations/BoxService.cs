using StorageBox.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StorageBox.Models;

namespace StorageBox.Implementations
{
    public class BoxService : IBoxService
    {
        private MyDBContext _context;
        public BoxService(MyDBContext context)
        {
            _context = context;
        }

        public int Count(ProductSKU productSKU)
        {
            return productSKU.Boxes.Count();
        }

        public bool Empty(Box box)
        {
            try
            {
                box.Status = Status.Empty;
                box.ProductSKU = null;
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Fill(Box box)
        {
            try
            {
                box.Status = Status.Full;
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public ICollection<Box> Get(ProductSKU productSKU)
        {
            return productSKU.Boxes;
        }

        public IEnumerable<Box> Get(ProductSKU productSKU, int numberOfRecords)
        {
            return productSKU.Boxes.Take(numberOfRecords);
        }

        public bool Reserve(Box box)
        {
            try
            {
                box.Status = Status.Reserved;
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
