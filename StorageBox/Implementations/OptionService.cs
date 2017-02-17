using StorageBox.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StorageBox.Models;
using Caliburn.Micro;

namespace StorageBox.Implementations
{
    class OptionService : IOptionService
    {

        private MyDBContext _context;
        public OptionService(MyDBContext context)
        {
            _context = context;
        }

        public void Create(string optionName, int ProductID, Product product)
        {
            try
            {
                Option option = new Option() { OptionName = optionName, ProductID = product.ProductID, Product = product };
                _context.Options.Add(option);
                _context.SaveChanges();
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException e)
            {
                throw e;
            }
        }

        public BindableCollection<Option> Get(Product product)
        {
            if (product != null)
            {
                List<Option> options = _context.Options.Where(o => o.ProductID == product.ProductID).ToList();
                return new BindableCollection<Option>(options);
            }
            else return null;
        }

        public BindableCollection<Option> GetAll()
        {
            List<Option> options = _context.Options.ToList();
            return new BindableCollection<Option>(options);
        }

        public void Remove(Option option)
        {
            if (option != null)
            {
                _context.Options.Remove(option);
                _context.SaveChanges();
            }
        }
    }
}
