using System;
using Caliburn.Micro;
using StorageBox.Contracts;
using StorageBox.Models;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

namespace StorageBox.Implementations
{
    class ProductVariantService : IProductVariantService
    {
        private MyDBContext _context;

        public ProductVariantService(MyDBContext context) {
            _context = context;
        }

        public BindableCollection<ProductVariant> Get(Product product)
        {
            BindableCollection<ProductVariant> result = new BindableCollection<ProductVariant>();
            foreach (ProductSKU sku in product.ProductSKUS) {
                ProductVariant productVariant = new ProductVariant();

                productVariant.ProductSKU = sku;
                productVariant.Product = sku.Product;
                foreach (SKUValue skuval in sku.SKUValues)
                {
                    productVariant._optionValues.Add(skuval.Option, skuval.OptionValue);
                }

                result.Add(productVariant);
            }
            return result;
        }
    }
}
