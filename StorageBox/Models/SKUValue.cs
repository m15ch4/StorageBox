namespace StorageBox.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class SKUValue
    {
        public SKUValue()
        {
        }

        [Key]
        [Column(Order = 1)]
        public int ProductID { get; set; }

        [Key]
        [Column(Order = 2)]
        public int ProductSKUID { get; set; }

        [Key]
        [Column(Order = 3)]
        public int OptionID { get; set; }

        public int OptionValueID { get; set; }

        public virtual ProductSKU ProductSKU { get; set; }
        public virtual Option Option { get; set; }
        public virtual OptionValue OptionValue { get; set; }

        public string Description
        {
            get {
                try
                {
                    return ProductSKU.Product.ProductName + ": " + ProductSKU.Sku + " " + Option.OptionName + " " + OptionValue.ValueName;
                }
                catch
                {
                    return "Błędne powiązanie";
                }
            }
            
        }
    }
}
