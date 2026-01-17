using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace LearningSystem.Infrastructure.Persistence;

public class LearningSystemDbContextFactory : IDesignTimeDbContextFactory<LearningSystemDbContext>
{
    public LearningSystemDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<LearningSystemDbContext>();

        optionsBuilder.UseSqlServer("Server=Lenovo;Database=OnlineLearningSystemDb;Integrated Security=true;TrustServerCertificate=true;");

        return new LearningSystemDbContext(optionsBuilder.Options);
    }
}
