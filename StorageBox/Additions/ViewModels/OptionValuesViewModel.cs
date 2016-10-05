using Caliburn.Micro;
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
    public class OptionValuesViewModel : Screen, IAddition
    {
        private IOptionService _optionService;
        private IOptionValueService _optionValueService;
        private BindableCollection<Product> _products;
        private IProductService _productService;
        private Product _productsSelectedItem;
        private BindableCollection<Option> _options;
        private Option _optionsSelectedItem;
        private string _optionValueName;
        private BindableCollection<OptionValue> _optionValues;
        private ICategoryService _categoryService;
        private BindableCollection<Category> _categories;
        private Category _categoriesSelectedItem;

        public OptionValuesViewModel(ICategoryService categoryService, IOptionValueService optionValueService, IOptionService optionService, IProductService productService)
        {
            _categoryService = categoryService;
            _optionValueService = optionValueService;
            _optionService = optionService;
            _productService = productService;
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            Categories = _categoryService.GetAll();
            OptionValueName = "";
            //Products = _productService.GetAll();
            //Options = _optionService.GetAll();
            //OptionValues = _optionValueService.GetAll();
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
                NotifyOfPropertyChange(() => CanCreateOptionValue);
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
                Options = _optionService.Get(_productsSelectedItem);
                NotifyOfPropertyChange(() => ProductsSelectedItem);
                NotifyOfPropertyChange(() => Options);
                NotifyOfPropertyChange(() => CanCreateOptionValue);
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
                NotifyOfPropertyChange(() => OptionsSelectedItem);
                OptionValues = _optionValueService.Get(OptionsSelectedItem);
                NotifyOfPropertyChange(() => CanCreateOptionValue);
            }
        }

        public string OptionValueName
        {
            get { return _optionValueName; }
            set
            {
                _optionValueName = value;
                NotifyOfPropertyChange(() => OptionValueName);
                NotifyOfPropertyChange(() => CanCreateOptionValue);
            }
        }

        public void CreateOptionValue()
        {
            _optionValueService.Create(OptionsSelectedItem, ProductsSelectedItem.ProductID, OptionValueName);
            OptionValueName = "";
            OptionValues = _optionValueService.Get(OptionsSelectedItem);
        }

        public bool CanCreateOptionValue
        {
            get { return ((CategoriesSelectedItem != null) && (ProductsSelectedItem != null) && (OptionsSelectedItem != null) && (OptionValueName != "")); }
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
    }
}
