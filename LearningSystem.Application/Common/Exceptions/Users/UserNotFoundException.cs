namespace LearningSystem.Application.Common.Exceptions.Users;

public class UserNotFoundException : NotFoundException
{
    public UserNotFoundException() : base("User not found")
    {
    }

    public UserNotFoundException(string message) : base(message)
    {
    }

    public UserNotFoundException(int Id) : base($"User with ID {Id} does not exist.")
    {
    }
}
