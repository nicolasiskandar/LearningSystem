using LearningSystem.Application.Commands.Quizzes;
using LearningSystem.Application.Results.Quizzes;

namespace LearningSystem.Application.Services.Quizzes;

public interface IQuizService
{
    Task<QuizResult> GetQuizByIdAsync(int id);
    Task<IEnumerable<QuizResult>> GetQuizzesAsync();
    Task<QuizResult> AddQuizAsync(CreateQuizCommand command, System.Security.Claims.ClaimsPrincipal claimsPrincipal);
    Task<QuizResult> UpdateQuizAsync(UpdateQuizCommand command, System.Security.Claims.ClaimsPrincipal claimsPrincipal);
    Task DeleteQuizAsync(int id, System.Security.Claims.ClaimsPrincipal claimsPrincipal);
}