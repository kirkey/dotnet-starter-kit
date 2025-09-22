using Accounting.Application.BudgetDetails.Responses;
using Accounting.Application.BudgetDetails.Search;

namespace Accounting.Infrastructure.Endpoints.BudgetDetails.v1;

public static class BudgetDetailSearchEndpoint
{
    internal static RouteHandlerBuilder MapBudgetDetailSearchEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("by-budget/{budgetId:guid}", async (DefaultIdType budgetId, ISender mediator) =>
            {
                var list = await mediator.Send(new SearchBudgetDetailsByBudgetIdQuery(budgetId));
                return Results.Ok(list);
            })
            .WithName(nameof(BudgetDetailSearchEndpoint))
            .WithSummary("list budget details by budget id")
            .Produces<List<BudgetDetailResponse>>()
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}
