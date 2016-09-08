namespace StorageBox.Orders.ViewModels
{
    using Caliburn.Micro;
    using Contracts;
    using Models;
    using StorageBox.Framework;
    using System.Diagnostics;
    using System.IO;
    using System.Windows.Media.Imaging;

    public class OrdersViewModel : Conductor<object>.Collection.OneActive, IWorkspace
    {
        private ICategoryService _categoryService;
        private BindableCollection<Category> _categories;
        private Category _categoriesSelectedItem;
        private BindableCollection<Product> _products;
        private IProductService _productService;
        private BindableCollection<ProductSKU> _skus;
        private Product _productsSelectedItem;
        private IProductSKUService _productSKUService;
        private IProductVariantService _productVariantService;
        private BindableCollection<ProductVariant> _productVarians;
        private ProductVariant _productsVariantSelectedItem;

        public OrdersViewModel(ICategoryService categoryService, IProductService productService, IProductSKUService productSKUService, IProductVariantService productVariantService)
        {
            _categoryService = categoryService;
            _productService = productService;
            _productSKUService = productSKUService;
            _productVariantService = productVariantService;

            _categories = _categoryService.GetAll();
            CategoriesSelectedItem = Categories[0];
        }

        override protected void OnActivate()
        {
            _categories = _categoryService.GetAll();
            CategoriesSelectedItem = Categories[0];
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
                NotifyOfPropertyChange(() => CategoriesSelectedItem);
                try {
                    Products = _productService.Get(_categoriesSelectedItem);
                }
                catch
                {
                    Trace.WriteLine("Wyjątek w CategoriesSelectedItem");
                }
                ProductVariants = null;

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
                NotifyOfPropertyChange(() => ProductsSelectedItem);
                NotifyOfPropertyChange(() => ItemDescription);
                NotifyOfPropertyChange(() => ProductImage);
                try
                {
                    ProductVariants = _productVariantService.Get(_productsSelectedItem);
                }
                catch
                {
                    Trace.WriteLine("Wyjątek w ProductsSelectedItem");
                    ProductVariants = null;
                }
            }
        }


        public BindableCollection<ProductVariant> ProductVariants
        {
            get { return _productVarians; }
            set
            {
                _productVarians = value;
                NotifyOfPropertyChange(() => ProductVariants);
            }
        }


        public ProductVariant ProductsVariantSelectedItem
        {
            get { return _productsVariantSelectedItem; }
            set
            {
                _productsVariantSelectedItem = value;
                NotifyOfPropertyChange(() => ProductsVariantSelectedItem);
            }
        }

        public string ItemDescription
        {
            get {
                if (ProductsSelectedItem != null)
                {
                    return ProductsSelectedItem.ProductDescription;
                }
                else
                {
                    return "";
                }
            }
        }

        public void AddToQueue(string productName)
        {
            
        }

        public string ProductImage
        {
            get
            {
                return Path.Combine("E:/", "myimage.jpg");
            }

        }

    }
}
