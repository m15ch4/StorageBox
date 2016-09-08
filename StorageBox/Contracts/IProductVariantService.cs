using Caliburn.Micro;
using StorageBox.Models;

namespace StorageBox.Contracts
{
    public interface IProductVariantService
    {
        BindableCollection<ProductVariant> Get(Product product);
    }
}
