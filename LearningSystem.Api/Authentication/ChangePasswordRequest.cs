using System.ComponentModel.DataAnnotations;

namespace LearningSystem.Api.Authentication;

public class ChangePasswordRequest
{
    [Required(ErrorMessage = "Old Password is required.")]
    [MinLength(8, ErrorMessage = "Old Password must be at least 8 characters.")]
    [RegularExpression(
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        ErrorMessage = "Old Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character."
    )]
    public string OldPassword { get; set; } = null!;

    [Required(ErrorMessage = "New Password is required.")]
    [MinLength(8, ErrorMessage = "New Password must be at least 8 characters.")]
    [RegularExpression(
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        ErrorMessage = "New Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character."
    )]
    public string NewPassword { get; set; } = null!;

    public ChangePasswordRequest(string oldPassword, string newPassword)
    {
        OldPassword = oldPassword;
        NewPassword = newPassword;
    }
}
