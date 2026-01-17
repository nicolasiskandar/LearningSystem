using System.ComponentModel.DataAnnotations;

namespace LearningSystem.Api.Dtos.Users;

public class UpdateUserDto
{
    [Required(ErrorMessage = "Full name is required.")]
    public string FullName { get; set; } = null!;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string Email { get; set; } = null!;

    public string? RoleName { get; set; }
}
