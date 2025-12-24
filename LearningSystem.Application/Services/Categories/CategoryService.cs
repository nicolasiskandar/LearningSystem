using LearningSystem.Application.Commands.Categories;
using LearningSystem.Application.Common.Exceptions.Categories;
using LearningSystem.Application.Mappers.Categories;
using LearningSystem.Application.Persistence;
using LearningSystem.Application.Results.Categories;

using LearningSystem.Application.Common.Caching;

namespace LearningSystem.Application.Services.Categories;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICategoryMapper _categoryMapper;
    private readonly ICacheService _cacheService;

    public CategoryService(
        ICategoryRepository categoryRepository, 
        ICategoryMapper categoryMapper,
        ICacheService cacheService)
    {
        _categoryRepository = categoryRepository;
        _categoryMapper = categoryMapper;
        _cacheService = cacheService;
    }

    public async Task<CategoryResult> CreateCategoryAsync(CreateCategoryCommand command)
    {
        var existingCategory = await _categoryRepository.GetCategoryByNameAsync(command.Name);
        if (existingCategory != null)
            throw new CategoryAlreadyExistsException($"Category with name '{command.Name}' already exists.");

        var category = _categoryMapper.Map(command);
        await _categoryRepository.AddCategoryAsync(category);
        
        await _cacheService.RemoveAsync("categories-all");

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
        
        await _cacheService.RemoveAsync($"category-{id}");
        await _cacheService.RemoveAsync("categories-all");
    }

    public async Task<IEnumerable<CategoryResult>> GetAllCategoriesAsync()
    {
        var cachedCategories = await _cacheService.GetAsync<IEnumerable<CategoryResult>>("categories-all");
        if (cachedCategories != null)
            return cachedCategories;

        var categories = await _categoryRepository.GetAllCategoriesAsync();
        var result = _categoryMapper.Map(categories);
        await _cacheService.SetAsync("categories-all", result);

        return result;
    }

    public async Task<CategoryResult> GetCategoryByIdAsync(int id)
    {
        var cachedCategory = await _cacheService.GetAsync<CategoryResult>($"category-{id}");
        if (cachedCategory != null)
            return cachedCategory;

        var category = await _categoryRepository.GetCategoryByIdAsync(id);
        if (category == null)
            throw new CategoryNotFoundException(id);

        var result = _categoryMapper.Map(category);
        await _cacheService.SetAsync($"category-{id}", result);

        return result;
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

        await _cacheService.RemoveAsync($"category-{command.Id}");
        await _cacheService.RemoveAsync("categories-all");

        return _categoryMapper.Map(category);
    }

    private async Task<bool> CategoryWithNameAlreadyExistsAsync(UpdateCategoryCommand command)
    {
        var category = await _categoryRepository.GetCategoryByNameAsync(command.Name);
        return category != null && category.Id != command.Id;
    }
}
