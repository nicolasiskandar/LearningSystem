using LearningSystem.Api.Dtos.QuizAttempts;
using LearningSystem.Application.Commands.QuizAttempts;
using LearningSystem.Application.Results.QuizAttempts;

namespace LearningSystem.Api.Mappers.QuizAttempts;

public class QuizAttemptMapper : IQuizAttemptMapper
{
    public QuizAttemptDto Map(QuizAttemptResult result)
    {
        return new QuizAttemptDto
        {
            Id = result.Id,
            QuizId = result.QuizId,
            UserId = result.UserId,
            Score = result.Score ?? 0,
            AttemptDate = result.AttemptDate,
            Answers = result.Answers.Select(a => new QuizAttemptAnswerDto
            {
                QuestionId = a.QuestionId,
                AnswerId = a.AnswerId
            }).ToList()
        };
    }

    public IEnumerable<QuizAttemptDto> Map(IEnumerable<QuizAttemptResult> results)
    {
        return results.Select(Map);
    }

    public CreateQuizAttemptCommand Map(CreateQuizAttemptDto dto)
    {
        return new CreateQuizAttemptCommand
        {
            QuizId = dto.QuizId
        };
    }

    public SubmitQuizAttemptCommand Map(SubmitQuizAttemptDto dto, int id)
    {
        return new SubmitQuizAttemptCommand
        {
            Id = id,
            Answers = dto.Answers.Select(a => new QuizAttemptAnswerCommand
            {
                QuestionId = a.QuestionId,
                AnswerId = a.AnswerId
            }).ToList()
        };
    }
}