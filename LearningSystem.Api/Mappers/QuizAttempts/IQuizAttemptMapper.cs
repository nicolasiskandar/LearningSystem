using LearningSystem.Api.Dtos.QuizAttempts;
using LearningSystem.Application.Commands.QuizAttempts;
using LearningSystem.Application.Results.QuizAttempts;

namespace LearningSystem.Api.Mappers.QuizAttempts;

public interface IQuizAttemptMapper
{
    QuizAttemptDto Map(QuizAttemptResult result);
    IEnumerable<QuizAttemptDto> Map(IEnumerable<QuizAttemptResult> results);
    CreateQuizAttemptCommand Map(CreateQuizAttemptDto dto);
    SubmitQuizAttemptCommand Map(SubmitQuizAttemptDto dto, int id);
}