namespace LearningSystem.Application.Common.Exceptions.Users;

public class UserUpdateFailedException : FailedException
{
    public UserUpdateFailedException() : base("User update failed.")
    {
    }
}
