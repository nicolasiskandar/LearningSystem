namespace LearningSystem.Api.Dtos.Answers;

public class AnswerDto
{
    public int Id { get; set; }
    public string AnswerText { get; set; } = null!;
    public bool IsCorrect { get; set; }
    public int QuestionId { get; set; }
}
