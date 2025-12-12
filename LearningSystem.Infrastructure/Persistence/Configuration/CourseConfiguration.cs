using LearningSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearningSystem.Infrastructure.Persistence.Configuration;

public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.ShortDescription).IsRequired();
        builder.Property(c => c.LongDescription).IsRequired();
        builder.Property(c => c.Difficulty).HasMaxLength(20).IsRequired();
        builder.Property(c => c.Thumbnail).IsRequired();
        builder.Property(c => c.CreatedAt).IsRequired();
        builder.Property(c => c.IsPublished).HasDefaultValue(false);

        builder.HasOne(c => c.Category)
               .WithMany(cat => cat.Courses)
               .HasForeignKey(c => c.CategoryId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
