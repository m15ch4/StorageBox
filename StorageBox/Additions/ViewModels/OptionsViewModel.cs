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
    public class OptionsViewModel : Screen, IAddition
    {
        private IOptionService _optionService;
        private BindableCollection<Product> _products;
        private IProductService _productService;
        private Product _productsSelectedItem;
        private string _optionName;
        private BindableCollection<Option> _options;

        public OptionsViewModel(IProductService productService, IOptionService optionService)
        {
            _productService = productService;
            _optionService = optionService;

            //Products = _productService.GetAll();
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            Products = _productService.GetAll();
            Options = _optionService.GetAll();
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

        public string OptionName
        {
            get { return _optionName; }
            set
            {
                _optionName = value;
                NotifyOfPropertyChange(() => OptionName);
            }
        }

        public void CreateOption()
        {
            _optionService.Create(OptionName, ProductsSelectedItem.ProductID, ProductsSelectedItem);
            OptionName = "";
            Options = _optionService.GetAll();
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
    }
}
