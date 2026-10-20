using System.Diagnostics;
using DocSign.Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace DocSign.Api.Middleware;

public sealed class GlobalExceptionHandler(
    IProblemDetailsService problemDetailsService,
    ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        ProblemDetails problem = exception switch
        {
            ValidationApiException v => new ValidationProblemDetails(v.FieldErrors)
            {
                Status = StatusCodes.Status400BadRequest,
                Title = v.Title,
                Detail = "See the errors property for details."
            },
            ApiException api => new ProblemDetails
            {
                Status = (int)api.Status,
                Title = api.Title,
                Detail = api.Message
            },
            _ => new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "An unexpected error occurred",
                Detail = "Contact support with the trace ID."
            }
        };

        problem.Instance = httpContext.Request.Path;

        if (problem.Status >= 500)
            logger.LogError(exception, "Unhandled exception. TraceId: {TraceId}",
                Activity.Current?.TraceId.ToString() ?? httpContext.TraceIdentifier);

        httpContext.Response.StatusCode = problem.Status!.Value;

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = problem
        });
    }
}
