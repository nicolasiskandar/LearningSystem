namespace LearningSystem.Application.Commands.Answers;

public class CreateAnswerCommand
{
    public string AnswerText { get; set; }
    public bool IsCorrect { get; set; }

    public CreateAnswerCommand(string answerText, bool isCorrect)
    {
        AnswerText = answerText;
        IsCorrect = isCorrect;
    }
}
