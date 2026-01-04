using LearningSystem.Application.Commands.Courses;
using LearningSystem.Application.Results.Courses;
using System.Security.Claims;

namespace LearningSystem.Application.Services.Courses;

public interface ICourseService
{
    Task<CourseResult> GetCourseByIdAsync(int id);
    Task<IEnumerable<CourseResult>> GetCoursesAsync(int page, int pageSize);
    Task<CourseResult> AddCourseAsync(CreateCourseCommand command, ClaimsPrincipal claimsPrincipal);
    Task<CourseResult> UpdateCourseAsync(UpdateCourseCommand command, ClaimsPrincipal claimsPrincipal);
    Task DeleteCourseAsync(int id, ClaimsPrincipal claimsPrincipal);
    Task EnrollUserInCourse(int userId, int courseId);
}
