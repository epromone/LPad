using System.Linq;
using System.Web.Http;
using WebApi.InMemoryRepository;
using WebApi.Models.Orders;

namespace WebApi.Controllers
{
    [RoutePrefix("orders")]
    public class OrdersController : ApiController
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;

        public OrdersController(IOrderRepository orderRepository, IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }

        [HttpGet]
        [Route("list")]
        public IHttpActionResult GetOrders(string productName = null, int page = 1, int pageSize = 10)
        {
            var orders = _orderRepository.GetFiltered(productName, page, pageSize);
            if (orders == null || !orders.Any())
            {
                return NotFound();
            }
            return Ok(orders);
        }

        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetOrder(int id)
        {
            var order = _orderRepository.GetById(id);
            if (order == null) 
            { 
                return NotFound(); 
            }
            return Ok(order);
        }

        [HttpPost]
        [Route("create")]
        public IHttpActionResult CreateOrder([FromBody] Order order)
        {
            if (order == null)
            {
                return BadRequest("Order is null.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid order data.");
            }

            var product = _productRepository.GetAll().FirstOrDefault(p => p.Name == order.ProductName);
            if (product == null)
            {
                return BadRequest("Invalid product name.");
            }

            //order.OrderDate = DateTime.Now;
            order.TotalAmount = order.Quantity * product.Price;
            _orderRepository.Add(order);

            return Created($"orders/{order.Id}", order);
        }

        [HttpPut]
        [Route("update/{id:int}")]
        public IHttpActionResult UpdateOrder(int id, [FromBody] Order order)
        {
            if (order == null)
            {
                return BadRequest("Order is null.");
            }
           
            if (id != order.Id)
            {
                return BadRequest("Order ID mismatch.");
            }
            var existingOrder = _orderRepository.GetById(id);
            
            if (existingOrder == null)
            {
                return NotFound();
            }

            var product = _productRepository.GetAll().FirstOrDefault(p => p.Name == order.ProductName);
            if (product == null)
            {
                return BadRequest("Invalid product name.");
            }

            order.TotalAmount = order.Quantity * product.Price;

            _orderRepository.Update(order);
            return Ok(order);
        }


        [HttpDelete]
        [Route("delete/{id:int}")]
        public IHttpActionResult DeleteOrder(int id)
        {
            var existingOrder = _orderRepository.GetById(id);
            if (existingOrder == null) 
            { 
                return NotFound(); 
            }

            _orderRepository.Delete(id);
            return Ok($"Order {id} deleted.");
        }
    }
}