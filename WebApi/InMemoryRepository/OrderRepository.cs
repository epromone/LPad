using Common;
using System.Collections.Generic;
using System.Linq;
using WebApi.Models.Orders;

namespace WebApi.InMemoryRepository
{
    [AutoRegister]
    public class OrderRepository : IOrderRepository
    {
        private static List<Order> _orders = new List<Order>();

        public IEnumerable<Order> GetFiltered(string productName, int page, int pageSize)
        {
            // filtered by product name and also doing pagination
            return _orders.Where(o => o.ProductName == productName || string.IsNullOrEmpty(productName))
                             .Skip((page - 1) * pageSize)
                             .Take(pageSize)
                             .ToList();
        }

        public Order GetById(int id) => _orders.FirstOrDefault(o => o.Id == id);

        public void Add(Order order)
        {
            order.Id = _orders.Any() ? _orders.Max(o => o.Id) + 1 : 1;
            _orders.Add(order);
        }

        public void Update(Order order)
        {
            var existing = GetById(order.Id);
            if (existing != null)
            {
                existing.ProductName = order.ProductName;
                existing.Quantity = order.Quantity;
                //existing.OrderDate = order.OrderDate;
                existing.TotalAmount = order.TotalAmount;
            }
        }

        public void Delete(int id)
        {
            var order = GetById(id);
            if (order != null)
            {
                _orders.Remove(order);
            }
        }
    }
}