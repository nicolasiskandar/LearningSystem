using LearningSystem.Domain.Entities;

namespace LearningSystem.Application.Authentication.Common;

public class AuthenticationResult
{
    public User User { get; set; } = null!;
    public string Token { get; set; } = null!;

    public AuthenticationResult(User user, string token)
    {
        User = user;
        Token = token;
    }
}
