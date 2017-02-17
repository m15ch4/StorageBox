namespace StorageBox.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;


    public enum Status
    {
        Empty,
        Full,
        Reserved
    }

    public class Box
    {
        public Box()
        {
        }

        public int BoxID { get; set; }
        public byte AddressRow { get; set; }
        public byte AddressCol { get; set; }


        //[ForeignKey("ProductSKU")]
        //[Column(Order = 1)]
        //public int? ProductID { get; set; }

        //[ForeignKey("ProductSKU")]
        //[Column(Order = 2)]
        //public int? ProductSKUID { get; set; }

        public virtual ProductSKU ProductSKU { get; set; }
        public virtual BoxSize BoxSize { get; set; }
        public Status Status { get; set; }
        public virtual ICollection<SBTask> SBTasks { get; set; }

        public string DisplayName
        {
            get
            {
                string displayName = "Wiersz: " + AddressRow + ", Kolumna: " + AddressCol + ", Rozmiar: " + BoxSize.BoxSizeName;
                
                return displayName;
            }
        }

        public string DisplayContent
        {
            get
            {
                string displayContent = "";
                if (Status == Status.Full)
                {
                    displayContent = ProductSKU.Product.ProductName + " [" + ProductSKU.Sku + "]" + " -> " + ProductSKU.SKUOptionsDescription;
                }
                else
                {
                    displayContent = "<Pusta>";
                }

                return displayContent;
            }
        }

        public System.Windows.Media.Brush ForegroundColor
        {
            get
            {
                if (Status == Status.Full)
                    return System.Windows.Media.Brushes.Black;
                else
                    return System.Windows.Media.Brushes.Red;
            }
        }

        public System.Windows.Media.Brush BackgroundColor
        {
            get
            {
                if (Status == Status.Full)
                    return System.Windows.Media.Brushes.LightGreen;
                else
                    return System.Windows.Media.Brushes.Yellow;
            }
        }
    }
}
