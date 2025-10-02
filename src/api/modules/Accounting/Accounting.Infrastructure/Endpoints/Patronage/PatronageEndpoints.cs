namespace Accounting.Infrastructure.Endpoints.Patronage;

/// <summary>
/// Endpoint configuration for Patronage module.
/// </summary>
public static class PatronageEndpoints
{
    /// <summary>
    /// Maps all Patronage endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapPatronageEndpoints(this IEndpointRouteBuilder app)
    {
        var patronageGroup = app.MapGroup("/patronage")
            .WithTags("Patronage")
            .WithDescription("Endpoints for managing patronage distributions and calculations");

        // Version 1 endpoints will be added here when implemented
        // patronageGroup.MapPatronageCreateEndpoint();
        // patronageGroup.MapPatronageUpdateEndpoint();
        // patronageGroup.MapPatronageDeleteEndpoint();
        // patronageGroup.MapPatronageGetEndpoint();
        // patronageGroup.MapPatronageSearchEndpoint();

        return app;
    }
}
