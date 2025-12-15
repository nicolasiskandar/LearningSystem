namespace LearningSystem.Application.Common.Exceptions.Users;

public class UserAlreadyExistsException : AlreadyExistsException
{
    public UserAlreadyExistsException(string message) : base(message)
    {
    }
}
