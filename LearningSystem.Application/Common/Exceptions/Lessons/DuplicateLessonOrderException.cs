namespace LearningSystem.Application.Common.Exceptions.Lessons;

public class DuplicateLessonOrderException : FailedException
{
    public DuplicateLessonOrderException(int courseId, int order)
        : base($"A lesson with order {order} already exists for course {courseId}")
    {
    }
}