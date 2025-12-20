using LearningSystem.Api.Dtos.QuestionTypes;
using LearningSystem.Application.Commands.QuestionTypes;
using LearningSystem.Application.Results.QuestionTypes;

namespace LearningSystem.Api.Mappers.QuestionTypes;

public class QuestionTypeMapper : IQuestionTypeMapper
{
    public QuestionTypeDto Map(QuestionTypeResult result)
    {
        return new QuestionTypeDto
        {
            Id = result.Id,
            Name = result.Name
        };
    }

    public IEnumerable<QuestionTypeDto> Map(IEnumerable<QuestionTypeResult> results)
    {
        return results.Select(Map);
    }

    public CreateQuestionTypeCommand Map(CreateQuestionTypeDto dto)
    {
        return new CreateQuestionTypeCommand
        {
            Name = dto.Name
        };
    }

    public UpdateQuestionTypeCommand Map(UpdateQuestionTypeDto dto, int id)
    {
        return new UpdateQuestionTypeCommand
        {
            Id = id,
            Name = dto.Name
        };
    }
}
