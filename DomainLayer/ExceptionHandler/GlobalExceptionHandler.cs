using System.Data;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Task_Managment_API.DomainLayer.ExceptionHandler;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        _logger.LogError(exception, exception.Message);

        var details = new ProblemDetails
        {
            Title = "An error occurred",
            Instance = httpContext.Request.Path,
            Type = "API Error",
            Detail = exception.Message
        };

        switch (exception)
        {
            case ArgumentException _:
                details.Type = "Request Error";
                details.Status = StatusCodes.Status400BadRequest;
                details.Detail = "Invalid argument provided.";
                break;
            case KeyNotFoundException _:
                details.Type = "Request Error";
                details.Status = StatusCodes.Status404NotFound;
                details.Detail = "The requested resource was not found.";
                break;
            case UnauthorizedAccessException _:
                details.Type = "Request Error";
                details.Status = StatusCodes.Status401Unauthorized;
                details.Detail = "You are not authorized to access this resource.";
                break;
            case  DuplicateNameException :
                details.Type = "Request Error";
                details.Status = StatusCodes.Status409Conflict;
                details.Detail = "The resource already exists.";
                break;
            default:
                details.Type = "API Error";
                details.Status = StatusCodes.Status500InternalServerError;
                details.Detail = "An unexpected error occurred.";
                break;
        }

        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = details.Status.Value;
        await httpContext.Response.WriteAsJsonAsync(details, cancellationToken: cancellationToken);
        return true;
    }
}