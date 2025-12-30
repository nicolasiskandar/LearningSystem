using LearningSystem.Domain.Entities;
using System.Security.Claims;

namespace LearningSystem.Application.Common.Security;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user, IEnumerable<string> roles);
    string CreateRefreshToken(User user);
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}
