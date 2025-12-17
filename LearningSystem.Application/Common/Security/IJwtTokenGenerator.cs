using LearningSystem.Domain.Entities;

namespace LearningSystem.Application.Common.Security;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user, IEnumerable<string> roles);
}
