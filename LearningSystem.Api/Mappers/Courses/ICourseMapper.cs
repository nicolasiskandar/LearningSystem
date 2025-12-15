using LearningSystem.Api.Dtos.Courses;
using LearningSystem.Application.Commands.Courses;
using LearningSystem.Application.Results.Courses;

namespace LearningSystem.Api.Mappers.Courses;

public interface ICourseMapper
{
    CourseDto Map(CourseResult result);
    IEnumerable<CourseDto> Map(IEnumerable<CourseResult> results);
    CreateCourseCommand Map(CreateCourseDto dto);
    UpdateCourseCommand Map(UpdateCourseDto dto, int id);
}
