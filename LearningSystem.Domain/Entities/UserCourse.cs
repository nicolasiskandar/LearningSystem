namespace LearningSystem.Domain.Entities;

public partial class UserCourse
{
    public int Id { get; set; }

    public int CourseId { get; set; }

    public int UserId { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
