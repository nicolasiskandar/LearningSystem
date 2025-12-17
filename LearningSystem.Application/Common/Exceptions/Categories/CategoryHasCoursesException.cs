namespace LearningSystem.Application.Common.Exceptions.Categories;

public class CategoryHasCoursesException : Exception
{
    public CategoryHasCoursesException(int categoryId)
        : base($"Cannot delete category with ID {categoryId} because it has associated courses.")
    {
    }
}
