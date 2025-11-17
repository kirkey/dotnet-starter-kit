using Accounting.Application.Budgets.Create;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Budgets.v1;

public static class BudgetCreateEndpoint
{
    internal static RouteHandlerBuilder MapBudgetCreateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateBudgetCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(BudgetCreateEndpoint))
            .WithSummary("create a budget")
            .WithDescription("create a budget")
            .Produces<CreateBudgetResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}
