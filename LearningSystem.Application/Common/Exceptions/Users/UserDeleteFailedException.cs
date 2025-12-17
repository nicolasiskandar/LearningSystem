namespace LearningSystem.Application.Common.Exceptions.Users;

public class UserDeleteFailedException : FailedException
{
    public UserDeleteFailedException() : base("User delete failed.")
    {
    }
}
