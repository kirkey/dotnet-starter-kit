using Accounting.Application.Budgets.Close;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Budgets.v1;

public static class BudgetCloseEndpoint
{
    internal static RouteHandlerBuilder MapBudgetCloseEndpoint(this IEndpointRouteBuilder endpoints)
        => endpoints
            .MapPost("/{id}/close", async (DefaultIdType id, CloseBudgetCommand command, ISender mediator) =>
            {
                if (id != command.BudgetId) return Results.BadRequest("ID in URL does not match ID in request body.");
                var result = await mediator.Send(command).ConfigureAwait(false);
                return TypedResults.Ok(result);
            })
            .WithName(nameof(BudgetCloseEndpoint))
            .WithSummary("Close budget")
            .WithDescription("Closes a budget")
            .Produces<DefaultIdType>()
            .RequirePermission(FshPermission.NameFor(FshActions.Complete, FshResources.Accounting))
            .MapToApiVersion(1);
}

