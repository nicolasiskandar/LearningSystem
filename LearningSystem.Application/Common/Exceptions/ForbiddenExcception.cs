namespace LearningSystem.Application.Common.Exceptions;

public class ForbiddenExcception : Exception
{
    public ForbiddenExcception(string message) : base(message)
    {
    }
}
