namespace LearningSystem.Application.Common.Exceptions.QuizAttempt;

public class QuizAttemptNotFoundException : NotFoundException
{
    public QuizAttemptNotFoundException(int id) : base($"Quiz Attempt with ID {id} not found.")
    {
    }
}
