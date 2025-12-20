namespace LearningSystem.Application.Common.Exceptions.Lessons;

public class LessonNotFoundException : NotFoundException
{
    public LessonNotFoundException(int id) : base($"Lesson with id {id} not found.")
    {
    }
}
