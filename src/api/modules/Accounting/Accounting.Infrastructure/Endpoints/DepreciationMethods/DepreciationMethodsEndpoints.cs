using Accounting.Infrastructure.Endpoints.DepreciationMethods.v1;
using Carter;

namespace Accounting.Infrastructure.Endpoints.DepreciationMethods;

/// <summary>
/// Endpoint configuration for DepreciationMethods module.
/// Provides comprehensive REST API endpoints for managing depreciation-methods.
/// Uses the ICarterModule delegated pattern with extension methods for each operation.
/// </summary>
public class DepreciationMethodsEndpoints : ICarterModule
{
    /// <summary>
    /// Maps all DepreciationMethods endpoints to the route builder.
    /// Delegates to extension methods for Create, Read, Update, Delete, and business operation endpoints.
    /// </summary>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/depreciation-methods").WithTags("depreciation-method");

        group.MapDepreciationMethodActivateEndpoint();
        group.MapDepreciationMethodCreateEndpoint();
        group.MapDepreciationMethodDeactivateEndpoint();
        group.MapDepreciationMethodDeleteEndpoint();
        group.MapDepreciationMethodGetEndpoint();
        group.MapDepreciationMethodSearchEndpoint();
        group.MapDepreciationMethodUpdateEndpoint();
    }
}
