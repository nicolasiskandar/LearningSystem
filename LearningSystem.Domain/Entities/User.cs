using Microsoft.AspNetCore.Identity;

namespace LearningSystem.Domain.Entities;

public partial class User : IdentityUser<int>
{
    public string FullName { get; set; } = null!;
    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Certificate> Certificates { get; set; } = [];
    public virtual ICollection<Course> Courses { get; set; } = [];
    public virtual ICollection<LessonCompleted> LessonsCompleted { get; set; } = [];
    public virtual ICollection<QuizAttempt> QuizAttempts { get; set; } = [];
    public virtual ICollection<UserCourse> UserCourses { get; set; } = [];
}
