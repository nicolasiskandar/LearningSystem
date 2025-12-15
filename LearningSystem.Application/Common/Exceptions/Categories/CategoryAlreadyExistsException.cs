namespace LearningSystem.Application.Common.Exceptions.Categories;

public class CategoryAlreadyExistsException : AlreadyExistsException
{
    public CategoryAlreadyExistsException(string message) : base(message)
    {
    }
}
