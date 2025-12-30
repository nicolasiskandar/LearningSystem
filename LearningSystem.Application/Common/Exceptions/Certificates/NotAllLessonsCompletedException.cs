namespace LearningSystem.Application.Common.Exceptions.Certificates;

public class NotAllLessonsCompletedException : ForbiddenExcception
{
    public NotAllLessonsCompletedException(int userId, int courseId)
        : base($"User {userId} did not finish course {courseId}")
    {
    }
}
