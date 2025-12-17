using LearningSystem.Application.Commands.Users;
using LearningSystem.Application.Results.Courses;
using LearningSystem.Application.Results.Users;

namespace LearningSystem.Application.Services.Users;

public interface IUserService
{
    Task<UserResult> AddUserAsync(CreateUserCommand command);

    Task<UserResult> GetUserByIdAsync(int id);
    Task<IEnumerable<UserResult>> GetUsersAsync();

    Task<UserResult> UpdateUserAsync(UpdateUserCommand command);
    Task DeleteUserAsync(int id);

    Task<IEnumerable<CourseResult>> GetCoursesCreatedByUserAsync(int userId);
    Task<IEnumerable<CourseResult>> GetCoursesEnrolledByUserAsync(int userId);
    Task<UserResult> GetMeAsync();
}
