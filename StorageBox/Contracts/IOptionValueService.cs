using Caliburn.Micro;
using StorageBox.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBox.Contracts
{
    public interface IOptionValueService
    {
        void Create(Option option, int productID, string valueName);
        BindableCollection<OptionValue> GetAll();
        BindableCollection<OptionValue> Get(Option option);
        void Remove(OptionValue optionValue);
    }
}
