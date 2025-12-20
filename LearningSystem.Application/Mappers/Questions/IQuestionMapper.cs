using LearningSystem.Application.Commands.Questions;
using LearningSystem.Application.Results.Questions;
using LearningSystem.Domain.Entities;

namespace LearningSystem.Application.Mappers.Questions;

public interface IQuestionMapper
{
    QuestionResult Map(Question question);
    IEnumerable<QuestionResult> Map(IEnumerable<Question> questions);
    Question Map(CreateQuestionCommand command);
    void Map(UpdateQuestionCommand command, Question question);
}