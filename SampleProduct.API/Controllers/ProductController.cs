using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SampleProduct.ORM.Models.DB;
using SampleProduct.Service;

namespace SampleProduct.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IRepository<Product> productRepository;

        public ProductController(IRepository<Product> productRepository)
        {
            this.productRepository = productRepository;
        }


        // GET api/products
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(productRepository.Where(a => a.IsActive == true));
        }

        // GET api/product/5
        [HttpGet("{id}")]
        public IActionResult Get(int? id)
        {
            if (!id.HasValue) return BadRequest();

            var product = productRepository.Where(a => a.ProductId == id).FirstOrDefault();
            if (product == null) return NotFound();

            return Ok(product);
        }

        // POST api/products
        [HttpPost]
        public IActionResult Post([FromBody]Product product)
        {
            if (!ModelState.IsValid || product == null) return BadRequest();
            product.IsActive = true;
            if (productRepository.Add(product))
                return Ok(product);

            return BadRequest();
        }

        // PUT api/products/5
        [HttpPut("{id}")]
        public IActionResult Put(int? id, [FromBody]Product product)
        {
            if (!id.HasValue) return BadRequest();
            if (!ModelState.IsValid || product == null) return BadRequest();

            var oldProduct = productRepository.Where(a => a.ProductId == id).FirstOrDefault();
            if (oldProduct == null) return NotFound();

            product.ProductId = oldProduct.ProductId;
            if (productRepository.Update(product))
                return Ok(product);
            return BadRequest();
        }

        // DELETE api/products/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int? id)
        {
            if (!id.HasValue) return BadRequest();

            var product = productRepository.Where(a => a.ProductId == id).FirstOrDefault();
            if (product == null) return NotFound();
            product.IsActive = false;
            if (productRepository.Update(product))
                return Ok(product);
            return BadRequest();
        }
    }
}