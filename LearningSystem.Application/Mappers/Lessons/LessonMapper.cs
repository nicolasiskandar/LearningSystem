using LearningSystem.Application.Commands.Lessons;
using LearningSystem.Application.Results.Lessons;
using LearningSystem.Domain.Entities;

namespace LearningSystem.Application.Mappers.Lessons;

public class LessonMapper : ILessonMapper
{
    public LessonResult Map(Lesson lesson)
    {
        return new LessonResult
        {
            Id = lesson.Id,
            CourseId = lesson.CourseId,
            Title = lesson.Title,
            Content = lesson.Content,
            VideoUrl = lesson.VideoUrl,
            Order = lesson.Order,
            EstimatedDuration = lesson.EstimatedDuration
        };
    }

    public IEnumerable<LessonResult> Map(IEnumerable<Lesson> lessons)
    {
        return lessons.Select(Map);
    }

    public Lesson Map(CreateLessonCommand command)
    {
        return new Lesson
        {
            CourseId = command.CourseId,
            Title = command.Title,
            Content = command.Content,
            VideoUrl = command.VideoUrl,
            Order = command.Order,
            EstimatedDuration = command.EstimatedDuration,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void Map(UpdateLessonCommand command, Lesson lesson)
    {
        lesson.CourseId = command.CourseId;
        lesson.Title = command.Title;
        lesson.Content = command.Content;
        lesson.VideoUrl = command.VideoUrl;
        lesson.Order = command.Order;
        lesson.EstimatedDuration = command.EstimatedDuration;
    }
}
