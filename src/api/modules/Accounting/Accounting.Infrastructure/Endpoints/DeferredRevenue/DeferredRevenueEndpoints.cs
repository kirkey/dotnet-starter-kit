using Accounting.Infrastructure.Endpoints.DeferredRevenue.v1;

namespace Accounting.Infrastructure.Endpoints.DeferredRevenue;

/// <summary>
/// Endpoint configuration for Deferred Revenue module.
/// </summary>
public static class DeferredRevenueEndpoints
{
    /// <summary>
    /// Maps all Deferred Revenue endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapDeferredRevenueEndpoints(this IEndpointRouteBuilder app)
    {
        var deferredRevenueGroup = app.MapGroup("/deferred-revenue")
            .WithTags("Deferred-Revenue")
            .WithDescription("Endpoints for managing deferred revenue entries");

        // CRUD operations
        deferredRevenueGroup.MapDeferredRevenueCreateEndpoint();
        deferredRevenueGroup.MapDeferredRevenueGetEndpoint();
        deferredRevenueGroup.MapDeferredRevenueUpdateEndpoint();
        deferredRevenueGroup.MapDeferredRevenueDeleteEndpoint();
        deferredRevenueGroup.MapDeferredRevenueSearchEndpoint();

        // Workflow operations
        deferredRevenueGroup.MapDeferredRevenueRecognizeEndpoint();

        return app;
    }
}
