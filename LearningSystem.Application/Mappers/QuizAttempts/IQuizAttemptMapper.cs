using LearningSystem.Application.Results.QuizAttempts;
using LearningSystem.Domain.Entities;

namespace LearningSystem.Application.Mappers.QuizAttempts;

public interface IQuizAttemptMapper
{
    QuizAttemptResult Map(QuizAttempt quizAttempt);
    IEnumerable<QuizAttemptResult> Map(ICollection<QuizAttempt> quizAttempts);
}
