using Accounting.Application.CostCenters.UpdateBudget.v1;

namespace Accounting.Infrastructure.Endpoints.CostCenters.v1;

public static class UpdateBudgetEndpoint
{
    internal static RouteHandlerBuilder MapUpdateBudgetEndpoint(this IEndpointRouteBuilder app)
    {
        return app.MapPut("/{id:guid}/budget", async (DefaultIdType id, UpdateBudgetCommand command, ISender mediator) =>
            {
                if (id != command.Id) return Results.BadRequest();
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
}
