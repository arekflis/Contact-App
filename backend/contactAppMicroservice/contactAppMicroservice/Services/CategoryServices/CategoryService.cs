using AutoMapper;
using contactAppMicroservice.Database;
using contactAppMicroservice.DTO.Response;
using Microsoft.EntityFrameworkCore;

namespace contactAppMicroservice.Services.CategoryServices
{
    public class CategoryService(ContactDbContext contactDbContext, IMapper mapper): ICategoryService
    {

        public async Task<List<CategoryResponse>?> getCategories()
        {
            var categories = await contactDbContext.Categories.ToListAsync();

            return mapper.Map<List<CategoryResponse>>(categories);
        }

        public async Task<List<SubcategoryResponse>?> getSubcategoriesByCategories(Guid categoryId)
        {
            var category = await contactDbContext.Categories.FindAsync(categoryId);
            if (category == null)
                throw new ArgumentException("Invalid category id");

            var subcategories = await contactDbContext.Subcategories.Where(s => s.CategoryId == categoryId).ToListAsync();

            return mapper.Map<List<SubcategoryResponse>>(subcategories);
        }
    }
}
