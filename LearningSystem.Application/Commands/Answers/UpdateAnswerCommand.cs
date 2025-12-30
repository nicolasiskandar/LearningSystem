namespace LearningSystem.Application.Commands.Answers;

public class UpdateAnswerCommand
{
    public int Id { get; set; }
    public string AnswerText { get; set; }
    public bool IsCorrect { get; set; }

    public UpdateAnswerCommand(int id, string answerText, bool isCorrect)
    {
        Id = id;
        AnswerText = answerText;
        IsCorrect = isCorrect;
    }
}
