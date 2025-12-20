using LearningSystem.Domain.Entities;

namespace LearningSystem.Application.Persistence;

public interface IQuestionRepository
{
    Task<Question?> GetQuestionByIdAsync(int id);
    Task<ICollection<Question>> GetAllQuestionsAsync();
    Task AddQuestionAsync(Question question);
    Task UpdateQuestionAsync(Question question);
    Task RemoveQuestionAsync(Question question);
    Task<ICollection<Question>> GetQuestionsByQuizIdAsync(int quizId);
    Task<bool> IsQuestionOrderExistsInQuizAsync(int quizId, int order);
}