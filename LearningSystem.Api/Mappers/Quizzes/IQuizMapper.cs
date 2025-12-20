using LearningSystem.Api.Dtos.Quizzes;
using LearningSystem.Application.Commands.Quizzes;
using LearningSystem.Application.Results.Quizzes;

namespace LearningSystem.Api.Mappers.Quizzes;

public interface IQuizMapper
{
    QuizDto Map(QuizResult result);
    IEnumerable<QuizDto> Map(IEnumerable<QuizResult> results);
    CreateQuizCommand Map(CreateQuizDto dto);
    UpdateQuizCommand Map(UpdateQuizDto dto, int id);
}