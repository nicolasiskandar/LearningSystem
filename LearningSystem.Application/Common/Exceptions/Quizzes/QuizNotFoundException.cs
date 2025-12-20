namespace LearningSystem.Application.Common.Exceptions.Quizzes;

public class QuizNotFoundException : NotFoundException
{
    public QuizNotFoundException(int quizId) : base($"Quiz with ID {quizId} not found.")
    {
    }
}