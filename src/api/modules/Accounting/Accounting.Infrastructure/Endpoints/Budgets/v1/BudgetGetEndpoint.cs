using Accounting.Application.Budgets.Responses;
using Accounting.Application.Budgets.Get;

namespace Accounting.Infrastructure.Endpoints.Budgets.v1;

public static class BudgetGetEndpoint
{
    internal static RouteHandlerBuilder MapBudgetGetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetBudgetQuery(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(BudgetGetEndpoint))
            .WithSummary("get a budget by id")
            .WithDescription("get a budget by id")
            .Produces<BudgetResponse>()
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}
