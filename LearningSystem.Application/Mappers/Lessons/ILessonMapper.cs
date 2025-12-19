using LearningSystem.Application.Commands.Lessons;
using LearningSystem.Application.Results.Lessons;
using LearningSystem.Domain.Entities;

namespace LearningSystem.Application.Mappers.Lessons;

public interface ILessonMapper
{
    LessonResult Map(Lesson lesson);
    IEnumerable<LessonResult> Map(IEnumerable<Lesson> lessons);
    Lesson Map(CreateLessonCommand command);
    void Map(UpdateLessonCommand command, Lesson lesson);
}
