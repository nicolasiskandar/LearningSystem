using LearningSystem.Domain.Entities;

namespace LearningSystem.Application.Persistence;

public interface IQuizAttemptRepository
{
    Task<QuizAttempt?> GetQuizAttemptByIdAsync(int id);
    Task<ICollection<QuizAttempt>> GetAllQuizAttemptsAsync();
    Task AddQuizAttemptAsync(QuizAttempt quizAttempt);
    Task UpdateQuizAttemptAsync(QuizAttempt quizAttempt);
    Task RemoveQuizAttemptAsync(QuizAttempt quizAttempt);
    Task<ICollection<QuizAttempt>> GetQuizAttemptsByUserIdAsync(int userId);
}
