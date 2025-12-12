namespace LearningSystem.Domain.Entities;

public partial class Quiz
{
    public int Id { get; set; }

    public int CourseId { get; set; }

    public int LessonId { get; set; }

    public string Title { get; set; } = null!;

    public int PassingScore { get; set; }

    public int TimeLimit { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual Lesson Lesson { get; set; } = null!;

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();

    public virtual ICollection<QuizAttempt> QuizAttempts { get; set; } = new List<QuizAttempt>();
}
