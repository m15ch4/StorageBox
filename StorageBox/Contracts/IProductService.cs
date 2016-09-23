using Caliburn.Micro;
using StorageBox.Models;

namespace StorageBox.Contracts
{
    public interface IProductService
    {
        BindableCollection<Product> Get(Category category);
        BindableCollection<Product> GetAll();
        void Create(string productName, string productDescription, Category category, string imagePath);

    }
}
