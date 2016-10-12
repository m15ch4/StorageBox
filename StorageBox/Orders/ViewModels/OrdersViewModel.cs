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
        private ProductVariant _productsVariantSelectedItem = null;
        private BindableCollection<WishListItem> _orderQueue = new BindableCollection<WishListItem>();
        private WishListItem _orderQueueSelectedItem;
        private IWindowManager _windowManager;
        private IBoxService _boxService;
        private ISBTaskService _sbTaskService;
        private BindableCollection<ProductSKU> _productSKUs;
        private ProductSKU _productSKUsSelectedItem;
        private IShell _shell;
        private IEventAggregator _eventAggregator;

        public OrdersViewModel(IWindowManager windowManager, IEventAggregator eventAggregator, ICategoryService categoryService, IProductService productService, IProductSKUService productSKUService, IProductVariantService productVariantService, IBoxService boxService, ISBTaskService sbTaskService)
        {
            _windowManager = windowManager;
            _categoryService = categoryService;
            _productService = productService;
            _productSKUService = productSKUService;
            _productVariantService = productVariantService;
            _boxService = boxService;
            _sbTaskService = sbTaskService;
            _eventAggregator = eventAggregator;

            _categories = _categoryService.GetAll();
            if (_categories.Count > 0)
            {
                CategoriesSelectedItem = Categories[0];
            }
        }

        override protected void OnActivate()
        {
            _categories = _categoryService.GetAll();
            CategoriesSelectedItem = null;
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
                    Products = null;
                }
                ProductSKUs = null;

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
                    ProductSKUs = _productSKUService.Get(_productsSelectedItem);
                }
                catch
                {
                    Trace.WriteLine("Wyjątek w ProductsSelectedItem");
                    ProductSKUs = null;
                }
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
                NotifyOfPropertyChange(() => CanAddToWishList);
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
        public void AddToWishList(ProductSKU productSKU)
        {
            int inBoxCount = _boxService.Count(productSKU);
            WishListItem wishListItem = null;
            wishListItem = OrderQueue.SingleOrDefault(i => i.ProductSKU.ProductSKUID == productSKU.ProductSKUID);
            if (wishListItem == null)
            {
                wishListItem = new WishListItem();
                wishListItem.ProductSKU = productSKU;
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

            NotifyOfPropertyChange(() => wishListItem.WishListItemCountDescription);
            NotifyOfPropertyChange(() => OrderQueue);
            NotifyOfPropertyChange(() => CanOrder);
            NotifyOfPropertyChange(() => CanAddToWishList);
        }

        public bool CanAddToWishList
        {
            get {
                bool availability = false;
                bool stillAvailable = false;

                if (ProductSKUsSelectedItem != null)
                {
                    availability = _boxService.Count(ProductSKUsSelectedItem) != 0;

                    WishListItem wishListItem = OrderQueue.SingleOrDefault(i => i.ProductSKU.ProductSKUID == ProductSKUsSelectedItem.ProductSKUID);
                    if (wishListItem == null)
                    {
                        stillAvailable = true;
                    }
                    else
                    {
                        stillAvailable = (wishListItem.Count < _boxService.Count(ProductSKUsSelectedItem));
                    }
                }

                bool selected = _productSKUsSelectedItem != null;

                return (availability && selected && stillAvailable);
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
                    NotifyOfPropertyChange(() => OrderQueueSelectedItem.WishListItemCountDescription);
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
                    return LoadImage(_productsSelectedItem.ProductImageContent);
                }
                catch
                {
                    return new BitmapImage();
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

            ProcessOrderViewModel processOrderViewModel = new ProcessOrderViewModel(_windowManager, _eventAggregator, OrderQueue, _boxService, _sbTaskService);
            var dialog_result = _windowManager.ShowDialog(processOrderViewModel,null,mysettings);
            if (dialog_result)
            {
                CategoriesSelectedItem = null;
                OrderQueue.Clear();
                NotifyOfPropertyChange(() => CanOrder);
            }
        }

        public bool CanOrder
        {
            get { return (OrderQueue.Count != 0); }
        }

        public IShell Shell
        {
            get { return _shell; }
            set
            {
                _shell = value;
                NotifyOfPropertyChange(() => Shell);
            }
        }

    }
}
