using LearningSystem.Application.Persistence;
using LearningSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LearningSystem.Infrastructure.Persistence;
public class UserRepository : IUserRepository
{
    private readonly LearningSystemDbContext _context;

    public UserRepository(LearningSystemDbContext context)
    {
        _context = context;
    }

    public void AddUser(User user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();
    }

    public void DeleteUser(User user)
    {
        _context.Users.Remove(user);
        _context.SaveChanges();
    }
    public User? GetUserByEmail(string email)
    {
        return _context.Users
                       .Include(u => u.Role)
                       .FirstOrDefault(u => u.Email == email);
    }

    public User? GetUserById(int id)
    {
        return _context.Users
                       .Include(u => u.Role)
                       .FirstOrDefault(u => u.Id == id);
    }

    public ICollection<User> GetUsers()
    {
        return _context.Users
                       .Include(u => u.Role)
                       .ToList();
    }

    public void UpdateUser(User user)
    {
        _context.Users.Update(user);
        _context.SaveChanges();
    }
}