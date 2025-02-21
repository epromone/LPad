using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Helpers;
using System.Web.Http;
using System.Xml.Linq;
using WebApi.Models.Products;
using WebApi.InMemoryRepository;
using System.Net.Http;
using BusinessEntities;
using WebApi.Models.Users;

namespace WebApi.Controllers
{
    [RoutePrefix("api/product")]
    public class ProductController : ApiController
    {

        private readonly ProductRepository _repository = new ProductRepository();

        [HttpGet]
        [Route("")]
        public IEnumerable<Product> GetProducts()
        {
            return _repository.GetAll();
        }

        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetProduct(int id)
        {
            var product = _repository.GetById(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult CreateProduct([FromBody] Product product)
        {
            if (product == null)
            {
                return BadRequest("Invalid product data.");
            }

            _repository.Add(product);
            return Created($"products/{product.Id}", product);
        }

        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult UpdateProduct(int id, [FromBody] Product product)
        {
            if (product == null || id != product.Id) return BadRequest("Product ID mismatch.");
            var existingProduct = _repository.GetById(id);
            if (existingProduct == null) return NotFound();
            _repository.Update(product);
            return Ok(product);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult DeleteProduct(int id)
        {
            var existingProduct = _repository.GetById(id);
            if (existingProduct == null) return NotFound();
            _repository.Delete(id);
            return Ok($"Product {id} deleted.");
        }

    }
}