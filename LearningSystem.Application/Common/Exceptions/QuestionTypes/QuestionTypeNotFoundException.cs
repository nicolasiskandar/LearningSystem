namespace LearningSystem.Application.Common.Exceptions.QuestionTypes;

public class QuestionTypeNotFoundException : NotFoundException
{
    public QuestionTypeNotFoundException(int questionTypeId) : base($"Question type with ID {questionTypeId} not found.")
    {
    }
}