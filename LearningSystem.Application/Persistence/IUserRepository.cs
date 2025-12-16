using LearningSystem.Domain.Entities;

namespace LearningSystem.Application.Persistence;

public interface IUserRepository
{
    User? GetUserById(int id);
    User? GetUserByEmail(string email);
    ICollection<User> GetUsers();
    User? GetUserByIdWithCourses(int id);
    User? GetUserByIdWithEnrolledCourses(int id);
    void AddUser(User user);
    void UpdateUser(User user);
    void DeleteUser(User user);
}
