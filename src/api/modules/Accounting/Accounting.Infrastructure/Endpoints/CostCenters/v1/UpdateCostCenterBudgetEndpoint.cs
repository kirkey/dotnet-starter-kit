using Accounting.Application.CostCenters.UpdateBudget.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.CostCenters.v1;

/// <summary>
/// Endpoint for updating a cost center's budget allocation.
/// </summary>
public static class UpdateCostCenterBudgetEndpoint
{
    internal static RouteHandlerBuilder MapUpdateCostCenterBudgetEndpoint(this IEndpointRouteBuilder app)
    {
        return app.MapPut("/{id:guid}/budget", async (DefaultIdType id, UpdateCostCenterBudgetCommand command, ISender mediator) =>
            {
                if (id != command.Id)
                    return Results.BadRequest("ID mismatch");
                    
                var costCenterId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = costCenterId });
            })
            .WithName(nameof(UpdateCostCenterBudgetEndpoint))
            .WithSummary("Update cost center budget")
            .WithDescription("Updates the budget allocation for a cost center")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}
