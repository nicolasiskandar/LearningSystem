using LearningSystem.Application.Commands.Categories;
using LearningSystem.Application.Common.Exceptions.Categories;
using LearningSystem.Application.Mappers.Categories;
using LearningSystem.Application.Persistence;
using LearningSystem.Application.Results.Categories;

namespace LearningSystem.Application.Services.Categories;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICategoryMapper _categoryMapper;

    public CategoryService(ICategoryRepository categoryRepository, ICategoryMapper categoryMapper)
    {
        _categoryRepository = categoryRepository;
        _categoryMapper = categoryMapper;
    }

    public async Task<CategoryResult> CreateCategoryAsync(CreateCategoryCommand command)
    {
        var existingCategory = await _categoryRepository.GetCategoryByNameAsync(command.Name);
        if (existingCategory != null)
            throw new CategoryAlreadyExistsException($"Category with name '{command.Name}' already exists.");

        var category = _categoryMapper.Map(command);
        await _categoryRepository.AddCategoryAsync(category);

        return _categoryMapper.Map(category);
    }

    public async Task DeleteCategoryAsync(int id)
    {
        var category = await _categoryRepository.GetCategoryByIdAsync(id);
        if (category == null)
            throw new CategoryNotFoundException(id);

        if (category.Courses != null && category.Courses.Any())
            throw new CategoryHasCoursesException(id);

        await _categoryRepository.RemoveCategoryAsync(category);
    }

    public async Task<IEnumerable<CategoryResult>> GetAllCategoriesAsync()
    {
        var categories = await _categoryRepository.GetAllCategoriesAsync();
        return _categoryMapper.Map(categories);
    }

    public async Task<CategoryResult> GetCategoryByIdAsync(int id)
    {
        var category = await _categoryRepository.GetCategoryByIdAsync(id);
        if (category == null)
            throw new CategoryNotFoundException(id);

        if (category.Courses.Any())
            throw new InvalidOperationException("Cannot delete a category that has courses.");

        return _categoryMapper.Map(category);
    }

    public async Task<CategoryResult> UpdateCategoryAsync(UpdateCategoryCommand command)
    {
        var category = await _categoryRepository.GetCategoryByIdAsync(command.Id);
        if (category == null)
            throw new CategoryNotFoundException(command.Id);

        if (await CategoryWithNameAlreadyExistsAsync(command))
            throw new CategoryAlreadyExistsException($"Category with name '{command.Name}' already exists.");

        _categoryMapper.Map(command, category);
        await _categoryRepository.UpdateCategoryAsync(category);

        return _categoryMapper.Map(category);
    }

    private async Task<bool> CategoryWithNameAlreadyExistsAsync(UpdateCategoryCommand command)
    {
        var category = await _categoryRepository.GetCategoryByNameAsync(command.Name);
        return category != null && category.Id != command.Id;
    }
}
