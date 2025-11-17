using Accounting.Application.Budgets.Update;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Budgets.v1;

public static class BudgetUpdateEndpoint
{
    internal static RouteHandlerBuilder MapBudgetUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (DefaultIdType id, UpdateBudgetCommand request, ISender mediator) =>
            {
                var command = request with { Id = id };
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(BudgetUpdateEndpoint))
            .WithSummary("update a budget")
            .WithDescription("update a budget")
            .Produces<UpdateBudgetResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}
