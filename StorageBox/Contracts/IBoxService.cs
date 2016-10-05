using Caliburn.Micro;
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
        BindableCollection<Box> GetAll();
        Box Get(byte row, byte column);
        ICollection<Box> Get(ProductSKU productSKU);
        IEnumerable<Box> Get(ProductSKU productSKU, int numberOfRecords);
        bool Reserve(Box box);
        bool Fill(Box box, ProductSKU productSKU);
        bool Empty(Box box);

        int Count(ProductSKU productSKU);

        BindableCollection<BoxSize> GetAllBoxSizes();
        void CreateBoxSize(string boxSizeName);

        void CreateBox(byte row, byte column, BoxSize boxSize);
        bool FillSingle(ProductSKU productSKU, byte row, byte column);
        void Remove(Box box);
    }
}
