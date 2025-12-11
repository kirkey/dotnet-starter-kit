using Accounting.Infrastructure.Endpoints.CostCenters.v1;
using Carter;

namespace Accounting.Infrastructure.Endpoints.CostCenters;

/// <summary>
/// Endpoint configuration for CostCenters module.
/// Provides comprehensive REST API endpoints for managing cost-centers.
/// Uses the ICarterModule delegated pattern with extension methods for each operation.
/// </summary>
public class CostCentersEndpoints() : CarterModule
{
    /// <summary>
    /// Maps all CostCenters endpoints to the route builder.
    /// Delegates to extension methods for Create, Read, Update, Delete, and business operation endpoints.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/cost-centers").WithTags("cost-center");

        group.MapCostCenterActivateEndpoint();
        group.MapCostCenterCreateEndpoint();
        group.MapCostCenterDeactivateEndpoint();
        group.MapCostCenterDeleteEndpoint();
        group.MapCostCenterGetEndpoint();
        group.MapCostCenterRecordActualEndpoint();
        group.MapCostCenterSearchEndpoint();
        group.MapCostCenterUpdateEndpoint();
        group.MapUpdateCostCenterBudgetEndpoint();
        group.MapCostCenterDashboardEndpoint();
    }
}
