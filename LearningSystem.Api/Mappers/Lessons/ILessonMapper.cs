using LearningSystem.Api.Dtos.Lessons;
using LearningSystem.Application.Commands.Lessons;
using LearningSystem.Application.Results.Lessons;

namespace LearningSystem.Api.Mappers.Lessons;

public interface ILessonMapper
{
    LessonDto Map(LessonResult result);
    IEnumerable<LessonDto> Map(IEnumerable<LessonResult> results);
    CreateLessonCommand Map(CreateLessonDto dto);
    UpdateLessonCommand Map(UpdateLessonDto dto, int id);
}
