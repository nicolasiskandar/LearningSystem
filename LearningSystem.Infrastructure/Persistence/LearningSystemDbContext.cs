using LearningSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LearningSystem.Infrastructure.Persistence;

public class LearningSystemDbContext : IdentityDbContext<User, Role, int>
{
    public LearningSystemDbContext(DbContextOptions<LearningSystemDbContext> options)
        : base(options)
    {
    }

    public DbSet<Answer> Answers { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Certificate> Certificates { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Lesson> Lessons { get; set; }
    public DbSet<LessonCompleted> LessonCompleteds { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<QuestionType> QuestionTypes { get; set; }
    public DbSet<Quiz> Quizzes { get; set; }
    public DbSet<QuizAttempt> QuizAttempts { get; set; }
    public DbSet<QuizAttemptAnswer> QuizAttemptAnswers { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserCourse> UserCourses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LearningSystemDbContext).Assembly);
    }
}
