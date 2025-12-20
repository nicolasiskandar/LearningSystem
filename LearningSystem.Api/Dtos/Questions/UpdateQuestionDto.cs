using LearningSystem.Api.Dtos.Answers;
using System.ComponentModel.DataAnnotations;

namespace LearningSystem.Api.Dtos.Questions;

public class UpdateQuestionDto
{
    [Required]
    [MaxLength(1000)]
    public string QuestionText { get; set; } = null!;
    
    [Required]
    public int QuestionTypeId { get; set; }
    
    [Required]
    public int Order { get; set; }
    
    public List<UpdateAnswerDto> Answers { get; set; } = new();
}