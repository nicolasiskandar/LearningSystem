using LearningSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearningSystem.Infrastructure.Persistence.Configuration;

public class QuizAttemptAnswerConfiguration : IEntityTypeConfiguration<QuizAttemptAnswer>
{
    public void Configure(EntityTypeBuilder<QuizAttemptAnswer> builder)
    {
        builder.HasKey(qaa => qaa.Id);

        builder.HasOne(qaa => qaa.QuizAttempt)
               .WithMany(qa => qa.QuizAttemptAnswers)
               .HasForeignKey(qaa => qaa.QuizAttemptId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(qaa => qaa.Question)
               .WithMany(q => q.QuizAttemptAnswers)
               .HasForeignKey(qaa => qaa.QuestionId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(qaa => qaa.Answer)
               .WithMany(a => a.QuizAttemptAnswers)
               .HasForeignKey(qaa => qaa.AnswerId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
