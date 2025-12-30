using System.ComponentModel.DataAnnotations;

namespace LearningSystem.Api.Dtos.Answers;

public class CreateAnswerDto
{
    [Required]
    [MaxLength(500)]
    public string AnswerText { get; set; } = null!;

    [Required]
    public bool IsCorrect { get; set; }
}
