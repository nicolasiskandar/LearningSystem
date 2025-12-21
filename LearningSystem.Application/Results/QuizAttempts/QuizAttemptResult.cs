using LearningSystem.Application.Results.QuizAttemptAnswers;

namespace LearningSystem.Application.Results.QuizAttempts;

public class QuizAttemptResult
{
    public int Id { get; set; }
    public int QuizId { get; set; }
    public int UserId { get; set; }
    public int? Score { get; set; }
    public DateTime AttemptDate { get; set; }
    public List<QuizAttemptAnswerResult> Answers { get; set; } = [];
}
