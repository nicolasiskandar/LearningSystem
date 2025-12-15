using LearningSystem.Api.Dtos.Categories;
using LearningSystem.Application.Commands.Categories;
using LearningSystem.Application.Results.Categories;

namespace LearningSystem.Api.Mappers.Categories;

public interface ICategoryMapper
{
    CategoryDto Map(CategoryResult result);
    IEnumerable<CategoryDto> Map(IEnumerable<CategoryResult> results);
    CreateCategoryCommand Map(CreateCategoryDto dto);
    UpdateCategoryCommand Map(UpdateCategoryDto dto, int id);
}
