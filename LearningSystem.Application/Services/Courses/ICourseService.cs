using LearningSystem.Application.Commands.Courses;
using LearningSystem.Application.Results.Courses;

namespace LearningSystem.Application.Services.Courses;

public interface ICourseService
{
    CourseResult GetCourseById(int id);
    IEnumerable<CourseResult> GetCourses();
    CourseResult AddCourse(CreateCourseCommand command);
    CourseResult UpdateCourse(UpdateCourseCommand command);
    void DeleteCourse(int id);
}
