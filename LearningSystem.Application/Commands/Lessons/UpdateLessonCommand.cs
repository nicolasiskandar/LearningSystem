namespace LearningSystem.Application.Commands.Lessons;

public class UpdateLessonCommand
{
    public int Id { get; set; }
    public int CourseId { get; set; }
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public string? VideoUrl { get; set; }
    public int Order { get; set; }
    public int EstimatedDuration { get; set; }
}
