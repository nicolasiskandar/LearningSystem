using LearningSystem.Application.Commands.QuestionTypes;
using LearningSystem.Application.Results.QuestionTypes;
using LearningSystem.Domain.Entities;

namespace LearningSystem.Application.Mappers.QuestionTypes;

public class QuestionTypeMapper : IQuestionTypeMapper
{
    public QuestionTypeResult Map(QuestionType questionType)
    {
        return new QuestionTypeResult
        {
            Id = questionType.Id,
            Name = questionType.Name
        };
    }

    public IEnumerable<QuestionTypeResult> Map(IEnumerable<QuestionType> questionTypes)
    {
        return questionTypes.Select(Map);
    }

    public QuestionType Map(CreateQuestionTypeCommand command)
    {
        return new QuestionType
        {
            Name = command.Name
        };
    }

    public void Map(UpdateQuestionTypeCommand command, QuestionType questionType)
    {
        questionType.Name = command.Name;
    }
}
