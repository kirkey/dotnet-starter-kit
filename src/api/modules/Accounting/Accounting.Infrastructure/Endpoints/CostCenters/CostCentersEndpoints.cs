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

        group.MapCostCenterCreateEndpoint();
        group.MapCostCenterGetEndpoint();
        group.MapCostCenterSearchEndpoint();

        return app;
    }
}

