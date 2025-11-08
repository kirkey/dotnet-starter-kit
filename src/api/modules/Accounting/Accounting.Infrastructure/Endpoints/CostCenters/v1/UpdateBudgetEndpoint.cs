using Accounting.Application.CostCenters.UpdateBudget.v1;

namespace Accounting.Infrastructure.Endpoints.CostCenters.v1;

public static class UpdateBudgetEndpoint
{
    internal static RouteHandlerBuilder MapUpdateBudgetEndpoint(this IEndpointRouteBuilder app)
    {
        return app.MapPut("/{id:guid}/budget", async ([FromRoute] DefaultIdType id, [FromBody] UpdateBudgetRequest request, ISender mediator) =>
            {
                var command = new UpdateBudgetCommand(id, request.BudgetAmount);
                var costCenterId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = costCenterId });
            })
            .WithName(nameof(UpdateBudgetEndpoint))
            .WithSummary("update cost center budget")
            .WithDescription("updates the budget allocation for a cost center")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Accounting.Update")
            .MapToApiVersion(1);
    }

    public sealed record UpdateBudgetRequest(decimal BudgetAmount);
}


