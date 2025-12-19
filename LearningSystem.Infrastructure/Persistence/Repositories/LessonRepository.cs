using LearningSystem.Application.Persistence;
using LearningSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LearningSystem.Infrastructure.Persistence.Repositories;

public class LessonRepository : ILessonRepository
{
    private readonly LearningSystemDbContext _context;

    public LessonRepository(LearningSystemDbContext context)
    {
        _context = context;
    }

    public async Task AddLessonAsync(Lesson lesson)
    {
        await _context.Lessons.AddAsync(lesson);
        await _context.SaveChangesAsync();
    }

    public async Task<ICollection<Lesson>> GetAllLessonsAsync()
    {
        return await _context.Lessons.ToListAsync();
    }

    public async Task<Lesson?> GetLessonByIdAsync(int id)
    {
        return await _context.Lessons.FirstOrDefaultAsync(l => l.Id == id);
    }

    public async Task<ICollection<Lesson>> GetLessonsByCourseIdAsync(int courseId)
    {
        return await _context.Lessons
            .Where(l => l.CourseId == courseId)
            .ToListAsync();
    }

    public async Task RemoveLessonAsync(Lesson lesson)
    {
        _context.Lessons.Remove(lesson);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateLessonAsync(Lesson lesson)
    {
        _context.Lessons.Update(lesson);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> LessonOrderExistsAsync(int courseId, int order)
    {
        return await _context.Lessons.AnyAsync(l => l.CourseId == courseId && l.Order == order);
    }
}
