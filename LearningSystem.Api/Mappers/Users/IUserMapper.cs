using LearningSystem.Api.Dtos.Users;
using LearningSystem.Application.Commands.Users;
using LearningSystem.Application.Results.Users;

namespace LearningSystem.Api.Mappers.Users;

public interface IUserMapper
{
    UserDto Map(UserResult result);
    IEnumerable<UserDto> Map(IEnumerable<UserResult> results);
    CreateUserCommand Map(CreateUserDto dto);
    UpdateUserCommand Map(UpdateUserDto dto, int id);
}
