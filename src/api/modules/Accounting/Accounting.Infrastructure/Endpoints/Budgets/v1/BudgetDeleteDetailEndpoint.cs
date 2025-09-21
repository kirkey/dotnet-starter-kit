using Accounting.Application.Budgets.BudgetDetails.Commands;

namespace Accounting.Infrastructure.Endpoints.Budgets.v1;

public static class BudgetDeleteDetailEndpoint
{
    internal static RouteHandlerBuilder MapBudgetDeleteLineEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}/lines/{accountId:guid}", async (DefaultIdType id, DefaultIdType accountId, ISender mediator) =>
            {
                var cmd = new DeleteBudgetDetailCommand { BudgetId = id, AccountId = accountId };
                await mediator.Send(cmd).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName(nameof(BudgetDeleteDetailEndpoint))
            .WithSummary("delete a budget line")
            .WithDescription("Delete a budget line for a given budget and account")
            .RequirePermission("Permissions.Accounting.Delete")
            .MapToApiVersion(1);
    }
}

