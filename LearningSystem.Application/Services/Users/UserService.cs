using AutoMapper;
using LearningSystem.Application.Dtos.Users;
using LearningSystem.Application.Exceptions;
using LearningSystem.Application.Persistence;
using LearningSystem.Application.Security;
using LearningSystem.Domain.Entities;

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

    public UserDto GetUserById(int id)
    {
        var user = _userRepository.GetUserById(id);
        if (user == null)
            throw new UserNotFoundException($"User with ID {id} does not exist.");
        
        return _mapper.Map<UserDto>(user);
    }

    public ICollection<UserDto> GetUsers()
    {
        var users = _userRepository.GetUsers();
        return _mapper.Map<ICollection<UserDto>>(users);
    }

    public UserDto AddUser(CreateUserDto dto)
    {
        var user = _userRepository.GetUserByEmail(dto.Email);

        if (user != null)
            throw new UserAlreadyExistsException($"User with email {dto.Email} already exists.");

        user = _mapper.Map<User>(dto);
        user.HashedPassword = _passwordHasher.HashPassword(dto.Password);
        user.CreatedAt = DateTime.UtcNow;
        user.RoleId = 1; // Student by default

        _userRepository.AddUser(user);

        var createdUser = _userRepository.GetUserById(user.Id);
        return _mapper.Map<UserDto>(createdUser!);
    }

    public UserDto UpdateUser(int id, UpdateUserDto dto)
    {
        var user = _userRepository.GetUserById(id);

        if (user == null)
            throw new UserNotFoundException($"User with ID {id} does not exist.");
        if (user.Email == dto.Email)
            throw new UserAlreadyExistsException($"User with email {dto.Email} already exists.");

        _mapper.Map(dto, user);
        _userRepository.UpdateUser(user);

        return _mapper.Map<UserDto>(user);
    }

    public void DeleteUser(int id)
    {
        var user = _userRepository.GetUserById(id);
        if (user == null)
            throw new UserNotFoundException($"User with ID {id} does not exist.");

        _userRepository.DeleteUser(user);
    }
}
