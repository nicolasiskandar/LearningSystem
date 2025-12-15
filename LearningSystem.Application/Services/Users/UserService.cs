using AutoMapper;
using LearningSystem.Application.Common.Security;
using LearningSystem.Application.Common.Exceptions;
using LearningSystem.Application.Persistence;
using LearningSystem.Domain.Entities;
using LearningSystem.Application.Commands.Users;
using LearningSystem.Application.Results.Users;

namespace LearningSystem.Application.Services.Users;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher _passwordHasher;

    public UserService(IUserRepository userRepository, IMapper mapper, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _passwordHasher = passwordHasher;
    }

    public UserResult GetUserById(int id)
    {
        var user = _userRepository.GetUserById(id);
        if (user == null)
            throw new UserNotFoundException($"User with ID {id} does not exist.");
        
        return _mapper.Map<UserResult>(user);
    }

    public ICollection<UserResult> GetUsers()
    {
        var users = _userRepository.GetUsers();
        return _mapper.Map<ICollection<UserResult>>(users);
    }

    public UserResult AddUser(CreateUserCommand command)
    {
        var user = _userRepository.GetUserByEmail(command.Email);

        if (user != null)
            throw new UserAlreadyExistsException($"User with email {command.Email} already exists.");

        user = _mapper.Map<User>(command);
        user.HashedPassword = _passwordHasher.HashPassword(command.Password);
        user.CreatedAt = DateTime.UtcNow;
        user.RoleId = 1; // Student by default

        _userRepository.AddUser(user);

        var createdUser = _userRepository.GetUserById(user.Id);
        return _mapper.Map<UserResult>(createdUser!);
    }

    public UserResult UpdateUser(UpdateUserCommand command)
    {
        var user = _userRepository.GetUserById(command.Id);

        if (user == null)
            throw new UserNotFoundException($"User with ID {command.Id} does not exist.");
        if (UserWithEmailAlreadyExists(user, command))
            throw new UserAlreadyExistsException($"User with email {command.Email} already exists.");

        _mapper.Map(command, user);
        _userRepository.UpdateUser(user);

        return _mapper.Map<UserResult>(user);
    }

    public void DeleteUser(int id)
    {
        var user = _userRepository.GetUserById(id);
        if (user == null)
            throw new UserNotFoundException($"User with ID {id} does not exist.");

        _userRepository.DeleteUser(user);
    }

    private bool UserWithEmailAlreadyExists(User user, UpdateUserCommand command)
    {
        return user.Email != command.Email && _userRepository.GetUserByEmail(user.Email) != null;
    }
}
