using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FSH.Framework.Infrastructure.OpenApi;

/// <summary>
/// Document filter to log detailed information about OpenAPI generation errors.
/// </summary>
public class OpenApiErrorLoggingFilter(ILogger<OpenApiErrorLoggingFilter> logger) : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        // Log all registered endpoints for debugging
        int apiDescriptionCount = context.ApiDescriptions.Count();
        logger.LogInformation("OpenAPI Document Generation - Processing {Count} API descriptions", apiDescriptionCount);

        foreach (var apiDescription in context.ApiDescriptions)
        {
            try
            {
                string httpMethod = !string.IsNullOrEmpty(apiDescription.HttpMethod) 
                    ? apiDescription.HttpMethod.ToUpperInvariant() 
                    : "UNKNOWN";
                string relativePath = apiDescription.RelativePath ?? "UNKNOWN";
                var actionDescriptor = apiDescription.ActionDescriptor;
                
                logger.LogDebug(
                    "Processing endpoint: {Method} {Path} - DisplayName: {DisplayName}",
                    httpMethod,
                    relativePath,
                    actionDescriptor.DisplayName);

                // Check for parameter binding issues
                foreach (var parameter in apiDescription.ParameterDescriptions)
                {
                    if (parameter.Type == null)
                    {
                        logger.LogWarning(
                            "Parameter '{ParameterName}' in endpoint {Method} {Path} has null Type. Source: {Source}",
                            parameter.Name,
                            httpMethod,
                            relativePath,
                            parameter.Source?.Id);
                    }
                }

                // Check for response type issues
                foreach (var response in apiDescription.SupportedResponseTypes)
                {
                    if (response.Type == null && response.StatusCode != 204) // 204 No Content is expected to have null type
                    {
                        logger.LogWarning(
                            "Response type is null for endpoint {Method} {Path} with status code {StatusCode}",
                            httpMethod,
                            relativePath,
                            response.StatusCode);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Error while processing API description for {Method} {Path}. Error: {ErrorMessage}. InnerException: {InnerException}. StackTrace: {StackTrace}",
                    apiDescription.HttpMethod,
                    apiDescription.RelativePath,
                    ex.Message,
                    ex.InnerException?.Message,
                    ex.StackTrace);
            }
        }

        // Validate the generated document
        try
        {
            if (swaggerDoc.Paths == null || swaggerDoc.Paths.Count == 0)
            {
                logger.LogWarning("OpenAPI document has no paths defined");
            }
            else
            {
                logger.LogInformation("OpenAPI document generated successfully with {PathCount} paths", swaggerDoc.Paths.Count);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Error validating OpenAPI document: {ErrorMessage}. InnerException: {InnerException}. StackTrace: {StackTrace}",
                ex.Message,
                ex.InnerException?.Message,
                ex.StackTrace);
        }
    }
}

