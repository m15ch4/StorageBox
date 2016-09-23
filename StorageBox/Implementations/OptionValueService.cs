using StorageBox.Contracts;
using StorageBox.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace StorageBox.Implementations
{
    public class OptionValueService : IOptionValueService
    {
        private MyDBContext _context;
        public OptionValueService(MyDBContext context)
        {
            _context = context;
        }

        public void Create(Option option, int productID, string valueName)
        {
            OptionValue optionValue = new OptionValue() { Option = option, ProductID = productID, ValueName = valueName };
            _context.OptionValues.Add(optionValue);
            _context.SaveChanges();
        }

        public BindableCollection<OptionValue> Get(Option option)
        {
            List<OptionValue> optionValues = _context.OptionValues.Where(ov => ov.OptionID == option.OptionID).ToList();
            return new BindableCollection<OptionValue>(optionValues);
        }

        public BindableCollection<OptionValue> GetAll()
        {
            List<OptionValue> optionValues = _context.OptionValues.ToList();
            return new BindableCollection<OptionValue>(optionValues);
        }
    }
}
