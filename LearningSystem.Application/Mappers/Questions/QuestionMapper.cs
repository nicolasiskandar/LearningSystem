using LearningSystem.Application.Commands.Questions;
using LearningSystem.Application.Results.Answers;
using LearningSystem.Application.Results.Questions;
using LearningSystem.Domain.Entities;

namespace LearningSystem.Application.Mappers.Questions;

public class QuestionMapper : IQuestionMapper
{
    public QuestionResult Map(Question question)
    {
        return new QuestionResult
        {
            Id = question.Id,
            QuestionText = question.QuestionText,
            QuestionType = question.QuestionType.Name,
            Order = question.Order,
            QuizId = question.QuizId,
            Answers = question.Answers.Select(a => new AnswerResult
            {
                Id = a.Id,
                AnswerText = a.AnswerText,
                IsCorrect = a.IsCorrect,
                QuestionId = a.QuestionId
            }).ToList()
        };
    }

    public IEnumerable<QuestionResult> Map(IEnumerable<Question> questions)
    {
        return questions.Select(Map);
    }

    public Question Map(CreateQuestionCommand command)
    {
        return new Question
        {
            QuestionText = command.QuestionText,
            QuestionTypeId = command.QuestionTypeId,
            Order = command.Order,
            QuizId = command.QuizId,
            Answers = command.Answers.Select(a => new Answer
            {
                AnswerText = a.AnswerText,
                IsCorrect = a.IsCorrect
            }).ToList()
        };
    }

    public void Map(UpdateQuestionCommand command, Question question)
    {
        question.QuestionText = command.QuestionText;
        question.QuestionTypeId = command.QuestionTypeId;
        question.Order = command.Order;
        
        foreach (var answerCommand in command.Answers)
        {
            var existingAnswer = question.Answers.FirstOrDefault(a => a.Id == answerCommand.Id);
            if (existingAnswer != null)
            {
                existingAnswer.AnswerText = answerCommand.AnswerText;
                existingAnswer.IsCorrect = answerCommand.IsCorrect;
            }
        }
    }
}