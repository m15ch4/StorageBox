using System.Collections.Generic;
using System.Diagnostics;

namespace StorageBox.Models
{
    public class ProductVariant
    {
        private Product _product;
        private ProductSKU _productSKU;
        public Dictionary<Option, OptionValue> _optionValues;
        public static int reserved = 0;

        public ProductVariant()
        {
            _optionValues = new Dictionary<Option, OptionValue>();
        }

        public Product Product
        {
            get { return _product; }
            set { _product = value; }
        }

        public ProductSKU ProductSKU
        {
            get { return _productSKU; }
            set { _productSKU = value; }
        }



        public string ProductVariantDescriptionNoNumber
        {
            get
            {
                string _description = "";
                _description += ProductSKU.Sku;
                foreach (KeyValuePair<Option, OptionValue> ov in _optionValues)
                {
                    _description += " " + ov.Key.OptionName + ": " + ov.Value.ValueName;
                }


                return _description;
            }
        }

        public string ProductVariantDescriptionWithNumber
        {
            get
            {
                return (ProductVariantDescriptionNoNumber + " Ilość: " + Available);
            }
        }

        // TODO: zliczanie ile jeszcze jest dostępnych przedmiotów
        public int Available
        {
            get { return ProductSKU.Boxes.Count; }
        }

    }
}
