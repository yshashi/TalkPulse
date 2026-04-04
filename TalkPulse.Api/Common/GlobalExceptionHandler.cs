using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace TalkPulse.Api.Common;

internal sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var problemDetails = exception switch
        {
            ValidationException validationExp => new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Validation Failed",
                Detail = "One or more validation errors occurred.",
                Extensions =
                {
                    ["errors"] = validationExp.Errors.Select(e => new
                    {
                        e.PropertyName,
                        e.ErrorMessage
                    }).ToArray()
                }
            },
            ArgumentException argExp => new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Invalid Argument",
                Detail = argExp.Message
            },
            InvalidOperationException ioExp => new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Invalid Operation",
                Detail = ioExp.Message
            },
            KeyNotFoundException notFoundExp => new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Resource Not Found",
                Detail = notFoundExp.Message
            },
            _ => new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "An unexpected error occurred.",
                Detail = "Please try again later or contact support if the issue persists."
            }


        };
        if (problemDetails.Status >= 500)
        {
            logger.LogError(exception, "An unhandled exception occurred: {Message}", exception.Message);
        }
        else
        {
            logger.LogWarning(exception, "A handled exception occurred: {Message}", exception.Message);
        }
        httpContext.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
        httpContext.Response.ContentType = "application/problem+json";
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }

}