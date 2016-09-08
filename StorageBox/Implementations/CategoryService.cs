using StorageBox.Contracts;
using System;
using Caliburn.Micro;
using StorageBox.Models;

namespace StorageBox.Implementations
{
    public class CategoryService : BaseService, ICategoryService
    {
        public CategoryService() : base()
        {

        }
        public void Add(string categoryName)
        {
            Category category = new Category() { CategoryName = categoryName };
            _context.Categories.Add(category);
            _context.SaveChanges();
        }

        public BindableCollection<Category> GetAll()
        {
            return new BindableCollection<Category>(_context.Categories);
        }
    }
}
