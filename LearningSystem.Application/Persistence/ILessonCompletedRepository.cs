using LearningSystem.Domain.Entities;

namespace LearningSystem.Application.Persistence;

public interface ILessonCompletedRepository
{
    Task<LessonCompleted?> GetByUserAndLessonAsync(int userId, int lessonId);
    Task AddAsync(LessonCompleted lessonCompleted);
    Task<IEnumerable<LessonCompleted>> GetCompletedLessonsForCourseAsync(int userId, int courseId);
}
