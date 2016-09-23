namespace StorageBox.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ProductSKU
    {
        public ProductSKU()
        {

        }

        [Key]
        [Column(Order = 2)]
        [Index(IsUnique = true)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductSKUID { get; set; }

        [Key, ForeignKey("Product")]
        [Column(Order = 1)]
        public int ProductID { get; set; }

        public string Sku { get; set; }

        public string Price { get; set; }

        public virtual Product Product { get; set; }
        public virtual ICollection<SKUValue> SKUValues { get; set; }
        public virtual ICollection<Box> Boxes { get; set; }
        public virtual ICollection<SBTask> SBTasks { get; set; }

    }
}
