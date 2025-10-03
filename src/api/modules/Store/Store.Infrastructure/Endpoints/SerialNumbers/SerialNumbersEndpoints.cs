using Store.Infrastructure.Endpoints.SerialNumbers.v1;

namespace Store.Infrastructure.Endpoints.SerialNumbers;

/// <summary>
/// Endpoint configuration for Serial Numbers module.
/// </summary>
public static class SerialNumbersEndpoints
{
    /// <summary>
    /// Maps all Serial Numbers endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapSerialNumbersEndpoints(this IEndpointRouteBuilder app)
    {
        var serialNumbersGroup = app.MapGroup("/serialnumbers")
            .WithTags("SerialNumbers")
            .WithDescription("Endpoints for managing serial numbers");

        // Version 1 endpoints
        serialNumbersGroup.MapCreateSerialNumberEndpoint();
        serialNumbersGroup.MapUpdateSerialNumberEndpoint();
        serialNumbersGroup.MapDeleteSerialNumberEndpoint();
        serialNumbersGroup.MapGetSerialNumberEndpoint();
        serialNumbersGroup.MapSearchSerialNumbersEndpoint();

        return app;
    }
}
