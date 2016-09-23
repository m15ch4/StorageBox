using Caliburn.Micro;
using StorageBox.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBox.Contracts
{
    public interface ISKUValueService
    {
        void Create(int productID, int productSKUID, int optionID, int optionValueID);
        BindableCollection<SKUValue> GetAll();
        BindableCollection<SKUValue> Get(Product product);
    }
}
