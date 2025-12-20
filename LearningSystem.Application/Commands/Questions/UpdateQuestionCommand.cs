using LearningSystem.Application.Commands.Answers;

namespace LearningSystem.Application.Commands.Questions;

public class UpdateQuestionCommand
{
    public int Id { get; set; }
    public string QuestionText { get; set; }
    public int QuestionTypeId { get; set; }
    public int Order { get; set; }
    public List<UpdateAnswerCommand> Answers { get; set; } = new();

    public UpdateQuestionCommand(int id, string questionText, int questionTypeId, int order, List<UpdateAnswerCommand> answers)
    {
        Id = id;
        QuestionText = questionText;
        QuestionTypeId = questionTypeId;
        Order = order;
        Answers = answers;
    }
}

