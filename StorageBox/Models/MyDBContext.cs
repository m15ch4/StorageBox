﻿using StorageBox.Framework;
using System.Data.Entity;

namespace StorageBox.Models
{
    public class MyDBContext : DbContext, IMyDBContext
    {
        public MyDBContext() : base("StorageBox.Properties.Settings.StorageBoxDBConnectionString")
        {
            
        }
        public MyDBContext(string connectionString = "StorageBox.Properties.Settings.StorageBoxDBConnectionString") : base("StorageBox.Properties.Settings.StorageBoxDBConnectionString")
        {

        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<OptionValue> OptionValues { get; set; }
        public DbSet<ProductSKU> ProductSKUS { get; set; }
        public DbSet<SKUValue> SKUValues { get; set; }
        public DbSet<Box> Boxes { get; set; }
        public DbSet<BoxSize> BoxSizes { get; set; }
        public DbSet<SBTask> SBTasks { get; set; }
        public DbSet<SBUser> SBUsers { get; set; }
        public DbSet<SBRole> SBRoles { get; set; }
    }
}
