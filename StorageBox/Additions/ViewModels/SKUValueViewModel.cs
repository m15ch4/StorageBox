using Caliburn.Micro;
using StorageBox.Contracts;
using StorageBox.Framework;
using StorageBox.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
        private SKUValue _skuValuesSelectedItem;

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
                NotifyOfPropertyChange(() => CanCreateSKUValue);
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
                NotifyOfPropertyChange(() => CanCreateSKUValue);
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
                NotifyOfPropertyChange(() => CanCreateSKUValue);
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
                NotifyOfPropertyChange(() => CanCreateSKUValue);
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
                NotifyOfPropertyChange(() => CanCreateSKUValue);
            }
        }

        public void CreateSKUValue()
        {
            try
            {
                _skuValueService.Create(ProductsSelectedItem.ProductID, ProductSKUsSelectedItem.ProductSKUID, OptionsSelectedItem.OptionID, OptionValuesSelectedItem.OptionValueID);
                SKUValues = _skuValueService.Get(ProductsSelectedItem);
                MessageBox.Show("Utworzono nowe połączenie SKU z wartością opcji.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException e)
            {
                MessageBox.Show("Nie dodano nowego połączenia SKU z wartością opcji. Spróbuj ponownie.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public bool CanCreateSKUValue
        {
            get { return ((CategoriesSelectedItem != null) && (ProductsSelectedItem != null) && (OptionsSelectedItem != null) && (OptionValuesSelectedItem != null) && (ProductSKUsSelectedItem != null)); }
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


        public SKUValue SKUValuesSelectedItem
        {
            get
            {
                return _skuValuesSelectedItem;
            }
            set
            {
                _skuValuesSelectedItem = value;
                NotifyOfPropertyChange(() => SKUValuesSelectedItem);
                NotifyOfPropertyChange(() => CanRemoveSKUValue);
            }
        }
        public bool CanRemoveSKUValue
        {
            get { return (SKUValuesSelectedItem != null); }
        }
        public void RemoveSKUValue(SKUValue skuValue)
        {
            if (skuValue != null)
            {
                try
                {
                    _skuValueService.Remove(skuValue);
                    SKUValues = _skuValueService.Get(ProductsSelectedItem);
                    MessageBox.Show("Usunięto wybrane powiązanie SKU - wartość opcji. ", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (System.Data.Entity.Infrastructure.DbUpdateException e)
                {
                    MessageBox.Show("Nie usunięto wybranego powiązania SKU - wartość opcji. Prawdopodobną przyczyną są istniejące odwołania do tego elementu.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
