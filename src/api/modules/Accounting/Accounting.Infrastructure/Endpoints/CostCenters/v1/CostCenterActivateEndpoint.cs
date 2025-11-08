using Accounting.Application.CostCenters.Activate.v1;

namespace Accounting.Infrastructure.Endpoints.CostCenters.v1;

public static class CostCenterActivateEndpoint
{
    internal static RouteHandlerBuilder MapCostCenterActivateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/activate", async (DefaultIdType id, ISender mediator) =>
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
            .RequirePermission("Permissions.Accounting.Update")
            .MapToApiVersion(1);
    }
}

