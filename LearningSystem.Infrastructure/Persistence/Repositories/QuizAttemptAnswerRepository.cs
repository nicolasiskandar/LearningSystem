using LearningSystem.Application.Persistence;
using LearningSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LearningSystem.Infrastructure.Persistence.Repositories;

public class QuizAttemptAnswerRepository : IQuizAttemptAnswerRepository
{
    private readonly LearningSystemDbContext _context;

    public QuizAttemptAnswerRepository(LearningSystemDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(QuizAttemptAnswer quizAttemptAnswer)
    {
        await _context.QuizAttemptAnswers.AddAsync(quizAttemptAnswer);
        await _context.SaveChangesAsync();
    }

    public async Task<ICollection<QuizAttemptAnswer>> GetAllAsync()
    {
        return await _context.QuizAttemptAnswers.ToListAsync();
    }

    public async Task<QuizAttemptAnswer?> GetByIdAsync(int id)
    {
        return await _context.QuizAttemptAnswers.FindAsync(id);
    }

    public async Task<ICollection<QuizAttemptAnswer>> GetByQuizAttemptIdAsync(int quizAttemptId)
    {
        return await _context.QuizAttemptAnswers.Where(qaa => qaa.QuizAttemptId == quizAttemptId).ToListAsync();
    }

    public async Task RemoveAsync(QuizAttemptAnswer quizAttemptAnswer)
    {
        _context.QuizAttemptAnswers.Remove(quizAttemptAnswer);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(QuizAttemptAnswer quizAttemptAnswer)
    {
        _context.QuizAttemptAnswers.Update(quizAttemptAnswer);
        await _context.SaveChangesAsync();
    }
}
