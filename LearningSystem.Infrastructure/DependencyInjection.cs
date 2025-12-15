using LearningSystem.Application.Common.Security;
using LearningSystem.Application.Persistence;
using LearningSystem.Infrastructure.Persistence;
using LearningSystem.Infrastructure.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LearningSystem.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
      this IServiceCollection services,
      ConfigurationManager configuration)
    {
        services.AddDbContext<LearningSystemDbContext>(options =>
           options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddSingleton<PasswordHasher<object>>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();

        return services;
    }
}