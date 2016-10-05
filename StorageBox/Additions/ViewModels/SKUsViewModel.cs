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
        private ICategoryService _categoryService;
        private BindableCollection<Category> _categories;
        private Category _categoriesSelectedItem;
        private int _threshold;

        public SKUsViewModel(ICategoryService categoryService, IProductSKUService productSKUService, IProductService productService)
        {
            _categoryService = categoryService;
            _productSKUService = productSKUService;
            _productService = productService;

            //Products = _productService.GetAll();
        }

        
        protected override void OnActivate()
        {
            base.OnActivate();
            Categories = _categoryService.GetAll();
            SKU = "";
            //Products = _productService.GetAll();
            //SKUs = _productSKUService.GetAll();
        }

        public BindableCollection<Category> Categories
        {
            get { return _categories; }
            set
            {
                _categories = value;
                NotifyOfPropertyChange(() => Categories);
            }
        }

        public Category CategoriesSelectedItem
        {
            get { return _categoriesSelectedItem; }
            set
            {
                _categoriesSelectedItem = value;
                Products = _productService.Get(_categoriesSelectedItem);
                NotifyOfPropertyChange(() => CategoriesSelectedItem);
                NotifyOfPropertyChange(() => CanCreateSKU);
            }
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
                SKUs = _productSKUService.Get(_productsSelectedItem);
                NotifyOfPropertyChange(() => ProductsSelectedItem);
                NotifyOfPropertyChange(() => CanCreateSKU);
            }
        }

        public string SKU
        {
            get { return _sku; }
            set
            {
                _sku = value;
                NotifyOfPropertyChange(() => SKU);
                NotifyOfPropertyChange(() => CanCreateSKU);
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

        public int Threshold
        {
            get { return _threshold; }
            set
            {
                _threshold = value;
                NotifyOfPropertyChange(() => Threshold);
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
                _productSKUService.Create(SKU, ProductsSelectedItem, Price, Threshold);
                SKU = "";
                Price = "";
                SKUs = _productSKUService.Get(ProductsSelectedItem);
            }
        }


        public bool CanCreateSKU
        {
            get { return ((CategoriesSelectedItem != null) && (ProductsSelectedItem != null) && (SKU != "")); }
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
