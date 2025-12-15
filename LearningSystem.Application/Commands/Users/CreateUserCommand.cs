namespace LearningSystem.Application.Commands.Users;

public class CreateUserCommand
{
    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public CreateUserCommand(string fullName, string email, string password)
    {
        FullName = fullName;
        Email = email;
        Password = password;
    }
}
