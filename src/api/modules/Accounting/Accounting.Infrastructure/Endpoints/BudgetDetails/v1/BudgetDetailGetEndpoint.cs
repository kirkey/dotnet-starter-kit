using Accounting.Application.Budgets.Details.Get;
using Accounting.Application.Budgets.Details.Responses;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.BudgetDetails.v1;

public static class BudgetDetailGetEndpoint
{
    internal static RouteHandlerBuilder MapBudgetDetailGetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetBudgetDetailQuery(id));
                return Results.Ok(response);
            })
            .WithName(nameof(BudgetDetailGetEndpoint))
            .WithSummary("get budget detail by id")
            .Produces<BudgetDetailResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

