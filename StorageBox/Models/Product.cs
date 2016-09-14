namespace StorageBox.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Product
    {
        public Product()
        {
        }

        public int ProductID { get; set; }

        [Index(IsUnique = true)]
        [MaxLength(200)]
        public string ProductName { get; set; }

        public string ProductDescription { get; set; }

        public string ProductImage { get; set; }

        public virtual Category Category { get; set; }
        public virtual ICollection<Option> Options { get; set; }
        public virtual ICollection<ProductSKU> ProductSKUS { get; set; }

    }
}
