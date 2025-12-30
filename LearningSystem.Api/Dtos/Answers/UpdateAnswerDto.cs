using System.ComponentModel.DataAnnotations;

namespace LearningSystem.Api.Dtos.Answers;

public class UpdateAnswerDto
{
    [Required]
    [MaxLength(500)]
    public string AnswerText { get; set; } = null!;

    [Required]
    public bool IsCorrect { get; set; }

    public int Id { get; set; }
}
