using LearningSystem.Api.Authentication;
using LearningSystem.Application.Authentication.Commands;
using LearningSystem.Application.Authentication.Common;
using LearningSystem.Application.Authentication.Queries;

namespace LearningSystem.Api.Mappers.Authentication;

public interface IAuthMapper
{
    AuthenticationResponse Map(AuthenticationResult result);
    RegisterUserCommand Map(RegisterRequest command);
    LoginQuery Map(LoginRequest query);
}
