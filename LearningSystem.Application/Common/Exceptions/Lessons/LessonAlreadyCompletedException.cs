namespace LearningSystem.Application.Common.Exceptions.Lessons;

public class LessonAlreadyCompletedException : ConflictException
{
    public LessonAlreadyCompletedException(string message) : base(message)
    {
    }
}
