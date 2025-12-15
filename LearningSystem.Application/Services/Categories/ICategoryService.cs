using LearningSystem.Application.Commands.Categories;
using LearningSystem.Application.Results.Categories;

namespace LearningSystem.Application.Services.Categories;

public interface ICategoryService
{
    CategoryResult GetCategoryById(int id);
    IEnumerable<CategoryResult> GetAllCategories();
    CategoryResult CreateCategory(CreateCategoryCommand command);
    CategoryResult UpdateCategory(UpdateCategoryCommand command);
    void DeleteCategory(int id);
}
