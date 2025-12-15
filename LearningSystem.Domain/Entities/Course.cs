namespace LearningSystem.Domain.Entities;

public partial class Course
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string ShortDescription { get; set; } = null!;

    public string LongDescription { get; set; } = null!;

    public int CategoryId { get; set; }

    public string Difficulty { get; set; } = null!;

    public int CreatedBy { get; set; }

    public string Thumbnail { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public bool IsPublished { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<Certificate> Certificates { get; set; } = new List<Certificate>();

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();

    public virtual ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();

    public virtual ICollection<UserCourse> UserCourses { get; set; } = new List<UserCourse>();
}
