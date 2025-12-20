namespace LearningSystem.Application.Common.Exceptions.Questions;

public class QuestionNotFoundException : NotFoundException
{
    public QuestionNotFoundException(int questionId) : base($"Question with ID {questionId} not found.")
    {
    }
}