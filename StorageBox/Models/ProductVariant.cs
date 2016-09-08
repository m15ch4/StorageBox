using System.Collections.Generic;
using System.Diagnostics;

namespace StorageBox.Models
{
    public class ProductVariant
    {
        private Product _product;
        private ProductSKU _productSKU;
        public Dictionary<Option, OptionValue> _optionValues;

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

        public string Description
        {
            get
            {
                string _description = "";
                _description += ProductSKU.Sku;
                foreach (KeyValuePair<Option, OptionValue> ov in _optionValues)
                {
                    _description += " " + ov.Key.OptionName + ": " + ov.Value.ValueName;
                    Trace.WriteLine("1");
                }
                //var enumerator = _optionValues.GetEnumerator();
                //bool hasNext = enumerator.MoveNext();
                //while (hasNext)
                //{
                //    _description += enumerator.Current.Key.OptionName + ": " + enumerator.Current.Value.ValueName;
                //    hasNext = enumerator.MoveNext();
                //    if (hasNext)
                //    {
                //        _description += ", ";
                //    }
                //    Trace.WriteLine("1");
                //}
                Trace.WriteLine(_description);
                return _description;
            }
        }

    }
}
