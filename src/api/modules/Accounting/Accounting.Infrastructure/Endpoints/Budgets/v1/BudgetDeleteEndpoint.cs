using Accounting.Application.Budgets.Delete;

namespace Accounting.Infrastructure.Endpoints.Budgets.v1;

public static class BudgetDeleteEndpoint
{
    internal static RouteHandlerBuilder MapBudgetDeleteEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                await mediator.Send(new DeleteBudgetCommand(id)).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName(nameof(BudgetDeleteEndpoint))
            .WithSummary("delete budget by id")
            .WithDescription("delete budget by id")
            .RequirePermission("Permissions.Accounting.Delete")
            .MapToApiVersion(1);
    }
}
