using LearningSystem.Domain.Entities;

namespace LearningSystem.Application.Persistence;

public interface IUserCourseRepository
{
    Task<UserCourse?> GetUserCourseAsync(int userId, int courseId);
    Task AddUserCourseAsync(UserCourse userCourse);
}
