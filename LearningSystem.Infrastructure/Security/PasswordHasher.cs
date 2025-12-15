using LearningSystem.Application.Common.Security;
using Microsoft.AspNetCore.Identity;

namespace LearningSystem.Infrastructure.Security;

public class PasswordHasher : IPasswordHasher
{
    private readonly PasswordHasher<object> _hasher;

    public PasswordHasher(PasswordHasher<object> hasher)
    {
        _hasher = hasher;
    }

    public string HashPassword(string password)
    {
        return _hasher.HashPassword(null!, password);
    }

    public bool VerifyPassword(string hashedPassword, string providedPassword)
    {
        var result = _hasher.VerifyHashedPassword(null!, hashedPassword, providedPassword);
        return result == PasswordVerificationResult.Success;
    }
}
