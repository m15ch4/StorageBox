using StorageBox.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBox.Contracts
{
    public interface IBoxService
    {
        ICollection<Box> Get(ProductSKU productSKU);
        IEnumerable<Box> Get(ProductSKU productSKU, int numberOfRecords);
        bool Reserve(Box box);
        bool Fill(Box box);
        bool Empty(Box box);

        int Count(ProductSKU productSKU);
    }
}
