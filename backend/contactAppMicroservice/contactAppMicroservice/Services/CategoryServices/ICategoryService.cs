using contactAppMicroservice.DTO.Response;

namespace contactAppMicroservice.Services.CategoryServices
{
    public interface ICategoryService
    {
        Task<List<CategoryResponse>?> getCategories();
        Task<List<SubcategoryResponse>?> getSubcategoriesByCategories(Guid categoryId);
    }
}
