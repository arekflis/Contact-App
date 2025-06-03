using contactAppMicroservice.DTO.Response;
using contactAppMicroservice.Services.CategoryServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace contactAppMicroservice.Controllers.Category
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController(ICategoryService categoryService) : ControllerBase
    {

        [HttpGet("categories")]
        [Authorize]
        public async Task<ActionResult<List<CategoryResponse>>> getAllCategories()
        {
            var categoriesResponse = await categoryService.getCategories();

            if (categoriesResponse is null || categoriesResponse.Count == 0)
                return NotFound();

            return Ok(categoriesResponse);
        }

        [HttpGet("subcategories/{categoryId}")]
        [Authorize]
        public async Task<ActionResult<List<CategoryResponse>>> getAllCategories(Guid categoryId)
        {
            try
            {
                var subcategoriesResponse = await categoryService.getSubcategoriesByCategories(categoryId);

                if (subcategoriesResponse is null || subcategoriesResponse.Count == 0)
                    return NotFound();

                return Ok(subcategoriesResponse);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
