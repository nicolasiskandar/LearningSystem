namespace LearningSystem.Domain.Entities;

public partial class QuizAttemptAnswer
{
    public int Id { get; set; }

    public int QuizAttemptId { get; set; }

    public int QuestionId { get; set; }

    public int AnswerId { get; set; }

    public virtual Answer Answer { get; set; } = null!;

    public virtual Question Question { get; set; } = null!;

    public virtual QuizAttempt QuizAttempt { get; set; } = null!;
}
