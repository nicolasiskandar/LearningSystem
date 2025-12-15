namespace LearningSystem.Application.Results.Users;

public class UserResult
{
    public int Id { get; set; }
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string RoleName { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}
