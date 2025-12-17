using LearningSystem.Domain.Entities;
using LearningSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearningSystem.Infrastructure.Configuration;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(r => r.Name)
            .IsUnique();

        builder.Property(r => r.NormalizedName)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasData(
            Enum.GetValues<Roles>()
                .Select(r => new Role
                {
                    Id = (int)r,
                    Name = r.ToString(),
                    NormalizedName = r.ToString().ToUpperInvariant()
                })
        );
    }
}
