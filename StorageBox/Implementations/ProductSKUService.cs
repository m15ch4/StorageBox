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
    class ProductSKUService : BaseService, IProductSKUService
    {

        public ProductSKUService() : base()
        {

        }

        public BindableCollection<ProductSKU> Get(Product product)
        {
            List<ProductSKU> productSKUs = _context.ProductSKUS.Where(p => p.Product.ProductID == product.ProductID).ToList();
            return new BindableCollection<ProductSKU>(productSKUs);
        }
    }
}
