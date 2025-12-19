using LearningSystem.Application.Common.Exceptions;
using System.Net;
using System.Text.Json;

namespace LearningSystem.Api.Middleware;

public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
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

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        HttpStatusCode status;
        string message = exception.Message;

        switch (exception)
        {
            case NotFoundException:
                status = HttpStatusCode.NotFound;
                break;
            case AlreadyExistsException:
            case FailedException:
                status = HttpStatusCode.BadRequest;
                break;
            case ConflictException:
                status = HttpStatusCode.Conflict;
                break;
            case ForbiddenExcception:
                status = HttpStatusCode.Forbidden;
                break;
            case UnauthorizedAccessException:
                status = HttpStatusCode.Unauthorized;
                break;
            default:
                status = HttpStatusCode.InternalServerError;
                Console.WriteLine(message);
                message = "An unexpected error occurred.";
                break;
        }

        context.Response.StatusCode = (int)status;
        var response = JsonSerializer.Serialize(new { message });
        return context.Response.WriteAsync(response);
    }
}
