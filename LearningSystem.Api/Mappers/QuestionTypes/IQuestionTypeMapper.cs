using LearningSystem.Api.Dtos.QuestionTypes;
using LearningSystem.Application.Commands.QuestionTypes;
using LearningSystem.Application.Results.QuestionTypes;

namespace LearningSystem.Api.Mappers.QuestionTypes;

public interface IQuestionTypeMapper
{
    QuestionTypeDto Map(QuestionTypeResult result);
    IEnumerable<QuestionTypeDto> Map(IEnumerable<QuestionTypeResult> results);
    CreateQuestionTypeCommand Map(CreateQuestionTypeDto dto);
    UpdateQuestionTypeCommand Map(UpdateQuestionTypeDto dto, int id);
}
