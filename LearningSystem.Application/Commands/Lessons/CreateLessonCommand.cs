namespace LearningSystem.Application.Commands.Lessons;

public class CreateLessonCommand
{
    public int CourseId { get; set; }
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public string? VideoUrl { get; set; }
    public int Order { get; set; }
    public int EstimatedDuration { get; set; }
}
