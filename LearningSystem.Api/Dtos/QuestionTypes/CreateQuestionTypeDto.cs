using System.ComponentModel.DataAnnotations;

namespace LearningSystem.Api.Dtos.QuestionTypes;

public class CreateQuestionTypeDto
{
    [Required]
    public string Name { get; set; }
}
