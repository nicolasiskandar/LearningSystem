namespace LearningSystem.Application.Common.Exceptions.UserCourse;

public class UserAlreadyEnrolledInCourseException : ConflictException
{
    public UserAlreadyEnrolledInCourseException(int userId, int courseId)
        : base($"User with ID {userId} is already enrolled in course with ID {courseId}")
    {
    }
}