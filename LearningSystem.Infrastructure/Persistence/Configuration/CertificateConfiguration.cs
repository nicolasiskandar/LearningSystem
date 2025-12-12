using LearningSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearningSystem.Infrastructure.Persistence.Configuration;

public class CertificateConfiguration : IEntityTypeConfiguration<Certificate>
{
    public void Configure(EntityTypeBuilder<Certificate> builder)
    {
        builder.HasKey(c => c.Id);
        builder.HasIndex(c => new { c.UserId, c.CourseId }).IsUnique();

        builder.Property(c => c.DownloadUrl).IsRequired();

        builder.HasOne(c => c.User)
               .WithMany(u => u.Certificates)
               .HasForeignKey(c => c.UserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.Course)
               .WithMany(c => c.Certificates)
               .HasForeignKey(c => c.CourseId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
