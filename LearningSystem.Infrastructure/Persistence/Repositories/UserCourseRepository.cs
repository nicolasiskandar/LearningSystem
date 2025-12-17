using LearningSystem.Application.Persistence;
using LearningSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LearningSystem.Infrastructure.Persistence.Repositories;

public class UserCourseRepository : IUserCourseRepository
{
    private readonly LearningSystemDbContext _context;

    public UserCourseRepository(LearningSystemDbContext context)
    {
        _context = context;
    }

    public async Task<UserCourse?> GetUserCourseAsync(int userId, int courseId)
    {
        return await _context.UserCourses
            .FirstOrDefaultAsync(uc => uc.UserId == userId && uc.CourseId == courseId);
    }

    public async Task AddUserCourseAsync(UserCourse userCourse)
    {
        await _context.UserCourses.AddAsync(userCourse);
        await _context.SaveChangesAsync();
    }
}
