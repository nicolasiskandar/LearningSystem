using LearningSystem.Application.Services.Courses;
using LearningSystem.Application.Services.Users;
using LearningSystem.Application.Services.Categories;
using Microsoft.Extensions.DependencyInjection;
using LearningSystem.Application.Mappers.Users;
using LearningSystem.Application.Mappers.Categories;
using LearningSystem.Application.Mappers.Courses;
using LearningSystem.Application.Mappers.Lessons;
using LearningSystem.Application.Mappers.Certificates;
using LearningSystem.Application.Services.Auth;
using LearningSystem.Application.Services.Lessons;
using LearningSystem.Application.Services.Certificates;

namespace LearningSystem.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ICourseService, CourseService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ILessonService, LessonService>();
        services.AddScoped<ICertificateService, CertificateService>();

        services.AddScoped<ICategoryMapper, CategoryMapper>();
        services.AddScoped<ICourseMapper, CourseMapper>();
        services.AddScoped<IUserMapper, UserMapper>();
        services.AddScoped<ILessonMapper, LessonMapper>();
        services.AddScoped<ICertificateMapper, CertificateMapper>();

        return services;
    }
}
