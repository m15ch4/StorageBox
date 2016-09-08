namespace StorageBox.Models
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Box
    {
        public Box()
        {
        }

        public int BoxID { get; set; }
        public Int16 AddressRow { get; set; }
        public Int16 AddressCol { get; set; }


        [ForeignKey("ProductSKU")]
        [Column(Order = 1)]
        public int? ProductID { get; set; }

        [ForeignKey("ProductSKU")]
        [Column(Order = 2)]
        public int? ProductSKUID { get; set; }

        public virtual ProductSKU ProductSKU { get; set; }
        public virtual BoxSize BoxSize { get; set; }

    }
}
