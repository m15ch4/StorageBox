using Caliburn.Micro;
using StorageBox.Models;

namespace StorageBox.Contracts
{
    public interface IProductSKUService
    {
        BindableCollection<ProductSKU> Get(Product product);
        BindableCollection<ProductSKU> GetAll();
        void Create(string SKU, Product product, string price);
        void Remove(ProductSKU sku);
    }
}
