using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBox.Models
{
    public class WishListItem : PropertyChangedBase
    {
        //public ProductVariant _productVariant;
        public ProductSKU _productSKU;
        public int _count;


        public ProductSKU ProductSKU
        {
            get { return _productSKU; }
            set
            {
                _productSKU = value;
                NotifyOfPropertyChange(() => ProductSKU);
            }
        }
        //public ProductVariant ProductVariant
        //{
        //    get { return _productVariant; }
        //    set
        //    {
        //        _productVariant = value;
        //        NotifyOfPropertyChange(() => ProductVariant);
        //        NotifyOfPropertyChange(() => WishListItemDescription);
        //    }
        //}

        public int Count
        {
            get { return _count; }
            set
            {
                _count = value;
                NotifyOfPropertyChange(() => Count);
                NotifyOfPropertyChange(() => WishListItemCountDescription);
            }
        }

        public string WishListItemNameDescription
        {
            get { return ProductSKU.Product.ProductName + " <" + ProductSKU.Sku + ">"; }
        }

        public string WishListItemOptionsDescription
        {
            get { return ProductSKU.SKUOptionsDescription; }
        }

        public string WishListItemCountDescription
        {
            get { return "Zamówionych: " + Count.ToString(); }
        }
    }
}
