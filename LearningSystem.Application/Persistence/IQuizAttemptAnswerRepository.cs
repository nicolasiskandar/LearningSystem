using LearningSystem.Domain.Entities;

namespace LearningSystem.Application.Persistence;

public interface IQuizAttemptAnswerRepository
{
    Task<QuizAttemptAnswer?> GetByIdAsync(int id);
    Task<ICollection<QuizAttemptAnswer>> GetAllAsync();
    Task AddAsync(QuizAttemptAnswer quizAttemptAnswer);
    Task UpdateAsync(QuizAttemptAnswer quizAttemptAnswer);
    Task RemoveAsync(QuizAttemptAnswer quizAttemptAnswer);
    Task<ICollection<QuizAttemptAnswer>> GetByQuizAttemptIdAsync(int quizAttemptId);
}
