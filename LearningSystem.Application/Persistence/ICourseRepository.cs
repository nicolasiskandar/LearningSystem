using LearningSystem.Domain.Entities;

namespace LearningSystem.Application.Persistence;

public interface ICourseRepository
{
    Task<Course?> GetCourseByIdAsync(int id);
    Task<ICollection<Course>> GetAllCoursesAsync(int page, int pageSize, string? searchTerm = null);
    Task<ICollection<Course>> GetCoursesByCategoryIdAsync(int categoryId, int page, int pageSize);
    Task AddCourseAsync(Course course);
    Task UpdateCourseAsync(Course course);
    Task RemoveCourseAsync(Course course);
    Task<ICollection<Course>> GetCoursesByUserIdAsync(int userId);
    Task<IEnumerable<Course>> GetCoursesEnrolledByUserAsync(int userId);
    Task<IEnumerable<Lesson>> GetLessonsByCourseIdAsync(int courseId);
}
