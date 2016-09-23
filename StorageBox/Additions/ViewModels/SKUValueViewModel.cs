﻿using Caliburn.Micro;
using StorageBox.Contracts;
using StorageBox.Framework;
using StorageBox.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBox.Additions.ViewModels
{
    public class SKUValueViewModel : Screen, IAddition
    {
        private ICategoryService _categoryService;
        private IOptionService _optionService;
        private IOptionValueService _optionValueService;
        private IProductService _productService;
        private IProductSKUService _productSKUService;
        private BindableCollection<Category> _categories;
        private Category _categoriesSelectedItem;
        private BindableCollection<Product> _products;
        private Product _productsSelectedItem;
        private BindableCollection<Option> _options;
        private Option _optionsSelectedItem;
        private BindableCollection<OptionValue> _optionValues;
        private OptionValue _optionValuesSelectedItem;
        private BindableCollection<ProductSKU> _productSKUs;
        private ProductSKU _productSKUsSelectedItem;
        private ISKUValueService _skuValueService;
        private BindableCollection<SKUValue> _skuValues;

        public SKUValueViewModel(ICategoryService categoryService, IProductService productService, IProductSKUService productSKUService, IOptionService optionService, IOptionValueService optionValueService, ISKUValueService skuValueService)
        {
            _categoryService = categoryService;
            _productService = productService;
            _productSKUService = productSKUService;
            _optionService = optionService;
            _optionValueService = optionValueService;
            _skuValueService = skuValueService;
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            Categories = _categoryService.GetAll();
            SKUValues = _skuValueService.GetAll();
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
                if (_productsSelectedItem != null)
                {
                    Options = _optionService.Get(_productsSelectedItem);
                    ProductSKUs = _productSKUService.Get(_productsSelectedItem);
                    SKUValues = _skuValueService.Get(_productsSelectedItem);
                }
                else
                {
                    Options = null;
                    ProductSKUs = null;
                    SKUValues = null;
                }
                NotifyOfPropertyChange(() => ProductsSelectedItem);
            }
        }

        public BindableCollection<Option> Options
        {
            get { return _options; }
            set
            {
                _options = value;
                NotifyOfPropertyChange(() => Options);
            }
        }

        public Option OptionsSelectedItem
        {
            get { return _optionsSelectedItem; }
            set
            {
                _optionsSelectedItem = value;
                if (_optionsSelectedItem != null)
                {
                    OptionValues = _optionValueService.Get(_optionsSelectedItem);
                }
                else
                {
                    OptionValues = null;
                }
                NotifyOfPropertyChange(() => OptionsSelectedItem);
            }
        }

        public BindableCollection<OptionValue> OptionValues
        {
            get { return _optionValues; }
            set
            {
                _optionValues = value;
                NotifyOfPropertyChange(() => OptionValues);
            }
        }

        public OptionValue OptionValuesSelectedItem
        {
            get { return _optionValuesSelectedItem; }
            set
            {
                _optionValuesSelectedItem = value;
                NotifyOfPropertyChange(() => OptionValuesSelectedItem);
            }
        }

        public BindableCollection<ProductSKU> ProductSKUs
        {
            get { return _productSKUs; }
            set
            {
                _productSKUs = value;
                NotifyOfPropertyChange(() => ProductSKUs);
            }
        }

        public ProductSKU ProductSKUsSelectedItem
        {
            get { return _productSKUsSelectedItem; }
            set
            {
                _productSKUsSelectedItem = value;
                NotifyOfPropertyChange(() => ProductSKUsSelectedItem);
            }
        }

        public void CreateSKUValue()
        {
            _skuValueService.Create(ProductsSelectedItem.ProductID, ProductSKUsSelectedItem.ProductSKUID, OptionsSelectedItem.OptionID, OptionValuesSelectedItem.OptionValueID);
            SKUValues = _skuValueService.Get(ProductsSelectedItem);
        }

        public BindableCollection<SKUValue> SKUValues
        {
            get { return _skuValues; }
            set
            {
                _skuValues = value;
                NotifyOfPropertyChange(() => SKUValues);
            }
        }
    }
}