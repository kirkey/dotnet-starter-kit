using Accounting.Application.Budgets.Approve;

namespace Accounting.Infrastructure.Endpoints.Budgets.v1;

public static class BudgetApproveEndpoint
{
    internal static RouteHandlerBuilder MapBudgetApproveEndpoint(this IEndpointRouteBuilder endpoints)
        => endpoints
            .MapPost("/{id}/approve", async (DefaultIdType id, ApproveBudgetCommand command, ISender mediator) =>
            {
                if (id != command.BudgetId) return Results.BadRequest("ID in URL does not match ID in request body.");
                var result = await mediator.Send(command).ConfigureAwait(false);
                return TypedResults.Ok(result);
            })
            .WithName(nameof(BudgetApproveEndpoint))
            .WithSummary("Approve budget")
            .WithDescription("Approves a budget for activation")
            .Produces<DefaultIdType>();
}

