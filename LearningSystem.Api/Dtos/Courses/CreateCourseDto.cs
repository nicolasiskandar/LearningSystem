using System.ComponentModel.DataAnnotations;

namespace LearningSystem.Api.Dtos.Courses;

public class CreateCourseDto
{
    [Required]
    [MaxLength(255)]
    public string Title { get; set; } = null!;
    [Required]
    [MaxLength(255)]
    public string ShortDescription { get; set; } = null!;
    [Required]
    public string LongDescription { get; set; } = null!;
    [Required]
    public int CategoryId { get; set; }
    [Required]
    public string Difficulty { get; set; } = null!;
    [Required]
    public int CreatedBy { get; set; }
    [Required]
    public string Thumbnail { get; set; } = null!;
    public bool IsPublished { get; set; }
}
