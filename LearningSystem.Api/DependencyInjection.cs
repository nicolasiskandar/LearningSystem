using System.Threading.RateLimiting;
using LearningSystem.Api.Filters;
using LearningSystem.Api.Mappers.Authentication;
using LearningSystem.Api.Mappers.Categories;
using LearningSystem.Api.Mappers.Certificates;
using LearningSystem.Api.Mappers.Courses;
using LearningSystem.Api.Mappers.Lessons;
using LearningSystem.Api.Mappers.Questions;
using LearningSystem.Api.Mappers.QuestionTypes;
using LearningSystem.Api.Mappers.QuizAttempts;
using LearningSystem.Api.Mappers.Quizzes;
using LearningSystem.Api.Mappers.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.OpenApi.Models;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApi(this IServiceCollection services, IConfiguration configuration)
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

        services.AddControllers(options =>
        {
            options.Filters.Add<ValidationFilter>();
        });
        
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter: Bearer {your JWT token}"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        services.AddRateLimiter(options =>
        {
            var rateLimitingOptions = configuration.GetSection("RateLimiting");

            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            options.OnRejected = (context, cancellationToken) =>
            {
                context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>()
                    .CreateLogger("Microsoft.AspNetCore.RateLimiting")
                    .LogWarning($"Request rejected by rate limiter: {context.HttpContext.Request.Path}");
                return new ValueTask();
            };

            options.AddFixedWindowLimiter("fixed", opt =>
            {
                opt.PermitLimit = rateLimitingOptions.GetValue<int>("PermitLimit");
                opt.Window = rateLimitingOptions.GetValue<TimeSpan>("Window");
                opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                opt.QueueLimit = rateLimitingOptions.GetValue<int>("QueueLimit");
            });
        });

        return services;
    }
}
