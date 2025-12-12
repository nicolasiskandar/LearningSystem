namespace LearningSystem.Application.Exceptions;
public class UserNotFoundException : Exception
{
    public UserNotFoundException(string message)
        : base(message)
    {
    }
}
