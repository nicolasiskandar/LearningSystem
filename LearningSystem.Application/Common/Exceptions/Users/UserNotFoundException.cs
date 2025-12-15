namespace LearningSystem.Application.Common.Exceptions.Users;

public class UserNotFoundException : NotFoundException
{
    public UserNotFoundException(string message) : base(message)
    {
    }

    public UserNotFoundException(int Id) : base($"User with ID {Id} does not exist.")
    {
    }
}
