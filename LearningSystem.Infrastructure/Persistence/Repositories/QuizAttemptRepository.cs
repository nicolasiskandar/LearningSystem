using LearningSystem.Application.Persistence;
using LearningSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LearningSystem.Infrastructure.Persistence.Repositories;

public class QuizAttemptRepository : IQuizAttemptRepository
{
    private readonly LearningSystemDbContext _context;

    public QuizAttemptRepository(LearningSystemDbContext context)
    {
        _context = context;
    }

    public async Task AddQuizAttemptAsync(QuizAttempt quizAttempt)
    {
        await _context.QuizAttempts.AddAsync(quizAttempt);
        await _context.SaveChangesAsync();
    }

    public async Task<ICollection<QuizAttempt>> GetAllQuizAttemptsAsync()
    {
        return await _context.QuizAttempts
            .Include(qa => qa.QuizAttemptAnswers)
            .ToListAsync();
    }

    public async Task<QuizAttempt?> GetQuizAttemptByIdAsync(int id)
    {
        return await _context.QuizAttempts
            .Include(qa => qa.QuizAttemptAnswers)
            .FirstOrDefaultAsync(qa => qa.Id == id);
    }

    public async Task<ICollection<QuizAttempt>> GetQuizAttemptsByUserIdAsync(int userId)
    {
        return await _context.QuizAttempts
            .Where(qa => qa.UserId == userId)
            .Include(qa => qa.QuizAttemptAnswers)
            .ToListAsync();
    }

    public async Task RemoveQuizAttemptAsync(QuizAttempt quizAttempt)
    {
        _context.QuizAttempts.Remove(quizAttempt);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateQuizAttemptAsync(QuizAttempt quizAttempt)
    {
        _context.QuizAttempts.Update(quizAttempt);
        await _context.SaveChangesAsync();
    }
}
