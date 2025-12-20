using LearningSystem.Application.Commands.Quizzes;
using LearningSystem.Application.Results.Quizzes;
using LearningSystem.Application.Mappers.Questions;
using LearningSystem.Domain.Entities;

namespace LearningSystem.Application.Mappers.Quizzes;

public class QuizMapper : IQuizMapper
{
    private readonly IQuestionMapper _questionMapper;

    public QuizMapper(IQuestionMapper questionMapper)
    {
        _questionMapper = questionMapper;
    }

    public QuizResult Map(Quiz quiz)
    {
        return new QuizResult
        {
            Id = quiz.Id,
            Title = quiz.Title,
            PassingScore = quiz.PassingScore,
            TimeLimit = quiz.TimeLimit,
            CourseId = quiz.CourseId,
            LessonId = quiz.LessonId,
            Questions = quiz.Questions.Select(q => _questionMapper.Map(q)).ToList()
        };
    }

    public IEnumerable<QuizResult> Map(IEnumerable<Quiz> quizzes)
    {
        return quizzes.Select(Map);
    }

    public Quiz Map(CreateQuizCommand command)
    {
        return new Quiz
        {
            Title = command.Title,
            PassingScore = command.PassingScore,
            TimeLimit = command.TimeLimit,
            CourseId = command.CourseId,
            LessonId = command.LessonId
        };
    }

    public void Map(UpdateQuizCommand command, Quiz quiz)
    {
        quiz.Title = command.Title;
        quiz.PassingScore = command.PassingScore;
        quiz.TimeLimit = command.TimeLimit;
    }
}