namespace LearningSystem.Api.Dtos.QuizAttempts;

public class QuizAttemptDto
{
    public int Id { get; set; }
    public int QuizId { get; set; }
    public int UserId { get; set; }
    public int Score { get; set; }
    public DateTime? AttemptDate { get; set; }
    public ICollection<QuizAttemptAnswerDto> Answers { get; set; } = [];
}
