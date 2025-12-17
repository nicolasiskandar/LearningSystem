using LearningSystem.Application.Authentication.Commands;
using LearningSystem.Application.Commands.Users;
using LearningSystem.Application.Results.Users;
using LearningSystem.Domain.Entities;

namespace LearningSystem.Application.Mappers.Users;

public interface IUserMapper
{
    UserResult Map(User user);
    IEnumerable<UserResult> Map(IEnumerable<User> users);
    User Map(CreateUserCommand command);
    User Map(RegisterUserCommand command);
    void Map(UpdateUserCommand command, User user);
}
