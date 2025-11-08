using Accounting.Infrastructure.Endpoints.CostCenters.v1;

namespace Accounting.Infrastructure.Endpoints.CostCenters;

public static class CostCentersEndpoints
{
    internal static IEndpointRouteBuilder MapCostCentersEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/cost-centers")
            .WithTags("Cost Centers")
            .WithDescription("Endpoints for managing cost centers")
            .MapToApiVersion(1);

        // CRUD operations
        group.MapCostCenterCreateEndpoint();
        group.MapCostCenterGetEndpoint();
        group.MapCostCenterUpdateEndpoint();
        group.MapCostCenterSearchEndpoint();

        // Workflow operations
        group.MapCostCenterUpdateBudgetEndpoint();
        group.MapCostCenterRecordActualEndpoint();
        group.MapCostCenterActivateEndpoint();
        group.MapCostCenterDeactivateEndpoint();

        return app;
    }
}

