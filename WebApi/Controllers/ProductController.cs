using System.Web.Http;
using WebApi.Models.Products;
using WebApi.InMemoryRepository;
using System.Linq;

namespace WebApi.Controllers
{
    [RoutePrefix("products")]
    public class ProductsController : ApiController
    {

        private readonly IProductRepository _repository;

        public ProductsController(IProductRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Route("list")]
        public IHttpActionResult GetProducts(string name = null, int page = 1, int pageSize = 10)
        {
            // filterd by product name
            var products = _repository.GetFiltered(name, page, pageSize);
            if (products == null || !products.Any())
            {
                return NotFound();
            }
            return Ok(products);
        }

        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetProduct(int id)
        {
            var product = _repository.GetById(id);
            if (product == null)
            { 
                return NotFound(); 
            }
            return Ok(product);
        }

        [HttpPost]
        [Route("create")]
        public IHttpActionResult CreateProduct([FromBody] Product product)
        {
            if (product == null)
            {
                return BadRequest("Product is null.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid product data.");
            }

            _repository.Add(product);
            return Created($"products/{product.Id}", product);
        }

        [HttpPut]
        [Route("update/{id:int}")]
        public IHttpActionResult UpdateProduct(int id, [FromBody] Product product)
        {
            if (product == null)
            {
                return BadRequest("Product is null.");
            }
            if (id != product.Id)
            {
                return BadRequest("Product ID mismatch."); 
            }
            var existingProduct = _repository.GetById(id);
            if (existingProduct == null)
            { 
                return NotFound(); 
            }
            _repository.Update(product);
            return Ok(product);
        }

        [HttpDelete]
        [Route("delete/{id:int}")]
        public IHttpActionResult DeleteProduct(int id)
        {
            var existingProduct = _repository.GetById(id);
            if (existingProduct == null) 
            { 
                return NotFound();
            }
            _repository.Delete(id);
            return Ok($"Product {id} deleted.");
        }

    }
}