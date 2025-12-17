namespace LearningSystem.Api.Authentication;

public class AuthenticationResponse
{
    public int Id { get; set; }
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Token { get; set; } = null!;

    public AuthenticationResponse(int id, string fullName, string email, string token)
    {
        Id = id;
        FullName = fullName;
        Email = email;
        Token = token;
    }
}
