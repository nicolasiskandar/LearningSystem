using LearningSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearningSystem.Infrastructure.Persistence.Configuration;

public class QuizAttemptConfiguration : IEntityTypeConfiguration<QuizAttempt>
{
    public void Configure(EntityTypeBuilder<QuizAttempt> builder)
    {
        builder.HasKey(qa => qa.Id);

        builder.Property(qa => qa.Score).IsRequired();
        builder.Property(qa => qa.AttemptDate);

        builder.ToTable(tb =>
        {
            tb.HasCheckConstraint("CHK_QuizAttempt_Score", "[Score] >= 0");
        });

        builder.HasOne(qa => qa.Quiz)
               .WithMany(q => q.QuizAttempts)
               .HasForeignKey(qa => qa.QuizId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(qa => qa.User)
               .WithMany(u => u.QuizAttempts)
               .HasForeignKey(qa => qa.UserId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
