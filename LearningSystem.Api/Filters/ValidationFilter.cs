using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LearningSystem.Api.Filters;

public class ValidationFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.ModelState.IsValid) return;

        var errorMessage = context.ModelState
            .SelectMany(x => x.Value.Errors)
            .Select(e => e.ErrorMessage)
            .FirstOrDefault() ?? "Validation error";

        context.Result = new BadRequestObjectResult(new { message = errorMessage });
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}
