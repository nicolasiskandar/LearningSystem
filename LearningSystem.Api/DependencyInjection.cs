using LearningSystem.Api.Mappers.QuestionTypes;
using LearningSystem.Api.Mappers.Authentication;
using LearningSystem.Api.Mappers.Categories;
using LearningSystem.Api.Mappers.Courses;
using LearningSystem.Api.Mappers.Lessons;
using LearningSystem.Api.Mappers.Certificates;
using LearningSystem.Api.Mappers.Users;
using LearningSystem.Api.Mappers.Questions;
using LearningSystem.Api.Mappers.Quizzes;
using LearningSystem.Api.Mappers.QuizAttempts;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApi(this IServiceCollection services)
    {
        services.AddScoped<ICategoryMapper, CategoryMapper>();
        services.AddScoped<IUserMapper, UserMapper>();
        services.AddScoped<ICourseMapper, CourseMapper>();
        services.AddScoped<IAuthMapper, AuthMapper>();
        services.AddScoped<ILessonMapper, LessonMapper>();
        services.AddScoped<ICertificateMapper, CertificateMapper>();
        services.AddScoped<IQuestionMapper, QuestionMapper>();
        services.AddScoped<IQuizMapper, QuizMapper>();
        services.AddScoped<IQuestionTypeMapper, QuestionTypeMapper>();
        services.AddScoped<IQuizAttemptMapper, QuizAttemptMapper>();

        services.AddHttpContextAccessor();

        return services;
    }
}
