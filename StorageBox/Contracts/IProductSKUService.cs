using Caliburn.Micro;
using StorageBox.Models;
using System.Collections.Generic;

namespace StorageBox.Contracts
{
    public interface IProductSKUService
    {
        BindableCollection<ProductSKU> Get(Product product);
        BindableCollection<ProductSKU> GetAll();
        void Create(string SKU, Product product, string price, int threshold);
        void Remove(ProductSKU sku);
        
    }
}
