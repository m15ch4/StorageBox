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
    public class OptionsViewModel : Screen, IAddition
    {
        private IOptionService _optionService;
        private BindableCollection<Product> _products;
        private IProductService _productService;
        private Product _productsSelectedItem;
        private string _optionName;
        private BindableCollection<Option> _options;
        private ICategoryService _categoryService;
        private BindableCollection<Category> _categories;
        private Category _categoriesSelectedItem;
        private Option _optionsSelectedItem;

        public OptionsViewModel(ICategoryService categoryService, IProductService productService, IOptionService optionService)
        {
            _categoryService = categoryService;
            _productService = productService;
            _optionService = optionService;

            //Products = _productService.GetAll();
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            Categories = _categoryService.GetAll();
            Products = null;
            OptionName = "";
            //Products = _productService.GetAll();
            //Options = _optionService.GetAll();
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
                NotifyOfPropertyChange(() => CanCreateOption);
            }
        }

        public Option OptionsSelectedItem
        {
            get { return _optionsSelectedItem; }
            set
            {
                _optionsSelectedItem = value;
                NotifyOfPropertyChange(() => OptionsSelectedItem);
                NotifyOfPropertyChange(() => CanRemoveOption);
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
                NotifyOfPropertyChange(() => CanCreateOption);
            }
        }

        public string OptionName
        {
            get { return _optionName; }
            set
            {
                _optionName = value;
                NotifyOfPropertyChange(() => OptionName);
                NotifyOfPropertyChange(() => CanCreateOption);
            }
        }

        public void CreateOption()
        {
            try
            {
                _optionService.Create(OptionName, ProductsSelectedItem.ProductID, ProductsSelectedItem);
                Options = _optionService.Get(ProductsSelectedItem);
                MessageBox.Show("Dodano nową opcję.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                OptionName = "";
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException e)
            {
                MessageBox.Show("Nie dodano opcji.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public bool CanCreateOption
        {
            get { return ((OptionName != "") && (CategoriesSelectedItem != null) && (ProductsSelectedItem != null)); }
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

        public bool CanRemoveOption
        {
            get
            {
                return (_optionsSelectedItem != null);
            }
        }

        public void RemoveOption(Option option)
        {
            try
            {
                _optionService.Remove(option);
                Options = _optionService.Get(ProductsSelectedItem);
                MessageBox.Show("Usunięto wybraną opcję.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException e)
            {
                MessageBox.Show("Nie usunięto opcji. Sprawdź czy nie istnieją obiekty zależne od wybranej opcji.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
