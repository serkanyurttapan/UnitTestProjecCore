using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnitTestProject.Web.Models;
using UnitTestProject.Web.Repository;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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

        // POST api/<ProductsApiController>
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

        // DELETE api/<ProductsApiController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _repository.GetById(id);
            if (product is null)
                return BadRequest("Product not find");
               _repository.Delete(product);
            return NoContent();
        }
    }
}
