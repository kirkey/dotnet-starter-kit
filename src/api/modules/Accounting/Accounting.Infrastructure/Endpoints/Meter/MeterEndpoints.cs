using Accounting.Infrastructure.Endpoints.Meter.v1;

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
            .WithDescription("Endpoints for managing meters")
            .MapToApiVersion(1);

        // CRUD operations
        meterGroup.MapMeterCreateEndpoint();
        meterGroup.MapMeterGetEndpoint();
        meterGroup.MapMeterUpdateEndpoint();
        meterGroup.MapMeterDeleteEndpoint();
        meterGroup.MapMeterSearchEndpoint();

        return app;
    }
}
