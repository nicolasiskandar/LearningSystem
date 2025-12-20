using System.ComponentModel.DataAnnotations;

namespace LearningSystem.Api.Dtos.Quizzes;

public class CreateQuizDto
{
    [Required]
    [MaxLength(255)]
    public string Title { get; set; } = null!;
    
    [Required]
    public int PassingScore { get; set; }
    
    [Required]
    public int TimeLimit { get; set; }
    
    [Required]
    public int CourseId { get; set; }
    
    [Required]
    public int LessonId { get; set; }
}

public class UpdateQuizDto
{
    [Required]
    [MaxLength(255)]
    public string Title { get; set; } = null!;
    
    [Required]
    public int PassingScore { get; set; }
    
    [Required]
    public int TimeLimit { get; set; }
}