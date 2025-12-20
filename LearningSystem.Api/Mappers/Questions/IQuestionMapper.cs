using LearningSystem.Api.Dtos.Questions;
using LearningSystem.Application.Commands.Questions;
using LearningSystem.Application.Results.Questions;

namespace LearningSystem.Api.Mappers.Questions;

public interface IQuestionMapper
{
    QuestionDto Map(QuestionResult result);
    IEnumerable<QuestionDto> Map(IEnumerable<QuestionResult> results);
    CreateQuestionCommand Map(CreateQuestionDto dto);
    UpdateQuestionCommand Map(UpdateQuestionDto dto, int id);
}