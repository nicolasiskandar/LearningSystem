using LearningSystem.Application.Persistence;
using LearningSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LearningSystem.Infrastructure.Persistence.Repositories;

public class QuestionTypeRepository : IQuestionTypeRepository
{
    private readonly LearningSystemDbContext _context;

    public QuestionTypeRepository(LearningSystemDbContext context)
    {
        _context = context;
    }

    public async Task AddQuestionTypeAsync(QuestionType questionType)
    {
        await _context.QuestionTypes.AddAsync(questionType);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveQuestionTypeAsync(QuestionType questionType)
    {
        _context.QuestionTypes.Remove(questionType);
        await _context.SaveChangesAsync();
    }

    public async Task<ICollection<QuestionType>> GetAllQuestionTypesAsync()
    {
        return await _context.QuestionTypes.ToListAsync();
    }

    public async Task<QuestionType?> GetQuestionTypeByIdAsync(int id)
    {
        return await _context.QuestionTypes.FirstOrDefaultAsync(qt => qt.Id == id);
    }

    public async Task UpdateQuestionTypeAsync(QuestionType questionType)
    {
        _context.QuestionTypes.Update(questionType);
        await _context.SaveChangesAsync();
    }
}