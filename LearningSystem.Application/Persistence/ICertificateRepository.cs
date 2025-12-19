using LearningSystem.Domain.Entities;

namespace LearningSystem.Application.Persistence;

public interface ICertificateRepository
{
    Task<Certificate?> GetByIdAsync(int id);
    Task<ICollection<Certificate>> GetAllAsync();
    Task<ICollection<Certificate>> GetByUserIdAsync(int userId);
    Task<Certificate?> GetByUserAndCourse(int userId, int courseId);
    Task AddAsync(Certificate certificate);
    Task UpdateAsync(Certificate certificate);
    Task RemoveAsync(Certificate certificate);
}
