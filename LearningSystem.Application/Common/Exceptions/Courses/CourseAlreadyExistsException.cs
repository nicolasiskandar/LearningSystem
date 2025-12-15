namespace LearningSystem.Application.Common.Exceptions.Courses;

public class CourseAlreadyExistsException : AlreadyExistsException
{
    public CourseAlreadyExistsException(string message) : base(message)
    {
    }
}
