using LearningSystem.Application.Common.Security;
using LearningSystem.Application.Common.Exceptions.Users;
using LearningSystem.Application.Persistence;
using LearningSystem.Application.Commands.Users;
using LearningSystem.Application.Results.Users;
using LearningSystem.Application.Mappers.Users;

namespace LearningSystem.Application.Services.Users;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IUserMapper _userMapper;
    private readonly IPasswordHasher _passwordHasher;

    public UserService(IUserRepository userRepository, IUserMapper userMapper, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _userMapper = userMapper;
        _passwordHasher = passwordHasher;
    }

    public UserResult GetUserById(int id)
    {
        var user = _userRepository.GetUserById(id);
        if (user == null)
            throw new UserNotFoundException(id);
        
        return _userMapper.Map(user);
    }

    public IEnumerable<UserResult> GetUsers()
    {
        var users = _userRepository.GetUsers();
        return _userMapper.Map(users);
    }

    public UserResult AddUser(CreateUserCommand command)
    {
        var existingUser = _userRepository.GetUserByEmail(command.Email);
        if (existingUser != null)
            throw new UserAlreadyExistsException($"User with email {command.Email} already exists.");

        var user = _userMapper.Map(command);
        user.HashedPassword = _passwordHasher.HashPassword(command.Password);
        user.CreatedAt = DateTime.UtcNow;
        user.RoleId = 1; // Student by default

        _userRepository.AddUser(user);

        var createdUser = _userRepository.GetUserById(user.Id);
        return _userMapper.Map(createdUser!);
    }

    public UserResult UpdateUser(UpdateUserCommand command)
    {
        var user = _userRepository.GetUserById(command.Id);

        if (user == null)
            throw new UserNotFoundException(command.Id);
        if (UserWithEmailAlreadyExists(command))
            throw new UserAlreadyExistsException($"User with email {command.Email} already exists.");

        _userMapper.Map(command, user);
        _userRepository.UpdateUser(user);

        return _userMapper.Map(user);
    }

    public void DeleteUser(int id)
    {
        var user = _userRepository.GetUserById(id);
        if (user == null)
            throw new UserNotFoundException(id);

        _userRepository.DeleteUser(user);
    }

    private bool UserWithEmailAlreadyExists(UpdateUserCommand command)
    {
        var existingUser = _userRepository.GetUserByEmail(command.Email);
        return existingUser != null && existingUser.Id != command.Id;
    }
}
