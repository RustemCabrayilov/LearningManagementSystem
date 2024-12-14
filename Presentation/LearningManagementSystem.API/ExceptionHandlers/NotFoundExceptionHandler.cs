using System.Text.Json;
using LearningManagementSystem.Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.API.ExceptionHandlers;

public class NotFoundExceptionHandler(ILogger<NotFoundException> _logger):IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not NotFoundException)
        {
            return false;
        }
        var problemDetails = new ProblemDetails()
        {
            Status = StatusCodes.Status500InternalServerError,
            Detail = exception.Message,
            Title = "Internal Server Error",
        };
        httpContext.Response.StatusCode = problemDetails.Status.Value;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        var errorDetails=JsonSerializer.Serialize(problemDetails);
        _logger.LogError($"problem details:{errorDetails}");
        return true;
    }
}