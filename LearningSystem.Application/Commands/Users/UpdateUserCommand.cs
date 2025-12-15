namespace LearningSystem.Application.Commands.Users;

public class UpdateUserCommand
{
    public int Id { get; set; }
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;

    public UpdateUserCommand(int id, string fullName, string email)
    {
        Id = id;
        FullName = fullName;
        Email = email;
    }
}
