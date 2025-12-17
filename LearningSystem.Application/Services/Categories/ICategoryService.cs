using LearningSystem.Application.Commands.Categories;
using LearningSystem.Application.Results.Categories;

namespace LearningSystem.Application.Services.Categories;

public interface ICategoryService
{
    Task<CategoryResult> GetCategoryByIdAsync(int id);
    Task<IEnumerable<CategoryResult>> GetAllCategoriesAsync();
    Task<CategoryResult> CreateCategoryAsync(CreateCategoryCommand command);
    Task<CategoryResult> UpdateCategoryAsync(UpdateCategoryCommand command);
    Task DeleteCategoryAsync(int id);
}
