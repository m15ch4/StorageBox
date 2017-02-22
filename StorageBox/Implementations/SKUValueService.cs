using StorageBox.Contracts;
using StorageBox.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace StorageBox.Implementations
{
    public class SKUValueService : ISKUValueService
    {
        private MyDBContext _context;
        public SKUValueService(MyDBContext context)
        {
            _context = context;
        }
        public void Create(int productID, int productSKUID, int optionID, int optionValueID)
        {
            try
            {
                SKUValue skuValue = new SKUValue() { ProductID = productID, ProductSKUID = productSKUID, OptionID = optionID, OptionValueID = optionValueID };
                _context.SKUValues.Add(skuValue);
                _context.SaveChanges();
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException e)
            {
                throw e;
            }
        }


        public BindableCollection<SKUValue> GetAll()
        {
            List<SKUValue> skuValues = _context.SKUValues.ToList();
            return new BindableCollection<SKUValue>(skuValues);
        }

        public BindableCollection<SKUValue> Get(Product product)
        {
            try
            {
                List<SKUValue> skuValues = _context.SKUValues.Where(sv => sv.ProductID == product.ProductID).ToList();
                return new BindableCollection<SKUValue>(skuValues);
            }
            catch
            {
                Console.Write("Exception: SKUValueService.Get(Product product");
                List<SKUValue> skuValues = _context.SKUValues.ToList();
                return new BindableCollection<SKUValue>(skuValues);
            }
        }

        public void Remove(SKUValue skuValue)
        {
            try
            {
                _context.SKUValues.Remove(skuValue);
                _context.SaveChanges();
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException e)
            {
                throw e;
            }
        }
    }
}
