using Accounting.Infrastructure.Endpoints.FixedAssets.v1;

namespace Accounting.Infrastructure.Endpoints.FixedAssets;

/// <summary>
/// Endpoint configuration for Fixed Assets module.
/// </summary>
public static class FixedAssetsEndpoints
{
    /// <summary>
    /// Maps all Fixed Assets endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapFixedAssetsEndpoints(this IEndpointRouteBuilder app)
    {
        var fixedAssetsGroup = app.MapGroup("/fixed-assets")
            .WithTags("Fixed-Assets")
            .WithDescription("Endpoints for managing fixed assets");

        // CRUD operations
        fixedAssetsGroup.MapFixedAssetCreateEndpoint();
        fixedAssetsGroup.MapFixedAssetGetEndpoint();
        fixedAssetsGroup.MapFixedAssetUpdateEndpoint();
        fixedAssetsGroup.MapFixedAssetDeleteEndpoint();
        fixedAssetsGroup.MapFixedAssetSearchEndpoint();

        // Workflow operations
        fixedAssetsGroup.MapFixedAssetDepreciateEndpoint();
        fixedAssetsGroup.MapFixedAssetDisposeEndpoint();
        fixedAssetsGroup.MapFixedAssetUpdateMaintenanceEndpoint();

        return app;
    }
}
