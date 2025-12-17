namespace LearningSystem.Application.Common.Exceptions;

public abstract class FailedException : Exception
{
    public FailedException(string message) : base(message)
    {
    }
}
