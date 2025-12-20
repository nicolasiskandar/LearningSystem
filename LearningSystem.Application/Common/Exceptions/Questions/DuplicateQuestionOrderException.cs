using LearningSystem.Application.Common.Exceptions;

namespace LearningSystem.Application.Common.Exceptions.Questions;

public class DuplicateQuestionOrderException : FailedException
{
    public DuplicateQuestionOrderException(int quizId, int order)
        : base($"A question with order {order} already exists for quiz {quizId}")
    {
    }
}
