using LearningSystem.Application.Mapping;
using LearningSystem.Application.Services.Users;
using Microsoft.Extensions.DependencyInjection;

namespace LearningSystem.Application;
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddAutoMapper(typeof(MappingProfile));

        return services;
    }
}
