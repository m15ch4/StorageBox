using Caliburn.Micro;
using Microsoft.Win32;
using StorageBox.Contracts;
using StorageBox.Framework;
using StorageBox.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBox.Additions.ViewModels
{
    class ProductsViewModel : Screen, IAddition
    {
        private IProductService _productService;
        private ICategoryService _categoryService;
        private BindableCollection<Category> _categories;
        private Category _categoriesSelectedItem;
        private string _productName;
        private string _productDescription;
        private BindableCollection<Product> _products;
        private string _imageFileName;

        public ProductsViewModel(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;

            //Categories = _categoryService.GetAll();
            //Products = _productService.GetAll();
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            Categories = _categoryService.GetAll();
            Products = _productService.GetAll();
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
            }
        }

        public string ProductName
        {
            get { return _productName; }
            set
            {
                _productName = value;
                NotifyOfPropertyChange(() => ProductName);
                NotifyOfPropertyChange(() => CanCreateProduct);
            }
        }

        public string ProductDescription
        {
            get { return _productDescription; }
            set
            {
                _productDescription = value;
                NotifyOfPropertyChange(() => ProductDescription);
            }
        }


        public void CreateProduct()
        {
            if (ProductName != "") {

                _productService.Create(ProductName, ProductDescription, CategoriesSelectedItem, ImageFileName);
                ProductName = "";
                ProductDescription = "";
                ImageFileName = "";
                Products = _productService.Get(CategoriesSelectedItem);
            }
        }

        public bool CanCreateProduct
        {
            get { return ((ProductName != "") && (CategoriesSelectedItem != null)); }
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

        public string ImageFileName
        {
            get { return _imageFileName; }
            set
            {
                _imageFileName = value;
                NotifyOfPropertyChange(() => ImageFileName);
            }
        }

        public void ImageFile()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == true)
                ImageFileName = fileDialog.FileName;
        }


    }
}
