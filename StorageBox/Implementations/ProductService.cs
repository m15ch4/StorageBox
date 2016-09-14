using System;
using Caliburn.Micro;
using StorageBox.Contracts;
using StorageBox.Models;
using System.Linq;
using System.Collections.Generic;

namespace StorageBox.Implementations
{
    public class ProductService : IProductService
    {
        private MyDBContext _context;

        public ProductService(MyDBContext context) 
        {
            _context = context;
        }
        public BindableCollection<Product> Get(Category category)
        {
            List<Product> productList = _context.Products.Where(p => p.Category.CategoryID == category.CategoryID).ToList();
            return new BindableCollection<Product>(productList);
        }
    }
}
