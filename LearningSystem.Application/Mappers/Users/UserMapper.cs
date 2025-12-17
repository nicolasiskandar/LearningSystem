using LearningSystem.Application.Authentication.Commands;
using LearningSystem.Application.Commands.Users;
using LearningSystem.Application.Results.Users;
using LearningSystem.Domain.Entities;

namespace LearningSystem.Application.Mappers.Users;

public class UserMapper : IUserMapper
{
    public UserResult Map(User user)
    {
        return new UserResult
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            CreatedAt = user.CreatedAt
        };
    }

    public IEnumerable<UserResult> Map(IEnumerable<User> users)
    {
        return users.Select(Map);
    }

    public User Map(CreateUserCommand command)
    {
        return new User
        {
            FullName = command.FullName,
            Email = command.Email,
            UserName = command.Email,
        };
    }

    public User Map(RegisterUserCommand command)
    {
        return new User
        {
            FullName = command.FullName,
            Email = command.Email,
            UserName = command.Email,
        };
    }

    public void Map(UpdateUserCommand command, User user)
    {
        user.FullName = command.FullName;
        user.Email = command.Email;
        user.UserName = command.Email;
    }
}
