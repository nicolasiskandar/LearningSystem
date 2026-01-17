namespace LearningSystem.Application.Commands.Users;

public class UpdateUserCommand
{
    public int Id { get; set; }
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? RoleName { get; set; }

    public UpdateUserCommand(int id, string fullName, string email, string? roleName = null)
    {
        Id = id;
        FullName = fullName;
        Email = email;
        RoleName = roleName;
    }
}
