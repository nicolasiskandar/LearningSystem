using LearningSystem.Application.Persistence;
using LearningSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LearningSystem.Infrastructure.Persistence.Repositories;

public class CourseRepository : ICourseRepository
{
    private readonly LearningSystemDbContext _context;

    public CourseRepository(LearningSystemDbContext context)
    {
        _context = context;
    }

    public async Task AddCourseAsync(Course course)
    {
        await _context.Courses.AddAsync(course);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveCourseAsync(Course course)
    {
        _context.Courses.Remove(course);
        await _context.SaveChangesAsync();
    }

    public async Task<ICollection<Course>> GetAllCoursesAsync(int page, int pageSize)
    {
        return await _context.Courses
                         .Include(c => c.Category)
                         .Skip((page - 1) * pageSize)
                         .Take(pageSize)
                         .ToListAsync();
    }

    public async Task<ICollection<Course>> GetCoursesByCategoryIdAsync(int categoryId, int page, int pageSize)
    {
        return await _context.Courses
                         .Include(c => c.Category)
                         .Where(c => c.CategoryId == categoryId)
                         .Skip((page - 1) * pageSize)
                         .Take(pageSize)
                         .ToListAsync();
    }

    public async Task<Course?> GetCourseByIdAsync(int id)
    {
        return await _context.Courses
                         .Include(c => c.Category)
                         .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task UpdateCourseAsync(Course course)
    {
        _context.Courses.Update(course);
        await _context.SaveChangesAsync();
    }

    public async Task<ICollection<Course>> GetCoursesByUserIdAsync(int userId)
    {
        return await _context.Courses
                             .Include(c => c.Category)
                             .Where(c => c.CreatedBy == userId)
                             .ToListAsync();

    }

    public async Task<IEnumerable<Course>> GetCoursesEnrolledByUserAsync(int userId)
    {
        return await _context.UserCourses
                             .Where(uc => uc.UserId == userId)
                             .Include(uc => uc.Course)
                                 .ThenInclude(c => c.Category)
                             .Select(uc => uc.Course)
                             .ToListAsync();
    }

    public async Task<IEnumerable<Lesson>> GetLessonsByCourseIdAsync(int courseId)
    {
        return await _context.Lessons
                             .Where(l => l.CourseId == courseId)
                             .ToListAsync();
    }
}
