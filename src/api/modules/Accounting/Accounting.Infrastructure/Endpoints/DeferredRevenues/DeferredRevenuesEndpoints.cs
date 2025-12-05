using Accounting.Infrastructure.Endpoints.DeferredRevenues.v1;
using Carter;

namespace Accounting.Infrastructure.Endpoints.DeferredRevenues;

/// <summary>
/// Endpoint configuration for DeferredRevenues module.
/// Provides comprehensive REST API endpoints for managing deferred-revenues.
/// Uses the ICarterModule delegated pattern with extension methods for each operation.
/// </summary>
public class DeferredRevenuesEndpoints() : CarterModule("accounting")
{
    /// <summary>
    /// Maps all DeferredRevenues endpoints to the route builder.
    /// Delegates to extension methods for Create, Read, Update, Delete, and business operation endpoints.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/deferred-revenues").WithTags("deferred-revenue");

        group.MapDeferredRevenueCreateEndpoint();
        group.MapDeferredRevenueDeleteEndpoint();
        group.MapDeferredRevenueGetEndpoint();
        group.MapDeferredRevenueRecognizeEndpoint();
        group.MapDeferredRevenueSearchEndpoint();
        group.MapDeferredRevenueUpdateEndpoint();
    }
}
