namespace StorageBox.Orders.ViewModels
{
    using Caliburn.Micro;
    using Contracts;
    using Dialogs;
    using Models;
    using StorageBox.Framework;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Dynamic;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Windows.Media.Imaging;

    public class OrdersViewModel : Conductor<object>.Collection.OneActive, IWorkspace
    {
        private ICategoryService _categoryService;
        private BindableCollection<Category> _categories;
        private Category _categoriesSelectedItem;
        private BindableCollection<Product> _products;
        private IProductService _productService;
        private Product _productsSelectedItem;
        private IProductSKUService _productSKUService;
        private IProductVariantService _productVariantService;
        private BindableCollection<ProductVariant> _productVariants;
        private ProductVariant _productsVariantSelectedItem = null;
        private BindableCollection<WishListItem> _orderQueue = new BindableCollection<WishListItem>();
        private WishListItem _orderQueueSelectedItem;
        private IWindowManager _windowManager;
        private IBoxService _boxService;
        private ISBTaskService _sbTaskService;

        public OrdersViewModel(IWindowManager windowManager, ICategoryService categoryService, IProductService productService, IProductSKUService productSKUService, IProductVariantService productVariantService, IBoxService boxService, ISBTaskService sbTaskService)
        {
            _windowManager = windowManager;
            _categoryService = categoryService;
            _productService = productService;
            _productSKUService = productSKUService;
            _productVariantService = productVariantService;
            _boxService = boxService;
            _sbTaskService = sbTaskService;

            _categories = _categoryService.GetAll();
            if (_categories.Count > 0)
            {
                CategoriesSelectedItem = Categories[0];
            }
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
            get { return _productVariants; }
            set
            {
                _productVariants = value;
                NotifyOfPropertyChange(() => ProductVariants);
                NotifyOfPropertyChange(() => CanAddToQueue);
            }
        }


        public ProductVariant ProductsVariantSelectedItem
        {
            get { return _productsVariantSelectedItem; }
            set
            {
                _productsVariantSelectedItem = value;
                NotifyOfPropertyChange(() => ProductsVariantSelectedItem);
                NotifyOfPropertyChange(() => CanAddToQueue);
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

        // ORDER: AddToWishList
        public void AddToWishList(ProductVariant productVariant)
        {
            if (ProductsVariantSelectedItem != null)
            {
                int inBoxCount = _boxService.Count(_productsVariantSelectedItem.ProductSKU);
                WishListItem wishListItem = null;
                wishListItem = OrderQueue.SingleOrDefault(i => i._productVariant.ProductSKU.ProductSKUID == _productsVariantSelectedItem.ProductSKU.ProductSKUID);
                if (wishListItem == null)
                {
                    wishListItem = new WishListItem();
                    wishListItem.ProductVariant = _productsVariantSelectedItem;
                    wishListItem.Count = 0;
                    //wishListItem.Count = 1;
                    OrderQueue.Add(wishListItem);
                }
                else
                {
                    //wishListItem.Count += 1;
                }

                if (inBoxCount > wishListItem.Count)
                {
                    wishListItem.Count += 1;
                }

                NotifyOfPropertyChange(() => wishListItem.WishListItemDescription);
                NotifyOfPropertyChange(() => OrderQueue);
                NotifyOfPropertyChange(() => CanOrder);
            }
        }

        // ORDER: RemoveFromWishList
        public void RemoveFromWishList(WishListItem wishListItem)
        {
            if (OrderQueueSelectedItem != null)
            {
                if (OrderQueueSelectedItem.Count > 1)
                {
                    OrderQueueSelectedItem.Count -= 1;
                    NotifyOfPropertyChange(() => OrderQueueSelectedItem.WishListItemDescription);
                }
                else
                {
                    OrderQueue.Remove(_orderQueueSelectedItem);
                }

                NotifyOfPropertyChange(() => OrderQueue);
                NotifyOfPropertyChange(() => CanOrder);
            }
        }

        public bool CanRemoveFromWishList
        {
            get { return _orderQueueSelectedItem != null; }
        }


        public bool CanAddToQueue
        {
            get { return _productsVariantSelectedItem != null; }
        }


        // TODO: tutaj trzeba zmienić żeby wartości budował na podstawie wishlist 
        public BindableCollection<WishListItem> OrderQueue
        {
            get { return _orderQueue; }
            set
            {
                _orderQueue = value;
                NotifyOfPropertyChange(() => OrderQueue);
                NotifyOfPropertyChange(() => CanOrder);
            }
        }

        public WishListItem OrderQueueSelectedItem
        {
            get { return _orderQueueSelectedItem; }
            set
            {
                _orderQueueSelectedItem = value;
                NotifyOfPropertyChange(() => OrderQueueSelectedItem);
                NotifyOfPropertyChange(() => CanRemoveFromWishList);
            }
        }

        private static BitmapImage LoadImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0) return null;
            var image = new BitmapImage();
            using (var mem = new MemoryStream(imageData))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }
        public BitmapImage ProductImage
        {
            get
            {
                try
                {
                    //return Path.Combine("E:/", "myimage.jpg");
                    return LoadImage(_productsSelectedItem.ProductImageContent);
                }
                catch
                {
                    return new BitmapImage(new System.Uri("E:/myimage.jpg"));
                }
            }

        }

        public void Order()
        {
            dynamic mysettings = new ExpandoObject();
            mysettings.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            //mysettings.ResizeMode = ResizeMode.NoResize;
           // mysettings.WindowStyle = WindowStyle.None;
           // mysettings.ShowInTaskbar = false;

            _windowManager.ShowDialog(new ProcessOrderViewModel(OrderQueue, _boxService, _sbTaskService),null,mysettings);
        }

        public bool CanOrder
        {
            get { return (OrderQueue.Count != 0); }
        }

    }
}
