using System;
using Caliburn.Micro;
using StorageBox.Contracts;
using StorageBox.Models;
using System.Linq;
using System.Collections.Generic;
using System.IO;

namespace StorageBox.Implementations
{
    public class ProductService : IProductService
    {
        private MyDBContext _context;

        public ProductService(MyDBContext context) 
        {
            _context = context;
        }

        public byte[] ReadFile(string filePath)
        {
            byte[] buffer;
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            try
            {
                int length = (int)fileStream.Length;  // get file length
                buffer = new byte[length];            // create buffer
                int count;                            // actual number of bytes read
                int sum = 0;                          // total number of bytes read

                // read until Read method returns 0 (end of the stream has been reached)
                while ((count = fileStream.Read(buffer, sum, length - sum)) > 0)
                    sum += count;  // sum is a buffer offset for next reading
            }
            finally
            {
                fileStream.Close();
            }
            return buffer;
        }

        public void Create(string productName, string productDescription, Category category, string imagePath)
        {
            byte[] imageData = ReadFile(imagePath);

            Product product = new Product() { ProductName = productName, ProductDescription = productDescription, Category = category, ProductImageContent=imageData };
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public BindableCollection<Product> Get(Category category)
        {
            List<Product> productList = _context.Products.Where(p => p.Category.CategoryID == category.CategoryID).ToList();
            return new BindableCollection<Product>(productList);
        }

        public BindableCollection<Product> GetAll()
        {
            List<Product> productList = _context.Products.ToList();
            return new BindableCollection<Product>(productList);
        }
    }
}
