using LearningSystem.Application.Commands.Courses;
using LearningSystem.Application.Results.Courses;

namespace LearningSystem.Application.Services.Courses;

public interface ICourseService
{
    Task<CourseResult> GetCourseByIdAsync(int id);
    Task<IEnumerable<CourseResult>> GetCoursesAsync();
    Task<CourseResult> AddCourseAsync(CreateCourseCommand command);
    Task<CourseResult> UpdateCourseAsync(UpdateCourseCommand command);
    Task DeleteCourseAsync(int id);
}
