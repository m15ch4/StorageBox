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
        public ProductVariant _productVariant;
        public int _count;

        public ProductVariant ProductVariant
        {
            get { return _productVariant; }
            set
            {
                _productVariant = value;
                NotifyOfPropertyChange(() => ProductVariant);
                NotifyOfPropertyChange(() => WishListItemDescription);
            }
        }

        public int Count
        {
            get { return _count; }
            set
            {
                _count = value;
                NotifyOfPropertyChange(() => Count);
                NotifyOfPropertyChange(() => WishListItemDescription);
            }
        }

        public string WishListItemDescription
        {
            get { return _productVariant.Product.ProductName + " [" + _productVariant.ProductSKU.Sku + "] " + _count; }
        }
    }
}
