using Accounting.Infrastructure.Endpoints.DeferredRevenues.v1;

namespace Accounting.Infrastructure.Endpoints.DeferredRevenues;

/// <summary>
/// Endpoint configuration for Deferred Revenue module.
/// </summary>
public static class DeferredRevenuesEndpoints
{
    /// <summary>
    /// Maps all Deferred Revenue endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapDeferredRevenuesEndpoints(this IEndpointRouteBuilder app)
    {
        var deferredRevenuesGroup = app.MapGroup("/deferred-revenues")
            .WithTags("Deferred-Revenues")
            .WithDescription("Endpoints for managing deferred revenue entries");

        // CRUD operations
        deferredRevenuesGroup.MapDeferredRevenueCreateEndpoint();
        deferredRevenuesGroup.MapDeferredRevenueGetEndpoint();
        deferredRevenuesGroup.MapDeferredRevenueUpdateEndpoint();
        deferredRevenuesGroup.MapDeferredRevenueDeleteEndpoint();
        deferredRevenuesGroup.MapDeferredRevenueSearchEndpoint();

        // Workflow operations
        deferredRevenuesGroup.MapDeferredRevenueRecognizeEndpoint();

        return app;
    }
}

