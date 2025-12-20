using LearningSystem.Domain.Entities;

namespace LearningSystem.Application.Persistence;

public interface IQuizRepository
{
    Task AddQuizAsync(Quiz quiz);
    Task<ICollection<Quiz>> GetAllQuizzesAsync();
    Task<Quiz?> GetQuizByIdAsync(int id);
    Task RemoveQuizAsync(Quiz quiz);
    Task UpdateQuizAsync(Quiz quiz);
}