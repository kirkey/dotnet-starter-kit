using Accounting.Application.CostCenters.UpdateBudget.v1;

namespace Accounting.Infrastructure.Endpoints.CostCenters.v1;

public static class CostCenterUpdateBudgetEndpoint
{
    internal static RouteHandlerBuilder MapCostCenterUpdateBudgetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}/budget", async (DefaultIdType id, UpdateBudgetCommand command, ISender mediator) =>
            {
                if (id != command.Id) return Results.BadRequest("ID in URL does not match ID in request body.");
                var costCenterId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = costCenterId, Message = "Budget updated successfully" });
            })
            .WithName(nameof(CostCenterUpdateBudgetEndpoint))
            .WithSummary("Update budget")
            .WithDescription("Updates the budget allocation for a cost center")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Accounting.Update")
            .MapToApiVersion(1);
    }
}

