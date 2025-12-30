namespace LearningSystem.Application.Common.Exceptions.Lessons;

public class InvalidLessonDurationException : FailedException
{
    public InvalidLessonDurationException(int duration)
        : base($"Invalid lesson duration: {duration}. Duration must be a positive value.")
    {
    }
}
