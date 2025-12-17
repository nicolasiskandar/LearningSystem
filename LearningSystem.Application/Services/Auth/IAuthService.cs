using LearningSystem.Application.Authentication.Commands;
using LearningSystem.Application.Authentication.Common;
using LearningSystem.Application.Authentication.Queries;

namespace LearningSystem.Application.Services.Auth;

public interface IAuthService
{
    Task<AuthenticationResult> RegisterAsync(RegisterUserCommand command);
    Task<AuthenticationResult> LoginAsync(LoginQuery query);
    Task<AuthenticationResult> RefreshTokenAsync(string refreshToken);
}
