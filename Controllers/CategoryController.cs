using E_commerce.DTOs;
using E_commerce.Models;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoryController : ControllerBase
    {
        private static List<Category> categories = new List<Category>();

        // GET Request /api/categories
        [HttpGet]
        public IActionResult GetCategories([FromQuery] string searchValue = "")
        {

            if (!string.IsNullOrEmpty(searchValue))
            {
                var searchCategories = categories.Where(c => !string.IsNullOrEmpty(c.Name) && c.Name.Contains(searchValue, StringComparison.OrdinalIgnoreCase)).ToList();
                return Ok(searchCategories);
            }

            return Ok(categories);
        }

        // POST Request /api/categories
        [HttpPost]
        public IActionResult CreateCategory([FromBody] CategoryCreateDto categoryData)
        {

            if (string.IsNullOrEmpty(categoryData.Name))
            {
                return BadRequest("Category name is required and must not be empty");
            }

            if (categoryData.Name.Length <= 2)
            {
                return BadRequest("Category name must be 2 characters long");
            }



            var newCategory = new Category
            {
                CategoryId = Guid.NewGuid(),
                Name = categoryData.Name,
                Description = categoryData.Description,
                CreatedAt = DateTime.UtcNow
            };

            categories.Add(newCategory);
            return Created($"/api/categories/{newCategory.CategoryId}", newCategory);
        }


        // DELETE Request /api/categories/{categoryID}
        [HttpDelete("{categoryID:guid}")]
        public IActionResult DeleteCategoryById(Guid categoryID)
        {

            var foundCategory = categories.FirstOrDefault(category => category.CategoryId == categoryID);
            if (foundCategory == null)
            {
                return NotFound("Category with this does not exists");
            }
            categories.Remove(foundCategory);
            return NoContent();
        }
         
        
        // PUT Request /api/categories/{categoryID}
        [HttpPut("{categoryID:guid}")]
        public IActionResult UpdateCategoryById(Guid categoryID, [FromBody] CategoryUpdateDto categoryData)
        {

            var foundCategory = categories.FirstOrDefault(category => category.CategoryId == categoryID);
            if (foundCategory == null)
            {
                return NotFound("Category with this does not exists");
            }
            if (categoryData == null)
            {
                return BadRequest("Category data is missing");
            }

            if (!string.IsNullOrWhiteSpace(categoryData.Name))
            {
                if (categoryData.Name.Length >= 2)
                {
                    foundCategory.Name = categoryData.Name;
                }
                else
                {
                    return BadRequest("Category name must be 2 characters long");
                }

                foundCategory.Name = categoryData.Name;
            }
            if (!string.IsNullOrEmpty(categoryData.Description))
            {
                foundCategory.Description = categoryData.Description;
            }


            return NoContent();
        }
    }
}

