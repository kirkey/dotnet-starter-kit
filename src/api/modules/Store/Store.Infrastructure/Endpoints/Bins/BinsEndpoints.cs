using Store.Infrastructure.Endpoints.Bins.v1;

namespace Store.Infrastructure.Endpoints.Bins;

/// <summary>
/// Endpoint configuration for Bins module.
/// </summary>
public static class BinsEndpoints
{
    /// <summary>
    /// Maps all Bins endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapBinsEndpoints(this IEndpointRouteBuilder app)
    {
        var binsGroup = app.MapGroup("/bins")
            .WithTags("Bins")
            .WithDescription("Endpoints for managing storage bins");

        // Version 1 endpoints
        binsGroup.MapCreateBinEndpoint();
        binsGroup.MapUpdateBinEndpoint();
        binsGroup.MapDeleteBinEndpoint();
        binsGroup.MapGetBinEndpoint();
        binsGroup.MapSearchBinsEndpoint();

        return app;
    }
}
