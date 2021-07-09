using System;
using System.Collections.Generic;
using System.Linq;

namespace Facade.Task1.OrderPlacement
{
    public class ProductCatalogService : ProductCatalog
    {
        public List<Product> _products;

        public ProductCatalogService()
        {
            _products = new List<Product>
            {
                new Product
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Cheese",
                    Category = "Food",
                    Price = 4
                },
                new Product
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Water",
                    Category = "Food",
                    Price = 3
                }
            };
                
        }

        public Product GetProductDetails(string productId)
        {
            return _products.FirstOrDefault(x => x.Id == productId);
        }
      
    }
}
