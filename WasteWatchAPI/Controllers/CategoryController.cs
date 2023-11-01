using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WasteWatch.Models;
using WasteWatch.DataAccessLayer;
using WasteWatch.Data;

namespace WasteWatch.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryRepository _categoryRepository;

        public CategoryController(ApplicationDbContext dbContext)
        {
            _categoryRepository = new CategoryRepository(dbContext);
        }

        [HttpGet]
        public IActionResult GetAllCategories()
        {
            var categories = _categoryRepository.GetAllCategories();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public IActionResult GetCategoryById(int id)
        {
            var category = _categoryRepository.GetCategoryById(id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        [HttpPost]
        public IActionResult AddCategory(Category category)
        {
            _categoryRepository.AddCategory(category);
            return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, category);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCategory(int id, Category category)
        {
            if (id != category.Id)
            {
                return BadRequest();
            }

            _categoryRepository.UpdateCategory(category);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCategory(int id)
        {
            _categoryRepository.DeleteCategory(id);
            return NoContent();
        }
    }
}
