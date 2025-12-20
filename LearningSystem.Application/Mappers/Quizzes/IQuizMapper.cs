using LearningSystem.Application.Commands.Quizzes;
using LearningSystem.Application.Results.Quizzes;
using LearningSystem.Domain.Entities;

namespace LearningSystem.Application.Mappers.Quizzes;

public interface IQuizMapper
{
    QuizResult Map(Quiz quiz);
    IEnumerable<QuizResult> Map(IEnumerable<Quiz> quizzes);
    Quiz Map(CreateQuizCommand command);
    void Map(UpdateQuizCommand command, Quiz quiz);
}