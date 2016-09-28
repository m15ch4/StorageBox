using StorageBox.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using StorageBox.Models;

namespace StorageBox.Implementations
{
    class ProductSKUService : IProductSKUService
    {
        private MyDBContext _context;
        public ProductSKUService(MyDBContext context)
        {
            _context = context;
        }

        public void Create(string SKU, Product product, string price)
        {
            ProductSKU productSKU = new ProductSKU() { Sku = SKU, Product = product, Price = price };
            _context.ProductSKUS.Add(productSKU);
            _context.SaveChanges();
        }

        public BindableCollection<ProductSKU> Get(Product product)
        {
            if (product != null)
            {
                List<ProductSKU> productSKUs = _context.ProductSKUS.Where(p => p.Product.ProductID == product.ProductID).ToList();
                return new BindableCollection<ProductSKU>(productSKUs);
            }
            else return null;
        }

        public BindableCollection<ProductSKU> GetAll()
        {
            List<ProductSKU> productSKUs = _context.ProductSKUS.ToList();
            return new BindableCollection<ProductSKU>(productSKUs);
        }

        public void Remove(ProductSKU sku)
        {
            _context.ProductSKUS.Remove(sku);
            _context.SaveChanges();
        }
    }
}
