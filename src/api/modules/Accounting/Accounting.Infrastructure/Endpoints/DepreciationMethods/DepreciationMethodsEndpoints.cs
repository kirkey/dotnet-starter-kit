using Accounting.Infrastructure.Endpoints.DepreciationMethods.v1;

namespace Accounting.Infrastructure.Endpoints.DepreciationMethods;

/// <summary>
/// Endpoint configuration for Depreciation Methods module.
/// </summary>
public static class DepreciationMethodsEndpoints
{
    /// <summary>
    /// Maps all Depreciation Methods endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapDepreciationMethodsEndpoints(this IEndpointRouteBuilder app)
    {
        var depreciationMethodsGroup = app.MapGroup("/depreciation-methods")
            .WithTags("Depreciation-Methods")
            .WithDescription("Endpoints for managing depreciation methods");

        // Version 1 endpoints
        depreciationMethodsGroup.MapDepreciationMethodCreateEndpoint();
        depreciationMethodsGroup.MapDepreciationMethodUpdateEndpoint();
        depreciationMethodsGroup.MapDepreciationMethodDeleteEndpoint();
        depreciationMethodsGroup.MapDepreciationMethodGetEndpoint();

        return app;
    }
}
