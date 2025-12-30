using LearningSystem.Application.Results.QuizAttemptAnswers;
using LearningSystem.Application.Results.QuizAttempts;
using LearningSystem.Domain.Entities;

namespace LearningSystem.Application.Mappers.QuizAttempts;

public class QuizAttemptMapper : IQuizAttemptMapper
{
    public QuizAttemptResult Map(QuizAttempt quizAttempt)
    {
        return new QuizAttemptResult
        {
            Id = quizAttempt.Id,
            QuizId = quizAttempt.QuizId,
            UserId = quizAttempt.UserId,
            Score = quizAttempt.Score,
            AttemptDate = quizAttempt.AttemptDate ?? DateTime.UtcNow,
            Answers = quizAttempt.QuizAttemptAnswers.Select(qaa => new QuizAttemptAnswerResult
            {
                QuestionId = qaa.QuestionId,
                AnswerId = qaa.AnswerId
            }).ToList()
        };
    }

    public IEnumerable<QuizAttemptResult> Map(ICollection<QuizAttempt> quizAttempts)
    {
        return quizAttempts.Select(Map);
    }
}
