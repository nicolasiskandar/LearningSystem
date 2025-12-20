using LearningSystem.Application.Persistence;
using LearningSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LearningSystem.Infrastructure.Persistence.Repositories;

public class QuizRepository : IQuizRepository
{
    private readonly LearningSystemDbContext _context;

    public QuizRepository(LearningSystemDbContext context)
    {
        _context = context;
    }

    public async Task AddQuizAsync(Quiz quiz)
    {
        await _context.Quizzes.AddAsync(quiz);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveQuizAsync(Quiz quiz)
    {
        _context.Quizzes.Remove(quiz);
        await _context.SaveChangesAsync();
    }

    public async Task<ICollection<Quiz>> GetAllQuizzesAsync()
    {
        return await _context.Quizzes
            .Include(q => q.Course)
            .Include(q => q.Lesson)
            .Include(q => q.Questions)
                .ThenInclude(q => q.QuestionType)
            .Include(q => q.Questions)
                .ThenInclude(q => q.Answers)
            .ToListAsync();
    }

    public async Task<Quiz?> GetQuizByIdAsync(int id)
    {
        return await _context.Quizzes
            .Include(q => q.Course)
            .Include(q => q.Lesson)
            .Include(q => q.Questions)
                .ThenInclude(q => q.QuestionType)
            .Include(q => q.Questions)
                .ThenInclude(q => q.Answers)
            .FirstOrDefaultAsync(q => q.Id == id);
    }

    public async Task UpdateQuizAsync(Quiz quiz)
    {
        _context.Quizzes.Update(quiz);
        await _context.SaveChangesAsync();
    }
}