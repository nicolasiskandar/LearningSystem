namespace LearningSystem.Application.Authentication.Queries;

public class LoginQuery
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;

    public LoginQuery(string email, string password)
    {
        Email = email;
        Password = password;
    }
}
