using LearningSystem.Api.Mappers.Authentication;
using LearningSystem.Api.Mappers.Categories;
using LearningSystem.Api.Mappers.Courses;
using LearningSystem.Api.Mappers.Users;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApi(this IServiceCollection services)
    {
        services.AddScoped<ICategoryMapper, CategoryMapper>();
        services.AddScoped<IUserMapper, UserMapper>();
        services.AddScoped<ICourseMapper, CourseMapper>();
        services.AddScoped<IAuthMapper, AuthMapper>();

        services.AddHttpContextAccessor();

        return services;
    }
}
