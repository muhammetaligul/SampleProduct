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
    public class CategoryController : ControllerBase
    {
        private readonly IRepository<Category> categoryRepository;

        public CategoryController(IRepository<Category> categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }


        // GET api/categories
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(categoryRepository.All());
        }

        // GET api/category/5
        [HttpGet("{id}")]
        public IActionResult Get(int? id)
        {
            if (!id.HasValue) return BadRequest();

            var category = categoryRepository.Where(a=>a.CategoryId == id).FirstOrDefault();
            if (category == null) return NotFound();

            return Ok(category);
        }

        // POST api/categories
        [HttpPost]
        public IActionResult Post([FromBody]Category category)
        {
            if (!ModelState.IsValid || category == null) return BadRequest();

            if (categoryRepository.Add(category))
                return Ok(category);

            return BadRequest();
        }

        // PUT api/categories/5
        [HttpPut("{id}")]
        public IActionResult Put(int? id, [FromBody]Category category)
        {
            if (!id.HasValue) return BadRequest();
            if (!ModelState.IsValid || category == null) return BadRequest();

            var oldCategory = categoryRepository.Where(a=>a.CategoryId == id).FirstOrDefault();
            if (oldCategory== null) return NotFound();

            category.CategoryId = oldCategory.CategoryId;
            if (categoryRepository.Update(category))
                return Ok(category);
            return BadRequest();
        }

        // DELETE api/categories/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int? id)
        {
            if (!id.HasValue) return BadRequest();

            var category = categoryRepository.Where(a=>a.CategoryId == id).FirstOrDefault();
            if (category == null) return NotFound();

            if (categoryRepository.Delete(category))
                return Ok(category);
            return BadRequest();
        }
    }
}