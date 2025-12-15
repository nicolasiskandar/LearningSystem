using LearningSystem.Application.Commands.Categories;
using LearningSystem.Application.Results.Categories;
using LearningSystem.Domain.Entities;

namespace LearningSystem.Application.Mappers.Categories;

public class CategoryMapper : ICategoryMapper
{
    public CategoryResult Map(Category category)
    {
        return new CategoryResult
        {
            Id = category.Id,
            Name = category.Name
        };
    }

    public IEnumerable<CategoryResult> Map(IEnumerable<Category> categories)
    {
        return categories.Select(Map);
    }

    public Category Map(CreateCategoryCommand command)
    {
        return new Category { Name = command.Name };
    }

    public void Map(UpdateCategoryCommand command, Category category)
    {
        category.Id = command.Id;
        category.Name = command.Name;
    }
}
