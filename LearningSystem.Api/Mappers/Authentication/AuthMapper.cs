using LearningSystem.Api.Authentication;
using LearningSystem.Application.Authentication.Commands;
using LearningSystem.Application.Authentication.Common;
using LearningSystem.Application.Authentication.Queries;

namespace LearningSystem.Api.Mappers.Authentication;

public class AuthMapper : IAuthMapper
{
    public AuthenticationResponse Map(AuthenticationResult result)
    {
        return new AuthenticationResponse(
            result.User.Id,
            result.User.FullName,
            result.User.Email,
            result.Token
        );
    }

    public RegisterUserCommand Map(RegisterRequest request)
    {
        return new RegisterUserCommand(
            request.FirstName + " " + request.LastName,
            request.Email,
            request.Password
        );
    }

    public LoginQuery Map(LoginRequest request)
    {
        return new LoginQuery(
            request.Email,
            request.Password
        );
    }
}
