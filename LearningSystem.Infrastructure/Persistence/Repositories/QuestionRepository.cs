using LearningSystem.Application.Persistence;
using LearningSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LearningSystem.Infrastructure.Persistence.Repositories;

public class QuestionRepository : IQuestionRepository
{
    private readonly LearningSystemDbContext _context;

    public QuestionRepository(LearningSystemDbContext context)
    {
        _context = context;
    }

    public async Task AddQuestionAsync(Question question)
    {
        await _context.Questions.AddAsync(question);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveQuestionAsync(Question question)
    {
        _context.Questions.Remove(question);
        await _context.SaveChangesAsync();
    }

    public async Task<ICollection<Question>> GetAllQuestionsAsync()
    {
        return await _context.Questions
                         .Include(q => q.QuestionType)
                         .Include(q => q.Answers)
                         .ToListAsync();
    }

    public async Task<Question?> GetQuestionByIdAsync(int id)
    {
        return await _context.Questions
                         .Include(q => q.QuestionType)
                         .Include(q => q.Answers)
                         .FirstOrDefaultAsync(q => q.Id == id);
    }

    public async Task UpdateQuestionAsync(Question question)
    {
        _context.Questions.Update(question);
        await _context.SaveChangesAsync();
    }

    public async Task<ICollection<Question>> GetQuestionsByQuizIdAsync(int quizId)
    {
        return await _context.Questions
                         .Include(q => q.QuestionType)
                         .Include(q => q.Answers)
                         .Where(q => q.QuizId == quizId)
                         .OrderBy(q => q.Order)
                         .ToListAsync();
    }

    public async Task<bool> IsQuestionOrderExistsInQuizAsync(int quizId, int order)
    {
        return await _context.Questions.AnyAsync(q => q.QuizId == quizId && q.Order == order);
    }
}