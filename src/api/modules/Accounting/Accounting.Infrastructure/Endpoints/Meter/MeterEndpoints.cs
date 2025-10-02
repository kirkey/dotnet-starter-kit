namespace Accounting.Infrastructure.Endpoints.Meter;

/// <summary>
/// Endpoint configuration for Meter module.
/// </summary>
public static class MeterEndpoints
{
    /// <summary>
    /// Maps all Meter endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapMeterEndpoints(this IEndpointRouteBuilder app)
    {
        var meterGroup = app.MapGroup("/meters")
            .WithTags("Meters")
            .WithDescription("Endpoints for managing meter readings and data");

        // Version 1 endpoints will be added here when implemented
        // meterGroup.MapMeterCreateEndpoint();
        // meterGroup.MapMeterUpdateEndpoint();
        // meterGroup.MapMeterDeleteEndpoint();
        // meterGroup.MapMeterGetEndpoint();
        // meterGroup.MapMeterSearchEndpoint();

        return app;
    }
}
