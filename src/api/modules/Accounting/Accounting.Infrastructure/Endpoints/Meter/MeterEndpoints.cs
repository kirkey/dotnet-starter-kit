using Accounting.Infrastructure.Endpoints.Meter.v1;
using Carter;

namespace Accounting.Infrastructure.Endpoints.Meter;

/// <summary>
/// Endpoint configuration for Meter module.
/// Provides comprehensive REST API endpoints for managing meter.
/// Uses the ICarterModule delegated pattern with extension methods for each operation.
/// </summary>
public class MeterEndpoints : ICarterModule
{
    /// <summary>
    /// Maps all Meter endpoints to the route builder.
    /// Delegates to extension methods for Create, Read, Update, Delete, and business operation endpoints.
    /// </summary>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/meter").WithTags("meter");

        group.MapMeterCreateEndpoint();
        group.MapMeterDeleteEndpoint();
        group.MapMeterGetEndpoint();
        group.MapMeterSearchEndpoint();
        group.MapMeterUpdateEndpoint();
    }
}
