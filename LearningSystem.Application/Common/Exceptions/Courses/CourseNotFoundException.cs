namespace LearningSystem.Application.Common.Exceptions.Courses;

public class CourseNotFoundException : NotFoundException
{
    public CourseNotFoundException(string message) : base(message)
    {
    }

    public CourseNotFoundException(int Id) : base($"Course with ID {Id} does not exist.")
    {
    }
}
