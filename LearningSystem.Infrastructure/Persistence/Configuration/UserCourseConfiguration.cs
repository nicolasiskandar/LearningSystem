using LearningSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearningSystem.Infrastructure.Persistence.Configuration;

public class UserCourseConfiguration : IEntityTypeConfiguration<UserCourse>
{
    public void Configure(EntityTypeBuilder<UserCourse> builder)
    {
        builder.HasKey(uc => uc.Id);
        builder.HasIndex(uc => new { uc.UserId, uc.CourseId }).IsUnique();

        builder.HasOne(uc => uc.User)
               .WithMany(u => u.UserCourses)
               .HasForeignKey(uc => uc.UserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(uc => uc.Course)
               .WithMany(c => c.UserCourses)
               .HasForeignKey(uc => uc.CourseId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
