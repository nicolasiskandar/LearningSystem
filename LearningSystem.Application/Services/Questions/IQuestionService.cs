using LearningSystem.Application.Commands.Questions;
using LearningSystem.Application.Results.Questions;
using System.Security.Claims;

namespace LearningSystem.Application.Services.Questions;

public interface IQuestionService
{
    Task<QuestionResult> GetQuestionByIdAsync(int id);
    Task<IEnumerable<QuestionResult>> GetQuestionsAsync();
    Task<IEnumerable<QuestionResult>> GetQuestionsByQuizIdAsync(int quizId);
    Task<QuestionResult> AddQuestionAsync(CreateQuestionCommand command, ClaimsPrincipal claimsPrincipal);
    Task<QuestionResult> UpdateQuestionAsync(UpdateQuestionCommand command, ClaimsPrincipal claimsPrincipal);
    Task DeleteQuestionAsync(int id, ClaimsPrincipal claimsPrincipal);
}