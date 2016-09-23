using Caliburn.Micro;
using StorageBox.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBox.Contracts
{
    public interface IOptionService
    {
        void Create(string optionName, int ProductID, Product product);
        BindableCollection<Option> Get(Product product);
        BindableCollection<Option> GetAll();
    }
}
