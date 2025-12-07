using Accounting.Infrastructure.Endpoints.FixedAssets.v1;
using Carter;

namespace Accounting.Infrastructure.Endpoints.FixedAssets;

/// <summary>
/// Endpoint configuration for FixedAssets module.
/// Provides comprehensive REST API endpoints for managing fixed-assets.
/// Uses the ICarterModule delegated pattern with extension methods for each operation.
/// </summary>
public class FixedAssetsEndpoints() : CarterModule
{
    /// <summary>
    /// Maps all FixedAssets endpoints to the route builder.
    /// Delegates to extension methods for Create, Read, Update, Delete, and business operation endpoints.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/fixed-assets").WithTags("fixed-asset");

        group.MapFixedAssetApproveEndpoint();
        group.MapFixedAssetCreateEndpoint();
        group.MapFixedAssetDeleteEndpoint();
        group.MapFixedAssetDepreciateEndpoint();
        group.MapFixedAssetDisposeEndpoint();
        group.MapFixedAssetGetEndpoint();
        group.MapFixedAssetRejectEndpoint();
        group.MapFixedAssetSearchEndpoint();
        group.MapFixedAssetUpdateEndpoint();
        group.MapFixedAssetUpdateMaintenanceEndpoint();
    }
}
