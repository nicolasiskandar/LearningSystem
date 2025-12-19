using LearningSystem.Domain.Entities;

namespace LearningSystem.Application.Persistence;

public interface IUserRepository
{
    Task<User?> GetUserByIdAsync(int id);
    Task<User?> GetUserByEmailAsync(string email);
    Task<IEnumerable<User>> GetUsersAsync();
    Task<User?> GetUserByIdWithCoursesAsync(int id);
    Task<User?> GetUserByIdWithEnrolledCoursesAsync(int id);
    Task AddUserAsync(User user);
    Task UpdateUserAsync(User user);
    Task DeleteUserAsync(User user);
}
