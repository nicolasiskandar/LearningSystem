namespace LearningSystem.Application.Authentication.Commands;

public class ChangePasswordCommand
{
    public string OldPassword { get; set; } = null!;
    public string NewPassword { get; set; } = null!;

    public ChangePasswordCommand(string oldPassword, string newPassword)
    {
        OldPassword = oldPassword;
        NewPassword = newPassword;
    }
}
