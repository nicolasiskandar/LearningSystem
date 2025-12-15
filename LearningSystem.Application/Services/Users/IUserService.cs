using LearningSystem.Application.Commands.Users;
using LearningSystem.Application.Results.Users;

namespace LearningSystem.Application.Services.Users;

public interface IUserService
{
    UserResult GetUserById(int id);
    IEnumerable<UserResult> GetUsers();
    UserResult AddUser(CreateUserCommand command);
    UserResult UpdateUser(UpdateUserCommand command);
    void DeleteUser(int id);
}
