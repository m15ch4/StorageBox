using StorageBox.Contracts;
using System;
using Caliburn.Micro;
using StorageBox.Models;

namespace StorageBox.Implementations
{
    public class CategoryService : ICategoryService
    {
        private MyDBContext _context;
        public CategoryService(MyDBContext context)
        {
            _context = context;
        }
        public void Add(string categoryName)
        {
            try
            {
                Category category = new Category() { CategoryName = categoryName };
                _context.Categories.Add(category);
                _context.SaveChanges();
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException e)
            {
                throw e;
            }
        }

        public void Delete(Category category)
        {
            try
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException e)
            {
                throw e;
            }
        }

        public BindableCollection<Category> GetAll()
        {
            return new BindableCollection<Category>(_context.Categories);
        }
    }
}
