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

        public OptionValuesViewModel(IOptionValueService optionValueService, IOptionService optionService, IProductService productService)
        {
            _optionValueService = optionValueService;
            _optionService = optionService;
            _productService = productService;
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            Products = _productService.GetAll();
            Options = _optionService.GetAll();
            OptionValues = _optionValueService.GetAll();
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
            }
        }

        public string OptionValueName
        {
            get { return _optionValueName; }
            set
            {
                _optionValueName = value;
                NotifyOfPropertyChange(() => OptionValueName);
            }
        }

        public void CreateOptionValue()
        {
            _optionValueService.Create(OptionsSelectedItem, ProductsSelectedItem.ProductID, OptionValueName);
            OptionValueName = "";
            OptionValues = _optionValueService.GetAll();
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
