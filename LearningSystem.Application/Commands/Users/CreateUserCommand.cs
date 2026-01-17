namespace LearningSystem.Application.Commands.Users;

public class CreateUserCommand
{
    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;
    public string? RoleName { get; set; }

    public CreateUserCommand(string fullName, string email, string password, string? roleName = null)
    {
        FullName = fullName;
        Email = email;
        Password = password;
        RoleName = roleName;
    }
}
