using LearningSystem.Api.Dtos.Courses;
using LearningSystem.Application.Commands.Courses;
using LearningSystem.Application.Results.Courses;

namespace LearningSystem.Api.Mappers.Courses;

public class CourseMapper : ICourseMapper
{
    public CourseDto Map(CourseResult result)
    {
        return new CourseDto
        {
            Id = result.Id,
            Title = result.Title,
            ShortDescription = result.ShortDescription,
            LongDescription = result.LongDescription,
            Category = result.Category,
            Difficulty = result.Difficulty,
            CreatedBy = result.CreatedBy,
            Thumbnail = result.Thumbnail,
            CreatedAt = result.CreatedAt,
            IsPublished = result.IsPublished
        };
    }

    public IEnumerable<CourseDto> Map(IEnumerable<CourseResult> results)
    {
        return results.Select(Map);
    }

    public CreateCourseCommand Map(CreateCourseDto dto)
    {
        return new CreateCourseCommand(
            dto.Title,
            dto.ShortDescription,
            dto.LongDescription,
            dto.CategoryId,
            dto.Difficulty,
            dto.CreatedBy,
            dto.Thumbnail,
            dto.IsPublished
        );
    }

    public UpdateCourseCommand Map(UpdateCourseDto dto, int id)
    {
        return new UpdateCourseCommand(
            id,
            dto.Title,
            dto.ShortDescription,
            dto.LongDescription,
            dto.CategoryId,
            dto.Difficulty,
            dto.Thumbnail,
            dto.IsPublished
        );
    }
}
