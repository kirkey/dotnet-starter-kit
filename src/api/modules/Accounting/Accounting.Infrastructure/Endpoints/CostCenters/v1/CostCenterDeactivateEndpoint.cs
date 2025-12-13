using Accounting.Application.CostCenters.Deactivate.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.CostCenters.v1;

public static class CostCenterDeactivateEndpoint
{
    internal static RouteHandlerBuilder MapCostCenterDeactivateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id}/deactivate", async (DefaultIdType id, ISender mediator) =>
            {
                var costCenterId = await mediator.Send(new DeactivateCostCenterCommand(id)).ConfigureAwait(false);
                return Results.Ok(new { Id = costCenterId, Message = "Cost center deactivated successfully" });
            })
            .WithName(nameof(CostCenterDeactivateEndpoint))
            .WithSummary("Deactivate cost center")
            .WithDescription("Deactivates a cost center")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))                                                               
            .MapToApiVersion(1);
    }
}

