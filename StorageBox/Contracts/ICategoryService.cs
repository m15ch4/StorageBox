using Caliburn.Micro;
using StorageBox.Models;

namespace StorageBox.Contracts
{
    public interface ICategoryService
    {
        BindableCollection<Category> GetAll();
        void Add(string categoryName);

    }
}
