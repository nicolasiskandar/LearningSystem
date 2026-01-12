using LearningSystem.Application.Commands.Lessons;
using LearningSystem.Application.Results.Lessons;
using System.Security.Claims;

namespace LearningSystem.Application.Services.Lessons;

public interface ILessonService
{
    Task<IEnumerable<LessonResult>> GetAllLessonsAsync();
    Task<LessonResult> GetLessonByIdAsync(int id);
    Task<IEnumerable<LessonResult>> GetLessonByCourseIdAsync(int courseId);
    Task<LessonResult> CreateLessonAsync(CreateLessonCommand command, ClaimsPrincipal user);
    Task<LessonResult> UpdateLessonAsync(UpdateLessonCommand command, ClaimsPrincipal user);
    Task DeleteLessonAsync(int id, ClaimsPrincipal user);
    Task MarkLessonAsCompletedAsync(int lessonId, ClaimsPrincipal user);
    Task<bool> IsLessonCompletedAsync(int lessonId, int userId);
}