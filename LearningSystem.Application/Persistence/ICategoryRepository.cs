using LearningSystem.Domain.Entities;

namespace LearningSystem.Application.Persistence;

public interface ICategoryRepository
{
    Task<Category?> GetCategoryByIdAsync(int id);
    Task<Category?> GetCategoryByNameAsync(string name);
    Task<ICollection<Category>> GetAllCategoriesAsync();
    Task AddCategoryAsync(Category category);
    Task UpdateCategoryAsync(Category category);
    Task RemoveCategoryAsync(Category category);
}
