namespace LearningSystem.Application.Common.Exceptions;

public abstract class AlreadyExistsException : Exception
{
    public AlreadyExistsException(string message) : base(message)
    {
    }
}
