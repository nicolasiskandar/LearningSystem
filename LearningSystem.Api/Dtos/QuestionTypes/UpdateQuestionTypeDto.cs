using System.ComponentModel.DataAnnotations;

namespace LearningSystem.Api.Dtos.QuestionTypes;

public class UpdateQuestionTypeDto
{
    [Required]
    public string Name { get; set; }
}
