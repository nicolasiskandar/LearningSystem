using LearningSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearningSystem.Infrastructure.Persistence.Configuration;

public class QuestionConfiguration : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        builder.HasKey(q => q.Id);

        builder.Property(q => q.QuestionText).IsRequired();
        builder.Property(q => q.Order).IsRequired();

        builder.HasOne(q => q.Quiz)
               .WithMany(quiz => quiz.Questions)
               .HasForeignKey(q => q.QuizId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(q => q.QuestionType)
               .WithMany(qt => qt.Questions)
               .HasForeignKey(q => q.QuestionTypeId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
