using LearningSystem.Application.Authentication.Commands;
using LearningSystem.Application.Authentication.Common;
using LearningSystem.Application.Authentication.Queries;
using LearningSystem.Application.Common.Exceptions.Users;
using LearningSystem.Application.Common.Security;
using LearningSystem.Application.Mappers.Users;
using LearningSystem.Domain.Entities;
using LearningSystem.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace LearningSystem.Application.Services.Auth;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IUserMapper _userMapper;
    private readonly IJwtTokenGenerator _jwtGenerator;

    public AuthService(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IUserMapper userMapper,
        IJwtTokenGenerator jwtGenerator)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _userMapper = userMapper;
        _jwtGenerator = jwtGenerator;
    }

    public async Task<AuthenticationResult> RegisterAsync(RegisterUserCommand command)
    {
        var existingUser = await _userManager.FindByEmailAsync(command.Email);
        if (existingUser != null)
            throw new UserAlreadyExistsException($"User with email {command.Email} already exists.");

        var user = _userMapper.Map(command);
        user.CreatedAt = DateTime.UtcNow;

        var result = await _userManager.CreateAsync(user, command.Password);
        if (!result.Succeeded)
            throw new UserRegistrationFailedException();

        await _userManager.AddToRoleAsync(user, Roles.Student.ToString());

        var roles = await _userManager.GetRolesAsync(user);
        var token = _jwtGenerator.GenerateToken(user, roles);

        return new AuthenticationResult(user, token);
    }

    public async Task<AuthenticationResult> LoginAsync(LoginQuery query)
    {
        var user = await _userManager.FindByEmailAsync(query.Email);
        if (user == null)
            throw new UnauthorizedAccessException("Invalid credentials.");

        var result = await _signInManager.CheckPasswordSignInAsync(user, query.Password, false);
        if (!result.Succeeded)
            throw new UnauthorizedAccessException("Invalid credentials.");

        var roles = await _userManager.GetRolesAsync(user);
        var token = _jwtGenerator.GenerateToken(user, roles);

        return new AuthenticationResult(user, token);
    }
}
