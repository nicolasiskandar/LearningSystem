using LearningSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearningSystem.Infrastructure.Persistence.Configuration;

public class LessonCompletedConfiguration : IEntityTypeConfiguration<LessonCompleted>
{
    public void Configure(EntityTypeBuilder<LessonCompleted> builder)
    {
        builder.HasKey(lc => lc.Id);

        builder.Property(lc => lc.CompletedDate).IsRequired();

        builder.HasOne(lc => lc.Lesson)
               .WithMany(l => l.LessonsCompleted)
               .HasForeignKey(lc => lc.LessonId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(lc => lc.User)
               .WithMany(u => u.LessonsCompleted)
               .HasForeignKey(lc => lc.UserId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
