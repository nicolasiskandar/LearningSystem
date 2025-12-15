using LearningSystem.Application.Services.Courses;
using LearningSystem.Application.Services.Users;
using LearningSystem.Application.Services.Categories;
using Microsoft.Extensions.DependencyInjection;
using LearningSystem.Application.Mappers.Users;
using LearningSystem.Application.Mappers.Categories;
using LearningSystem.Application.Mappers.Courses;

namespace LearningSystem.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ICourseService, CourseService>();
        services.AddScoped<ICategoryService, CategoryService>();

        services.AddScoped<ICategoryMapper, CategoryMapper>();
        services.AddScoped<ICourseMapper, CourseMapper>();
        services.AddScoped<IUserMapper, UserMapper>();

        return services;
    }
}
