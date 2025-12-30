using LearningSystem.Domain.Entities;

namespace LearningSystem.Application.Persistence;

public interface ILessonRepository
{
    Task<Lesson?> GetLessonByIdAsync(int id);
    Task<ICollection<Lesson>> GetAllLessonsAsync();
    Task<ICollection<Lesson>> GetLessonsByCourseIdAsync(int courseId);
    Task AddLessonAsync(Lesson lesson);
    Task UpdateLessonAsync(Lesson lesson);
    Task RemoveLessonAsync(Lesson lesson);
    Task<bool> LessonOrderExistsAsync(int courseId, int order);
}
