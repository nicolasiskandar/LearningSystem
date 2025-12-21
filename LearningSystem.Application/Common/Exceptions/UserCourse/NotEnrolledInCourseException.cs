namespace LearningSystem.Application.Common.Exceptions.UserCourse;

public class NotEnrolledInCourseException : ForbiddenExcception
{
    public NotEnrolledInCourseException(int userId, int courseId)
        : base($"User with ID {userId} is not enrolled in course with ID {courseId}")
    {
    }
}