using LearningSystem.Api.Dtos.Answers;
using LearningSystem.Api.Dtos.Questions;
using LearningSystem.Api.Dtos.Quizzes;
using LearningSystem.Application.Commands.Quizzes;
using LearningSystem.Application.Results.Quizzes;

namespace LearningSystem.Api.Mappers.Quizzes;

public class QuizMapper : IQuizMapper
{
    public QuizDto Map(QuizResult result)
    {
        return new QuizDto
        {
            Id = result.Id,
            Title = result.Title,
            PassingScore = result.PassingScore,
            TimeLimit = result.TimeLimit,
            CourseId = result.CourseId,
            LessonId = result.LessonId,
            Questions = result.Questions.Select(q => new QuestionDto
            {
                Id = q.Id,
                QuestionText = q.QuestionText,
                QuestionType = q.QuestionType,
                Order = q.Order,
                QuizId = q.Id,
                Answers = q.Answers.Select(a => new AnswerDto
                {
                    Id = a.Id,
                    AnswerText = a.AnswerText,
                    IsCorrect = a.IsCorrect,
                    QuestionId = a.QuestionId
                }).ToList()
            }).ToList()
        };
    }

    public IEnumerable<QuizDto> Map(IEnumerable<QuizResult> results)
    {
        return results.Select(Map);
    }

    public CreateQuizCommand Map(CreateQuizDto dto)
    {
        return new CreateQuizCommand(
            dto.Title,
            dto.PassingScore,
            dto.TimeLimit,
            dto.CourseId,
            dto.LessonId
        );
    }

    public UpdateQuizCommand Map(UpdateQuizDto dto, int id)
    {
        return new UpdateQuizCommand(
            id,
            dto.Title,
            dto.PassingScore,
            dto.TimeLimit
        );
    }
}