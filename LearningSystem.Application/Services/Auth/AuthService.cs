using LearningSystem.Application.Authentication.Commands;
using LearningSystem.Application.Authentication.Common;
using LearningSystem.Application.Authentication.Queries;
using LearningSystem.Application.Common.Exceptions;
using LearningSystem.Application.Common.Exceptions.Users;
using LearningSystem.Application.Common.Security;
using LearningSystem.Application.Mappers.Users;
using LearningSystem.Domain.Entities;
using LearningSystem.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace LearningSystem.Application.Services.Auth;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IUserMapper _userMapper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IJwtTokenGenerator _jwtGenerator;


    public AuthService(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IUserMapper userMapper,
        IHttpContextAccessor httpContextAccessor,
        IJwtTokenGenerator jwtGenerator)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _userMapper = userMapper;
        _httpContextAccessor = httpContextAccessor;
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
        var refreshToken = _jwtGenerator.CreateRefreshToken(user);

        return new AuthenticationResult(user, token, refreshToken);
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
        var refreshToken = _jwtGenerator.CreateRefreshToken(user);

        return new AuthenticationResult(user, token, refreshToken);
    }

    public async Task<AuthenticationResult> RefreshTokenAsync(string refreshToken)
    {
        var principal = _jwtGenerator.GetPrincipalFromExpiredToken(refreshToken);
        var userId = principal.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            throw new UserNotFoundException("User not found");

        var roles = await _userManager.GetRolesAsync(user);
        var newAccessToken = _jwtGenerator.GenerateToken(user, roles);
        var newRefreshToken = _jwtGenerator.CreateRefreshToken(user);

        return new AuthenticationResult(user, newAccessToken, newRefreshToken);
    }

    public async Task ChangePasswordAsync(ChangePasswordCommand command)
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
            throw new UserNotFoundException();

        if (!int.TryParse(userIdClaim.Value, out int userId))
            throw new UserNotFoundException();

        if (userId == null)
            throw new UnauthorizedAccessException("User not authenticated");

        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            throw new UserNotFoundException("User not found");

        var result = await _userManager.ChangePasswordAsync(user, command.OldPassword, command.NewPassword);
        if (!result.Succeeded)
            throw new UnauthorizedAccessException("Invalid old password.");
    }
}
