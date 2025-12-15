namespace LearningSystem.Application.Common.Exceptions.Categories;

public class CategoryNotFoundException : NotFoundException
{
    public CategoryNotFoundException(string message) : base(message)
    {
    }

    public CategoryNotFoundException(int categoryId) : base($"Category with id {categoryId} not found")
    {
    }
}
