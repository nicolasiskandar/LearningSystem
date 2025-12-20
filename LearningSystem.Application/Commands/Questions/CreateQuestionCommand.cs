using LearningSystem.Application.Commands.Answers;

namespace LearningSystem.Application.Commands.Questions;

public class CreateQuestionCommand
{
    public string QuestionText { get; set; }
    public int QuestionTypeId { get; set; }
    public int Order { get; set; }
    public int QuizId { get; set; }
    public List<CreateAnswerCommand> Answers { get; set; } = [];

    public CreateQuestionCommand(string questionText, int questionTypeId, int order, int quizId, List<CreateAnswerCommand> answers)
    {
        QuestionText = questionText;
        QuestionTypeId = questionTypeId;
        Order = order;
        QuizId = quizId;
        Answers = answers;
    }
}