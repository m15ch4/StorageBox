using Caliburn.Micro;
using StorageBox.Models;

namespace StorageBox.Contracts
{
    public interface IProductSKUService
    {
        BindableCollection<ProductSKU> Get(Product product);
    }
}
