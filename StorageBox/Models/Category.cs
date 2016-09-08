namespace StorageBox.Models
{
    using System.Collections.Generic;

    public class Category
    {
        public Category()
        {
        }

        public int CategoryID { get; set; }
        public string CategoryName { get; set; }

        public virtual ICollection<Product> Products { get; set; }

    }
}
