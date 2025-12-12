using LearningSystem.Application.Dtos.Users;

namespace LearningSystem.Application.Services.Users
{
    public interface IUserService
    {
        UserDto GetUserById(int id);
        ICollection<UserDto> GetUsers();
        UserDto AddUser(CreateUserDto dto);
        UserDto UpdateUser(int id, UpdateUserDto dto);
        void DeleteUser(int id);
    }
}
