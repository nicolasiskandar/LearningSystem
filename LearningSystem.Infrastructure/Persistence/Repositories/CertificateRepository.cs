using LearningSystem.Application.Persistence;
using LearningSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LearningSystem.Infrastructure.Persistence.Repositories;

public class CertificateRepository : ICertificateRepository
{
    private readonly LearningSystemDbContext _context;

    public CertificateRepository(LearningSystemDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Certificate certificate)
    {
        await _context.Certificates.AddAsync(certificate);
        await _context.SaveChangesAsync();
    }

    public async Task<ICollection<Certificate>> GetAllAsync()
    {
        return await _context.Certificates.ToListAsync();
    }

    public async Task<Certificate?> GetByIdAsync(int id)
    {
        return await _context.Certificates.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<ICollection<Certificate>> GetByUserIdAsync(int userId)
    {
        return await _context.Certificates
            .Where(c => c.UserId == userId)
            .ToListAsync();
    }

    public async Task<Certificate?> GetByUserAndCourse(int userId, int courseId)
    {
        return await _context.Certificates
            .FirstOrDefaultAsync(c => c.UserId == userId && c.CourseId == courseId);
    }

    public async Task RemoveAsync(Certificate certificate)
    {
        _context.Certificates.Remove(certificate);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Certificate certificate)
    {
        _context.Certificates.Update(certificate);
        await _context.SaveChangesAsync();
    }
}
