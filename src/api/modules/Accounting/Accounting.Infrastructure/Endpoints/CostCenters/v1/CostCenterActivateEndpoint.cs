using Accounting.Application.CostCenters.Activate.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.CostCenters.v1;

public static class CostCenterActivateEndpoint
{
    internal static RouteHandlerBuilder MapCostCenterActivateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id}/activate", async (DefaultIdType id, ISender mediator) =>
            {
                var costCenterId = await mediator.Send(new ActivateCostCenterCommand(id)).ConfigureAwait(false);
                return Results.Ok(new { Id = costCenterId, Message = "Cost center activated successfully" });
            })
            .WithName(nameof(CostCenterActivateEndpoint))
            .WithSummary("Activate cost center")
            .WithDescription("Activates a cost center for use")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

