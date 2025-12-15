using System.ComponentModel.DataAnnotations;

namespace LearningSystem.Api.Dtos.Categories;

public class CreateCategoryDto
{
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = null!;
}