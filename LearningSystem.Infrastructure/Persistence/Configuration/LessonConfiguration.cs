using LearningSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;
using System.Xml;

namespace LearningSystem.Infrastructure.Persistence.Configuration;

public class LessonConfiguration : IEntityTypeConfiguration<Lesson>
{
    public void Configure(EntityTypeBuilder<Lesson> builder)
    {
        builder.HasKey(l => l.Id);

        builder.HasIndex(l => new { l.CourseId, l.Order }).IsUnique();

        builder.Property(l => l.Title).HasMaxLength(255).IsRequired();
        builder.Property(l => l.Content).IsRequired();
        builder.Property(l => l.EstimatedDuration).IsRequired();
        builder.Property(l => l.CreatedAt).IsRequired();

        builder.ToTable(tb =>
        {
            tb.HasCheckConstraint("CHK_Lesson_EstimatedDuration", "[EstimatedDuration] > 0");
        });

        builder.HasOne(l => l.Course)
               .WithMany(c => c.Lessons)
               .HasForeignKey(l => l.CourseId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(l => l.CreatedByNavigation)
               .WithMany()
               .HasForeignKey(l => l.CreatedBy)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
