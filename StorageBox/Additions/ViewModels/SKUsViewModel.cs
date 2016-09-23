using Caliburn.Micro;
using StorageBox.Contracts;
using StorageBox.Framework;
using StorageBox.Implementations;
using StorageBox.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBox.Additions.ViewModels
{
    public class SKUsViewModel : Screen, IAddition
    {
        private BindableCollection<Product> _products;
        private IProductService _productService;
        private IProductSKUService _productSKUService;
        private Product _productsSelectedItem;
        private string _sku;
        private string _price;
        private BindableCollection<ProductSKU> _skus;
        private ProductSKU _skusSelectedItem;

        public SKUsViewModel(IProductSKUService productSKUService, IProductService productService)
        {
            _productSKUService = productSKUService;
            _productService = productService;

            //Products = _productService.GetAll();
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            Products = _productService.GetAll();
            SKUs = _productSKUService.GetAll();
        }

        public BindableCollection<Product> Products
        {
            get { return _products; }
            set
            {
                _products = value;
                NotifyOfPropertyChange(() => Products);
            }
        }

        public Product ProductsSelectedItem
        {
            get { return _productsSelectedItem; }
            set
            {
                _productsSelectedItem = value;
                NotifyOfPropertyChange(() => ProductsSelectedItem);
            }
        }

        public string SKU
        {
            get { return _sku; }
            set
            {
                _sku = value;
                NotifyOfPropertyChange(() => SKU);
            }
        }

        public string Price
        {
            get { return _price; }
            set
            {
                _price = value;
                NotifyOfPropertyChange(() => Price);
            }
        }

        public void CreateSKU()
        {
            if (SKU != "")
            {
                if (Price == "")
                {
                    Price = "0";
                }
                _productSKUService.Create(SKU, ProductsSelectedItem, Price);
                SKU = "";
                Price = "";
                SKUs = _productSKUService.GetAll();
            }
        }

        public BindableCollection<ProductSKU> SKUs
        {
            get { return _skus; }
            set
            {
                _skus = value;
                NotifyOfPropertyChange(() => SKUs);
            }
        }

        public ProductSKU SKUsSelectedItem
        {
            get { return _skusSelectedItem; }
            set
            {
                _skusSelectedItem = value;
                NotifyOfPropertyChange(() => SKUsSelectedItem);
            }
        }

        public void RemoveSKU(ProductSKU sku)
        {
            if (sku != null)
            {
                _productSKUService.Remove(sku);
                SKUs = _productSKUService.GetAll();
            }
        }
    }
}
