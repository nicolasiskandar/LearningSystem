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

    public CategoryResult CreateCategory(CreateCategoryCommand command)
    {
        var existingCategory = _categoryRepository.GetCategoryByName(command.Name);
        if (existingCategory != null)
            throw new CategoryAlreadyExistsException($"Category with name '{command.Name}' already exists.");

        var category = _categoryMapper.Map(command);
        _categoryRepository.AddCategory(category);

        return _categoryMapper.Map(category);
    }

    public void DeleteCategory(int id)
    {
        var category = _categoryRepository.GetCategoryById(id);
        if (category == null)
            throw new CategoryNotFoundException(id);

        _categoryRepository.RemoveCategory(category);
    }

    public IEnumerable<CategoryResult> GetAllCategories()
    {
        var categories = _categoryRepository.GetAllCategories();
        return _categoryMapper.Map(categories);
    }

    public CategoryResult GetCategoryById(int id)
    {
        var category = _categoryRepository.GetCategoryById(id);
        if (category == null)
            throw new CategoryNotFoundException(id);

        return _categoryMapper.Map(category);
    }

    public CategoryResult UpdateCategory(UpdateCategoryCommand command)
    {
        var category = _categoryRepository.GetCategoryById(command.Id);

        if (category == null)
            throw new CategoryNotFoundException(command.Id);
        if (CategoryWithNameAlreadyExists(command))
            throw new CategoryAlreadyExistsException($"Category with name '{command.Name}' already exists.");

        _categoryMapper.Map(command, category);
        _categoryRepository.UpdateCategory(category);

        return _categoryMapper.Map(category);
    }

    private bool CategoryWithNameAlreadyExists(UpdateCategoryCommand command)
    {
        var category = _categoryRepository.GetCategoryByName(command.Name);
        return category != null && category.Id != command.Id;
    }
}
