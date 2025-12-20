using LearningSystem.Api.Dtos.Answers;
using LearningSystem.Api.Dtos.Questions;
using LearningSystem.Application.Commands.Answers;
using LearningSystem.Application.Commands.Questions;
using LearningSystem.Application.Results.Questions;

namespace LearningSystem.Api.Mappers.Questions;

public class QuestionMapper : IQuestionMapper
{
    public QuestionDto Map(QuestionResult result)
    {
        return new QuestionDto
        {
            Id = result.Id,
            QuestionText = result.QuestionText,
            QuestionType = result.QuestionType,
            Order = result.Order,
            QuizId = result.QuizId,
            Answers = result.Answers.Select(a => new AnswerDto
            {
                Id = a.Id,
                AnswerText = a.AnswerText,
                IsCorrect = a.IsCorrect,
                QuestionId = a.QuestionId
            }).ToList()
        };
    }

    public IEnumerable<QuestionDto> Map(IEnumerable<QuestionResult> results)
    {
        return results.Select(Map);
    }

    public CreateQuestionCommand Map(CreateQuestionDto dto)
    {
        return new CreateQuestionCommand(
            dto.QuestionText,
            dto.QuestionTypeId,
            dto.Order,
            dto.QuizId,
            dto.Answers.Select(a => new CreateAnswerCommand(a.AnswerText, a.IsCorrect)).ToList()
        );
    }

    public UpdateQuestionCommand Map(UpdateQuestionDto dto, int id)
    {
        return new UpdateQuestionCommand(
            id,
            dto.QuestionText,
            dto.QuestionTypeId,
            dto.Order,
            dto.Answers.Select(a => new UpdateAnswerCommand(a.Id, a.AnswerText, a.IsCorrect)).ToList()
        );
    }
}