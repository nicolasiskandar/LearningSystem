namespace LearningSystem.Application.Common.Exceptions.Answers;

public class AnswerNotFoundException : NotFoundException
{
    public AnswerNotFoundException(int answerId) : base($"Answer with ID {answerId} not found.")
    {
    }
}