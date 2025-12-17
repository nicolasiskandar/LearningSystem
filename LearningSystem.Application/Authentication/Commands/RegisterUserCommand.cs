namespace LearningSystem.Application.Authentication.Commands;

public class RegisterUserCommand
{
    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public RegisterUserCommand(string fullName, string email, string password)
    {
        FullName = fullName;
        Email = email;
        Password = password;
    }
}
