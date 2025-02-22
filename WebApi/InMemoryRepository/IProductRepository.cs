using System.Collections.Generic;
using WebApi.Models.Products;

namespace WebApi.InMemoryRepository
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetAll();
        IEnumerable<Product> GetFiltered(string name, int page, int pageSize);
        Product GetById(int id);
        void Add(Product product);
        void Update(Product product);
        void Delete(int id);
    }
}
