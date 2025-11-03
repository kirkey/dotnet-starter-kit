using Microsoft.AspNetCore.Diagnostics;
using Serilog.Context;

namespace FSH.Framework.Infrastructure.Exceptions;

public class CustomExceptionHandler(ILogger<CustomExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(httpContext);
        ArgumentNullException.ThrowIfNull(exception);
        var problemDetails = new ProblemDetails { Instance = httpContext.Request.Path };

        switch (exception)
        {
            case ValidationException fluentException:
                {
                    problemDetails.Status = StatusCodes.Status400BadRequest;
                    problemDetails.Detail = "One or more validation errors occurred";
                    problemDetails.Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
                    problemDetails.Title = "Validation Error";
                    httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                    httpContext.Response.ContentType = "application/problem+json";
                    
                    // Group errors by property name for better readability
                    var errorsDictionary = fluentException.Errors
                        .GroupBy(x => x.PropertyName)
                        .ToDictionary(
                            g => g.Key,
                            g => g.Select(x => x.ErrorMessage).ToArray()
                        );
                    
                    problemDetails.Extensions.Add("errors", errorsDictionary);
                    
                    // Log validation errors for debugging
                    if (errorsDictionary.Count > 0)
                    {
                        foreach ((string? property, string[] errors) in errorsDictionary)
                        {
                            logger.LogWarning("Validation Error - {Property}: {Errors}", property, string.Join(", ", errors));
                        }
                    }
                    break;
                }
            case FshException e:
                {
                    httpContext.Response.StatusCode = (int)e.StatusCode;
                    httpContext.Response.ContentType = "application/problem+json";
                    problemDetails.Status = (int)e.StatusCode;
                    problemDetails.Detail = e.Message;
                    problemDetails.Title = "Application Error";
                    if (e.ErrorMessages.Any())
                    {
                        problemDetails.Extensions.Add("errors", e.ErrorMessages);
                    }

                    break;
                }
            default:
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                httpContext.Response.ContentType = "application/problem+json";
                problemDetails.Status = StatusCodes.Status500InternalServerError;
                problemDetails.Title = "Internal Server Error";
                problemDetails.Detail = exception.Message;
                break;
        }

        LogContext.PushProperty("StackTrace", exception.StackTrace);
        logger.LogError("{ProblemDetail}", problemDetails.Detail);
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken).ConfigureAwait(false);
        return true;
    }
}
