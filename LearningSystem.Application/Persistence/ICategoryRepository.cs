using LearningSystem.Domain.Entities;

namespace LearningSystem.Application.Persistence;

public interface ICategoryRepository
{
    Category? GetCategoryById(int id);
    Category? GetCategoryByName(string name);
    ICollection<Category> GetAllCategories();
    void AddCategory(Category category);
    void UpdateCategory(Category category);
    void RemoveCategory(Category category);
}
