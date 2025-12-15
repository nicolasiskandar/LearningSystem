using LearningSystem.Api.Dtos.Categories;
using LearningSystem.Application.Commands.Categories;
using LearningSystem.Application.Results.Categories;

namespace LearningSystem.Api.Mappers.Categories;

public class CategoryMapper : ICategoryMapper
{
    public CategoryDto Map(CategoryResult result)
    {
        return new CategoryDto
        {
            Id = result.Id,
            Name = result.Name
        };
    }

    public IEnumerable<CategoryDto> Map(IEnumerable<CategoryResult> results)
    {
        return results.Select(Map);
    }

    public CreateCategoryCommand Map(CreateCategoryDto dto)
    {
        return new CreateCategoryCommand
        {
            Name = dto.Name
        };
    }

    public UpdateCategoryCommand Map(UpdateCategoryDto dto, int id)
    {
        return new UpdateCategoryCommand
        {
            Id = id,
            Name = dto.Name
        };
    }
}
