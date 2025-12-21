using LearningSystem.Application.Commands.QuizAttempts;
using LearningSystem.Application.Results.QuizAttempts;
using System.Security.Claims;

namespace LearningSystem.Application.Services.QuizAttempts;

public interface IQuizAttemptService
{
    Task<QuizAttemptResult> CreateQuizAttemptAsync(CreateQuizAttemptCommand command, ClaimsPrincipal user);
    Task<QuizAttemptResult> GetQuizAttemptByIdAsync(int id);
    Task<IEnumerable<QuizAttemptResult>> GetQuizAttemptByUserIdAsync(int userId);
    Task<QuizAttemptResult> SubmitQuizAsync(SubmitQuizAttemptCommand command, ClaimsPrincipal user);
}
