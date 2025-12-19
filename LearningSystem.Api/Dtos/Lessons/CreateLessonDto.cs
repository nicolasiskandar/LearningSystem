using System.ComponentModel.DataAnnotations;

namespace LearningSystem.Api.Dtos.Lessons;

public class CreateLessonDto
{
    [Required]
    public int CourseId { get; set; }
    [Required]
    [MaxLength(100)]
    public string Title { get; set; } = null!;
    [Required]
    public string Content { get; set; } = null!;
    public string? VideoUrl { get; set; }
    [Required]
    public int Order { get; set; }
    [Required]
    public int EstimatedDuration { get; set; }
}
