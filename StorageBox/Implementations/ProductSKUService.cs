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

        public BindableCollection<ProductSKU> Get(Product product)
        {
            List<ProductSKU> productSKUs = _context.ProductSKUS.Where(p => p.Product.ProductID == product.ProductID).ToList();
            return new BindableCollection<ProductSKU>(productSKUs);
        }
    }
}
