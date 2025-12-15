using LearningSystem.Application.Commands.Courses;
using LearningSystem.Application.Results.Courses;
using LearningSystem.Domain.Entities;

namespace LearningSystem.Application.Mappers.Courses;

public interface ICourseMapper
{
    CourseResult Map(Course course);
    IEnumerable<CourseResult> Map(IEnumerable<Course> courses);
    Course Map(CreateCourseCommand command);
    void Map(UpdateCourseCommand command, Course course);
}
