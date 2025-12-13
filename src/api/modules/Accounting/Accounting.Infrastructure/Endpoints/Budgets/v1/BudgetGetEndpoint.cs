using Accounting.Application.Budgets.Get;
using Accounting.Application.Budgets.Responses;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Budgets.v1;

public static class BudgetGetEndpoint
{
    internal static RouteHandlerBuilder MapBudgetGetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetBudgetQuery(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(BudgetGetEndpoint))
            .WithSummary("get a budget by id")
            .WithDescription("get a budget by id")
            .Produces<BudgetResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}
