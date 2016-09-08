using Caliburn.Micro;
using StorageBox.Models;

namespace StorageBox.Contracts
{
    public interface IProductService
    {
        BindableCollection<Product> Get(Category category);

    }
}
