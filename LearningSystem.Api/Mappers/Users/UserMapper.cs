using LearningSystem.Api.Dtos.Users;
using LearningSystem.Application.Commands.Users;
using LearningSystem.Application.Results.Users;

namespace LearningSystem.Api.Mappers.Users;

public class UserMapper : IUserMapper
{
    public UserDto Map(UserResult result)
    {
        return new UserDto
        {
            Id = result.Id,
            FullName = result.FullName,
            Email = result.Email,
            RoleName = result.RoleName,
            CreatedAt = result.CreatedAt
        };
    }

    public IEnumerable<UserDto> Map(IEnumerable<UserResult> results)
    {
        return results.Select(Map);
    }

    public CreateUserCommand Map(CreateUserDto dto)
    {
        return new CreateUserCommand(
            dto.FullName,
            dto.Email,
            dto.Password
        );
    }

    public UpdateUserCommand Map(UpdateUserDto dto, int id)
    {
        return new UpdateUserCommand(
            id,
            dto.FullName,
            dto.Email
        );
    }
}
