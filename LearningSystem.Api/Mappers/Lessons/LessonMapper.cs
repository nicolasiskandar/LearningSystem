using LearningSystem.Api.Dtos.Lessons;
using LearningSystem.Application.Commands.Lessons;
using LearningSystem.Application.Results.Lessons;

namespace LearningSystem.Api.Mappers.Lessons;

public class LessonMapper : ILessonMapper
{
    public LessonDto Map(LessonResult result)
    {
        return new LessonDto
        {
            Id = result.Id,
            CourseId = result.CourseId,
            Title = result.Title,
            Content = result.Content,
            VideoUrl = result.VideoUrl,
            Order = result.Order,
            EstimatedDuration = result.EstimatedDuration
        };
    }

    public IEnumerable<LessonDto> Map(IEnumerable<LessonResult> results)
    {
        return results.Select(Map);
    }

    public CreateLessonCommand Map(CreateLessonDto dto)
    {
        return new CreateLessonCommand
        {
            CourseId = dto.CourseId,
            Title = dto.Title,
            Content = dto.Content,
            VideoUrl = dto.VideoUrl,
            Order = dto.Order,
            EstimatedDuration = dto.EstimatedDuration
        };
    }

    public UpdateLessonCommand Map(UpdateLessonDto dto, int id)
    {
        return new UpdateLessonCommand
        {
            Id = id,
            CourseId = dto.CourseId,
            Title = dto.Title,
            Content = dto.Content,
            VideoUrl = dto.VideoUrl,
            Order = dto.Order,
            EstimatedDuration = dto.EstimatedDuration
        };
    }
}
