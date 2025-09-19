using Accounting.Application.Budgets.Search;
using Accounting.Application.Budgets.Responses;

namespace Accounting.Infrastructure.Endpoints.Budgets.v1;

public static class BudgetSearchEndpoint
{
    internal static RouteHandlerBuilder MapBudgetSearchEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] SearchBudgetsQuery command) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(BudgetSearchEndpoint))
            .WithSummary("Gets a list of budgets")
            .WithDescription("Gets a list of budgets with pagination and filtering support")
            .Produces<PagedList<BudgetListItemResponse>>()
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}
