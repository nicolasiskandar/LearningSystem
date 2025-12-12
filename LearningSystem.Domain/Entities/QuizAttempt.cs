namespace LearningSystem.Domain.Entities;

public partial class QuizAttempt
{
    public int Id { get; set; }

    public int QuizId { get; set; }

    public int UserId { get; set; }

    public int Score { get; set; }

    public DateTime? AttemptDate { get; set; }

    public virtual Quiz Quiz { get; set; } = null!;

    public virtual ICollection<QuizAttemptAnswer> QuizAttemptAnswers { get; set; } = new List<QuizAttemptAnswer>();

    public virtual User User { get; set; } = null!;
}
