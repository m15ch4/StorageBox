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

        public void CreateBox(byte row, byte column, BoxSize boxSize)
        {
            Box box = new Models.Box() { AddressRow = row, AddressCol = column, BoxSize = boxSize, Status = Status.Empty };
            _context.Boxes.Add(box);
            _context.SaveChanges();
        }

        public void CreateBoxSize(string boxSizeName)
        {
            BoxSize boxSize = new BoxSize() { BoxSizeName = boxSizeName };
            _context.BoxSizes.Add(boxSize);
            _context.SaveChanges();
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

        public bool Fill(Box box, ProductSKU productSKU)
        {
            try
            {
                box.ProductSKU = productSKU;
                box.Status = Status.Full;
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool FillSingle(ProductSKU productSKU, byte row, byte column)
        {
            Box box = Get(row, column);
            if (box != null)
            {
                return Fill(box, productSKU);
            }
            return false;
        }

        public Box Get(byte row, byte column)
        {
            Box box = _context.Boxes.Where(b => (b.AddressRow == row) && (b.AddressCol == column)).SingleOrDefault();
            return box;
        }

        public ICollection<Box> Get(ProductSKU productSKU)
        {
            return productSKU.Boxes;
        }

        public IEnumerable<Box> Get(ProductSKU productSKU, int numberOfRecords)
        {
            return productSKU.Boxes.Take(numberOfRecords);
        }

        public BindableCollection<BoxSize> GetAllBoxSizes()
        {
            List<BoxSize> boxSizes = _context.BoxSizes.ToList();
            return new BindableCollection<BoxSize>(boxSizes);
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

        public BindableCollection<Box> GetAll()
        {
            List<Box> boxes = _context.Boxes.ToList();
            return new BindableCollection<Box>(boxes);
        }

        public void Remove(Box box)
        {
            _context.Boxes.Remove(box);
            _context.SaveChanges();
        }
    }
}
