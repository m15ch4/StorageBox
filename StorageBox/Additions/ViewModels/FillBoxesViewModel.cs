using Caliburn.Micro;
using StorageBox.Contracts;
using StorageBox.Framework;
using StorageBox.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBox.Additions.ViewModels
{
    public class FillBoxesViewModel : Screen, IAddition
    {
        private IBoxService _boxService;
        private BindableCollection<Category> _categories;
        private Category _categoriesSelectedItem;
        private ICategoryService _categoryService;
        private BindableCollection<Product> _products;
        private Product _productsSelectedItem;
        private IProductService _productService;
        private BindableCollection<ProductSKU> _skus;
        private ProductSKU _skusSelectedItem;
        private IProductSKUService _productSKUService;
        private string _row;
        private string _column;
        private BindableCollection<Box> _boxes;
        private BindableCollection<Box> _emptyBoxes;
        private Box _boxes2SelectedItem;

        public FillBoxesViewModel(IBoxService boxService, ICategoryService categoryService, IProductService productService, IProductSKUService productSKUService)
        {
            _boxService = boxService;
            _categoryService = categoryService;
            _productService = productService;
            _productSKUService = productSKUService;
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            Categories = _categoryService.GetAll();
            Boxes = _boxService.GetAll();
            Boxes2 = _boxService.GetEmpty();
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
                NotifyOfPropertyChange(() => CanFillSingle);
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
                NotifyOfPropertyChange(() => CanFillSingle);
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
                NotifyOfPropertyChange(() => CanFillSingle);
            }
        }

        public string Row
        {
            get { return _row; }
            set
            {
                _row = value;
                NotifyOfPropertyChange(() => Row);
            }
        }

        public string Column
        {
            get { return _column; }
            set
            {
                _column = value;
                NotifyOfPropertyChange(() => Column);
            }
        }

        public void FillSingle(ProductSKU productSKU, byte row, byte column)
        {
            if (_boxService.Get(row, column).Status == Status.Empty)
            {
                if (_boxService.FillSingle(productSKU, row, column))
                {
                    Boxes = _boxService.GetAll();

                    Boxes2SelectedItem = null;
                    Boxes2 = _boxService.GetEmpty();
                    if (Boxes2.Count != 0)
                        Boxes2SelectedItem = Boxes2.First();
                }
            }
        }

        public bool CanFillSingle
        {
            get { return ((CategoriesSelectedItem != null) && (ProductsSelectedItem != null) && (SKUsSelectedItem != null) && (Boxes2SelectedItem != null)); }
        }

        public void EmptySingle(Box box)
        {
            _boxService.Empty(box);
            Boxes = _boxService.GetAll();
            Boxes2 = _boxService.GetEmpty();
        }

        public BindableCollection<Box> Boxes
        {
            get { return _boxes; }
            set
            {
                _boxes = value;
                NotifyOfPropertyChange(() => Boxes);
                NotifyOfPropertyChange(() => Boxes2);
            }
        }

        public BindableCollection<Box> Boxes2
        {
            get { return _emptyBoxes; }
            set
            {
                _emptyBoxes = value;
                NotifyOfPropertyChange(() => Boxes2);
                NotifyOfPropertyChange(() => Boxes);
            }
        }

        public Box Boxes2SelectedItem
        {
            get { return _boxes2SelectedItem; }
            set
            {
                _boxes2SelectedItem = value;
                if (_boxes2SelectedItem != null)
                {
                    Row = _boxes2SelectedItem.AddressRow.ToString();
                    Column = _boxes2SelectedItem.AddressCol.ToString();
                }
                else
                {
                    Row = "";
                    Column = "";
                }
                NotifyOfPropertyChange(() => Boxes2SelectedItem);
                NotifyOfPropertyChange(() => CanFillSingle);
            }
        }
    }
}
