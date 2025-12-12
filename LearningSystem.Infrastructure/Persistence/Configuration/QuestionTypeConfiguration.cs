using LearningSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearningSystem.Infrastructure.Persistence.Configuration;

public class QuestionTypeConfiguration : IEntityTypeConfiguration<QuestionType>
{
    public void Configure(EntityTypeBuilder<QuestionType> builder)
    {
        builder.HasKey(qt => qt.Id);
        builder.Property(qt => qt.Name).HasMaxLength(20);
        builder.HasIndex(qt => qt.Name).IsUnique();

        builder.HasData(
            new QuestionType { Id = 1, Name = "MCQ" },
            new QuestionType { Id = 2, Name = "TF" },
            new QuestionType { Id = 3, Name = "MSQ" }
        );
    }
}
