using LearningSystem.Application.Persistence;
using LearningSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LearningSystem.Infrastructure.Persistence.Repositories;

public class LessonCompletedRepository : ILessonCompletedRepository
{
    private readonly LearningSystemDbContext _context;

    public LessonCompletedRepository(LearningSystemDbContext context)
    {
        _context = context;
    }

    public async Task<LessonCompleted?> GetByUserAndLessonAsync(int userId, int lessonId)
    {
        return await _context.LessonCompleteds
            .FirstOrDefaultAsync(lc => lc.UserId == userId && lc.LessonId == lessonId);
    }

    public async Task AddAsync(LessonCompleted lessonCompleted)
    {
        await _context.LessonCompleteds.AddAsync(lessonCompleted);
        await _context.SaveChangesAsync();
    }
    
    public async Task<IEnumerable<LessonCompleted>> GetCompletedLessonsForCourseAsync(int userId, int courseId)
    {
        return await _context.LessonCompleteds
            .Include(lc => lc.Lesson)
            .Where(lc => lc.UserId == userId && lc.Lesson.CourseId == courseId)
            .ToListAsync();
    }
}