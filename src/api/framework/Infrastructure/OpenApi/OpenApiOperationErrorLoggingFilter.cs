using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FSH.Framework.Infrastructure.OpenApi;

/// <summary>
/// Operation filter to log detailed information about OpenAPI operation generation errors.
/// This helps diagnose issues with specific endpoints that fail during Swagger generation.
/// </summary>
public class OpenApiOperationErrorLoggingFilter(ILogger<OpenApiOperationErrorLoggingFilter> logger) : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        try
        {
            string httpMethod = !string.IsNullOrEmpty(context.ApiDescription.HttpMethod)
                ? context.ApiDescription.HttpMethod.ToUpperInvariant()
                : "UNKNOWN";
            string relativePath = context.ApiDescription.RelativePath ?? "UNKNOWN";
            var actionDescriptor = context.ApiDescription.ActionDescriptor;

            logger.LogDebug(
                "Generating OpenAPI operation: {Method} {Path} - {DisplayName}",
                httpMethod,
                relativePath,
                actionDescriptor.DisplayName);

            // Validate parameters
            if (operation.Parameters != null)
            {
                foreach (var parameter in operation.Parameters)
                {
                    if (parameter.Schema == null)
                    {
                        logger.LogWarning(
                            "Parameter '{ParameterName}' in {Method} {Path} has null Schema",
                            parameter.Name,
                            httpMethod,
                            relativePath);
                    }
                }
            }

            // Validate request body
            if (operation.RequestBody != null)
            {
                foreach (var content in operation.RequestBody.Content)
                {
                    if (content.Value.Schema == null)
                    {
                        logger.LogWarning(
                            "Request body for {Method} {Path} with content type '{ContentType}' has null Schema",
                            httpMethod,
                            relativePath,
                            content.Key);
                    }
                    else if (content.Value.Schema.Reference == null && content.Value.Schema.Type == null)
                    {
                        logger.LogWarning(
                            "Request body schema for {Method} {Path} with content type '{ContentType}' has no Reference or Type defined. This may indicate a problem with type resolution.",
                            httpMethod,
                            relativePath,
                            content.Key);
                    }
                }
            }

            // Validate responses
            if (operation.Responses != null)
            {
                foreach (var response in operation.Responses)
                {
                    foreach (var content in response.Value.Content)
                    {
                        if (content.Value.Schema == null)
                        {
                            logger.LogWarning(
                                "Response schema for {Method} {Path} with status {StatusCode} and content type '{ContentType}' is null",
                                httpMethod,
                                relativePath,
                                response.Key,
                                content.Key);
                        }
                    }
                }
            }

            // Check for API description parameters that might cause issues
            foreach (var paramDesc in context.ApiDescription.ParameterDescriptions)
            {
                if (paramDesc.Type == null)
                {
                    logger.LogError(
                        "CRITICAL: Parameter '{ParameterName}' in {Method} {Path} has null Type. " +
                        "Source: {Source}, IsRequired: {IsRequired}, DefaultValue: {DefaultValue}. " +
                        "This will cause OpenAPI generation to fail!",
                        paramDesc.Name,
                        httpMethod,
                        relativePath,
                        paramDesc.Source?.Id,
                        paramDesc.IsRequired,
                        paramDesc.DefaultValue);
                }
                
                // Check for complex types in route/query parameters
                if (paramDesc.Type != null && 
                    (paramDesc.Source?.Id == "Path" || paramDesc.Source?.Id == "Query") &&
                    !IsSimpleType(paramDesc.Type))
                {
                    logger.LogWarning(
                        "Parameter '{ParameterName}' in {Method} {Path} is bound from {Source} but has complex type {Type}. " +
                        "This might cause serialization issues.",
                        paramDesc.Name,
                        httpMethod,
                        relativePath,
                        paramDesc.Source?.Id,
                        paramDesc.Type.Name);
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "CRITICAL ERROR generating OpenAPI operation for {Method} {Path}. " +
                "Error: {ErrorMessage}. " +
                "InnerException: {InnerException}. " +
                "InnerExceptionType: {InnerExceptionType}",
                context.ApiDescription.HttpMethod,
                context.ApiDescription.RelativePath,
                ex.Message,
                ex.InnerException?.Message,
                ex.InnerException?.GetType().FullName);

            // Log inner exception stack trace for better debugging
            if (ex.InnerException != null)
            {
                logger.LogError(
                    "Inner Exception Stack Trace: {StackTrace}",
                    ex.InnerException.StackTrace);
            }
            
            // Re-throw to maintain the original behavior
            throw;
        }
    }

    /// <summary>
    /// Determines if a type is a simple type that can be bound from route/query parameters.
    /// </summary>
    private static bool IsSimpleType(Type type)
    {
        return type.IsPrimitive
               || type.IsEnum
               || type == typeof(string)
               || type == typeof(decimal)
               || type == typeof(DateTime)
               || type == typeof(DateTimeOffset)
               || type == typeof(TimeSpan)
               || type == typeof(DefaultIdType)
               || Nullable.GetUnderlyingType(type) != null;
    }
}

