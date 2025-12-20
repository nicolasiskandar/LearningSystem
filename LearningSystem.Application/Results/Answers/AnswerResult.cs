namespace LearningSystem.Application.Results.Answers;

public class AnswerResult
{
    public int Id { get; set; }
    public string AnswerText { get; set; } = null!;
    public bool IsCorrect { get; set; }
    public int QuestionId { get; set; }
}
