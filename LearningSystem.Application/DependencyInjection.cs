using LearningSystem.Application.Mappers.Categories;
using LearningSystem.Application.Mappers.Certificates;
using LearningSystem.Application.Mappers.Courses;
using LearningSystem.Application.Mappers.Lessons;
using LearningSystem.Application.Mappers.Questions;
using LearningSystem.Application.Mappers.Quizzes;
using LearningSystem.Application.Mappers.Users;
using LearningSystem.Application.Mappers.QuestionTypes;
using LearningSystem.Application.Services.Auth;
using LearningSystem.Application.Services.Categories;
using LearningSystem.Application.Services.Certificates;
using LearningSystem.Application.Services.Courses;
using LearningSystem.Application.Services.Lessons;
using LearningSystem.Application.Services.Questions;
using LearningSystem.Application.Services.QuestionTypes;
using LearningSystem.Application.Services.Quizzes;
using LearningSystem.Application.Services.QuizAttempts;
using LearningSystem.Application.Services.Users;
using Microsoft.Extensions.DependencyInjection;
using LearningSystem.Application.Mappers.QuizAttempts;


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
        services.AddScoped<IQuestionService, QuestionService>();
        services.AddScoped<IQuestionTypeService, QuestionTypeService>();
        services.AddScoped<IQuizService, QuizService>();
        services.AddScoped<IQuizAttemptService, QuizAttemptService>();

        services.AddScoped<ICategoryMapper, CategoryMapper>();
        services.AddScoped<ICourseMapper, CourseMapper>();
        services.AddScoped<IUserMapper, UserMapper>();
        services.AddScoped<ILessonMapper, LessonMapper>();
        services.AddScoped<ICertificateMapper, CertificateMapper>();
        services.AddScoped<IQuestionMapper, QuestionMapper>();
        services.AddScoped<IQuizMapper, QuizMapper>();
        services.AddScoped<IQuestionTypeMapper, QuestionTypeMapper>();
        services.AddScoped<IQuizAttemptMapper, QuizAttemptMapper>();

        return services;
    }
}
