namespace FSH.Framework.Infrastructure.OpenApi;

/// <summary>
/// Configuration options for Swagger/OpenAPI documentation.
/// </summary>
public class SwaggerOptions
{
    /// <summary>
    /// Gets or sets a value indicating whether Swagger UI should be enabled.
    /// Default is false for production environments.
    /// </summary>
    public bool Enable { get; set; }
}

