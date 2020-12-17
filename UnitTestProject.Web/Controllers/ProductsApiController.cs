using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UnitTestProject.Web.Models;
using UnitTestProject.Web.Repository;

namespace UnitTestProject.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductsApiController : ControllerBase
    {
        private readonly IRepository<Product> _repository;
        public ProductsApiController(IRepository<Product> repository)
        {
            _repository = repository;
        }
        // GET: api/<ProductsApiController>
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            return Ok(await _repository.GetAll());
        }
        // GET api/<ProductsApiController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            return Ok(await _repository.GetById(id));
        }
        [HttpPost]
        public async Task<IActionResult> PostProduct([FromBody] Product product)
        {
            await _repository.Create(product);
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }
        // PUT api/productsapi/PutProduct/2
        [HttpPut("{id}")]
        public IActionResult PutProduct(int id, [FromBody] Product product)
        {
            if (product is null)
                return NotFound();
            if (product.Id != id)
                return BadRequest();
            _repository.Update(product);
            return NoContent();
        }
        // DELETE api/productsapi/DeleteProduct/2
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _repository.GetById(id);
            if (product is null)
                return BadRequest("Product not find");
            _repository.Delete(product);
            return NoContent();
        }
    }
}
