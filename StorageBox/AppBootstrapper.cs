namespace StorageBox {
    using System;
    using System.Collections.Generic;
    using Caliburn.Micro;
    using System.ComponentModel.Composition;
    using System.ComponentModel;
    using System.ComponentModel.Composition.Hosting;
    using Framework;
    using Orders.ViewModels;
    using Contracts;
    using Implementations;
    using Additions.ViewModels;
    using Orders.Dialogs;
    using Models;
    using Login;

    public class AppBootstrapper : BootstrapperBase {
        SimpleContainer container;

        public AppBootstrapper() {
            Initialize();
        }

        protected override void Configure() {
            container = new SimpleContainer();

            container.Singleton<IWindowManager, WindowManager>();
            container.Singleton<IEventAggregator, EventAggregator>();

            container.Singleton<MyDBContext, MyDBContext>();

            container.PerRequest<IShell, Shell.ViewModels.ShellViewModel>();

            container.PerRequest<IWorkspace, LoginViewModel>();
            container.PerRequest<IWorkspace, OrdersViewModel>();
            container.PerRequest<IWorkspace, AdditionsViewModel>();

            container.PerRequest<IAddition, AddCategoryViewModel>();
            container.PerRequest<IDialog, ProcessOrderViewModel>();

            container.PerRequest<ICategoryService, CategoryService>();
            container.PerRequest<IProductService, ProductService>();
            container.PerRequest<IProductSKUService, ProductSKUService>();
            container.PerRequest<IProductVariantService, ProductVariantService>();
            container.PerRequest<IBoxService, BoxService>();
            container.PerRequest<ISBTaskService, SBTaskService>();
            container.PerRequest<ILoginService, LoginService>();
        }

        protected override object GetInstance(Type service, string key) {
            var instance = container.GetInstance(service, key);
            if (instance != null)
                return instance;

            throw new InvalidOperationException("Could not locate any instances.");
        }

        protected override IEnumerable<object> GetAllInstances(Type service) {
            return container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance) {
            container.BuildUp(instance);
        }

        protected override void OnStartup(object sender, System.Windows.StartupEventArgs e) {
            DisplayRootViewFor<IShell>();
        }
    }
}