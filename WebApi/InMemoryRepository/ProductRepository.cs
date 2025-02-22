using Common;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using WebApi.Models.Products;

namespace WebApi.InMemoryRepository
{
    [AutoRegister]
    public class ProductRepository : IProductRepository
    {
        private static List<Product> _products = new List<Product>
        {
            new Product { Id = 1, Name = "RavenDB", Price = 3200.00m },
            new Product { Id = 2, Name = "Azure Sql Database", Price = 2800.00m },
            new Product { Id = 3, Name = "Virtual Machine", Price = 2550.00m }
        };

        public IEnumerable<Product> GetAll() => _products;

        public IEnumerable<Product> GetFiltered(string name, int page, int pageSize)
        {
            // filtered by product name and also doing pagination
            return _products.Where(p => p.Name == name || string.IsNullOrEmpty(name))
                             .Skip((page - 1) * pageSize)
                             .Take(pageSize)
                             .ToList();
        }

        public Product GetById(int id) => _products.FirstOrDefault(p => p.Id == id);

        public void Add(Product product)
        {
            product.Id = _products.Any() ? _products.Max(p => p.Id) + 1 : 1;
            _products.Add(product);
        }

        public void Update(Product product)
        {
            var existing = GetById(product.Id);
            if (existing != null)
            {
                existing.Name = product.Name;
                existing.Price = product.Price;
            }
        }

        public void Delete(int id)
        {
            var product = GetById(id);
            if (product != null)
            {
                _products.Remove(product);
            }
        }
    }
}
