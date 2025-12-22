using LearningSystem.Application.Common.Exceptions;
using System.Net;
using System.Text.Json;

namespace LearningSystem.Api.Middleware;

public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

    public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        HttpStatusCode status;
        string message = exception.Message;

        switch (exception)
        {
            case NotFoundException:
                status = HttpStatusCode.NotFound;
                _logger.LogWarning(exception, $"A 'Not Found' error occurred: {message}");
                break;
            case AlreadyExistsException:
            case FailedException:
                status = HttpStatusCode.BadRequest;
                _logger.LogWarning(exception, $"A 'Bad Request' error occurred: {message}");
                break;
            case ConflictException:
                status = HttpStatusCode.Conflict;
                _logger.LogWarning(exception, $"A 'Conflict' error occurred: {message}");
                break;
            case ForbiddenExcception:
                status = HttpStatusCode.Forbidden;
                _logger.LogWarning(exception, $"A 'Forbidden' error occurred: {message}");
                break;
            case UnauthorizedAccessException:
                status = HttpStatusCode.Unauthorized;
                _logger.LogWarning(exception, $"An 'Unauthorized Access' error occurred: {message}");
                break;
            default:
                status = HttpStatusCode.InternalServerError;
                _logger.LogError(exception, $"An unexpected error occurred: {message}");
                message = "An unexpected error occurred.";
                break;
        }

        context.Response.StatusCode = (int)status;
        var response = JsonSerializer.Serialize(new { message });
        return context.Response.WriteAsync(response);
    }
}
