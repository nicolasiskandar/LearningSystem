using LearningSystem.Application.Persistence;
using LearningSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LearningSystem.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly LearningSystemDbContext _context;

    public UserRepository(LearningSystemDbContext context)
    {
        _context = context;
    }

    public async Task AddUserAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteUserAsync(User user)
    {
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<ICollection<User>> GetUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User?> GetUserByIdWithCoursesAsync(int id)
    {
        return await _context.Users
            .Include(u => u.Courses)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User?> GetUserByIdWithEnrolledCoursesAsync(int id)
    {
        return await _context.Users
            .Include(u => u.UserCourses)
                .ThenInclude(uc => uc.Course)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task UpdateUserAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }
}
