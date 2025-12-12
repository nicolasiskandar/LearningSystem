using LearningSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearningSystem.Infrastructure.Persistence.Configuration;

public class QuizConfiguration : IEntityTypeConfiguration<Quiz>
{
    public void Configure(EntityTypeBuilder<Quiz> builder)
    {
        builder.HasKey(q => q.Id);

        builder.Property(q => q.Title).HasMaxLength(255).IsRequired();
        builder.Property(q => q.PassingScore).IsRequired();
        builder.Property(q => q.TimeLimit).IsRequired();

        builder.ToTable(tb =>
        {
            tb.HasCheckConstraint("CHK_Quiz_PassingScore", "[PassingScore] >= 0");
            tb.HasCheckConstraint("CHK_Quiz_TimeLimit", "[TimeLimit] >= 0");
        });

        builder.HasOne(q => q.Course)
               .WithMany(c => c.Quizzes)
               .HasForeignKey(q => q.CourseId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(q => q.Lesson)
               .WithMany(l => l.Quizzes)
               .HasForeignKey(q => q.LessonId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
