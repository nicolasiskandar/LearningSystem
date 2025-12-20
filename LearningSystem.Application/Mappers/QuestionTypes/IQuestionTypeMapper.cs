using LearningSystem.Application.Commands.QuestionTypes;
using LearningSystem.Application.Results.QuestionTypes;
using LearningSystem.Domain.Entities;

namespace LearningSystem.Application.Mappers.QuestionTypes;

public interface IQuestionTypeMapper
{
    QuestionTypeResult Map(QuestionType questionType);
    IEnumerable<QuestionTypeResult> Map(IEnumerable<QuestionType> questionTypes);
    QuestionType Map(CreateQuestionTypeCommand command);
    void Map(UpdateQuestionTypeCommand command, QuestionType questionType);
}
