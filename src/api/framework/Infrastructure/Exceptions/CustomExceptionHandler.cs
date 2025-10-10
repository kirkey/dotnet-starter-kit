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
            case FluentValidation.ValidationException fluentException:
                {
                    problemDetails.Detail = "one or more validation errors occurred";
                    problemDetails.Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
                    httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                    List<string> validationErrors = [];
                    foreach (var error in fluentException.Errors)
                    {
                        validationErrors.Add(error.ErrorMessage);
                    }
                    problemDetails.Extensions.Add("errors", validationErrors);
                    break;
                }
            case FshException e:
                {
                    httpContext.Response.StatusCode = (int)e.StatusCode;
                    problemDetails.Detail = e.Message;
                    if (e.ErrorMessages.Any())
                    {
                        problemDetails.Extensions.Add("errors", e.ErrorMessages);
                    }

                    break;
                }
            default:
                problemDetails.Detail = exception.Message;
                break;
        }

        LogContext.PushProperty("StackTrace", exception.StackTrace);
        logger.LogError("{ProblemDetail}", problemDetails.Detail);
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken).ConfigureAwait(false);
        return true;
    }
}
