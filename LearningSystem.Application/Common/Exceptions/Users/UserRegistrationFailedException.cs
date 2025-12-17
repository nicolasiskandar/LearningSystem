namespace LearningSystem.Application.Common.Exceptions.Users;

public class UserRegistrationFailedException : FailedException
{
    public UserRegistrationFailedException() : base("User registration failed.")
    {
    }
}
