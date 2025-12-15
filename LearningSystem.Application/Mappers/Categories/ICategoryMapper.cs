using LearningSystem.Application.Commands.Categories;
using LearningSystem.Application.Results.Categories;
using LearningSystem.Domain.Entities;

namespace LearningSystem.Application.Mappers.Categories;

public interface ICategoryMapper
{
    CategoryResult Map(Category category);
    IEnumerable<CategoryResult> Map(IEnumerable<Category> categories);
    Category Map(CreateCategoryCommand command);
    void Map(UpdateCategoryCommand command, Category category);
}
