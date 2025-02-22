using System.Collections.Generic;
using WebApi.Models.Orders;

namespace WebApi.InMemoryRepository
{
    public interface IOrderRepository
    {
        IEnumerable<Order> GetFiltered(string productName, int page, int pageSize);

        Order GetById(int id);

        void Add(Order order);
        void Update(Order order);
        void Delete(int id);
    }
}
