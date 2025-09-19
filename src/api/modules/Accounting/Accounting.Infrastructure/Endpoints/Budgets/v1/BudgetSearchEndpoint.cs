using Accounting.Application.Budgets.Search;
using FSH.Starter.Blazor.Infrastructure.Api;
using BudgetDto = Accounting.Application.Budgets.Dtos.BudgetDto;

namespace Accounting.Infrastructure.Endpoints.Budgets.v1;

public static class BudgetSearchEndpoint
{
    internal static RouteHandlerBuilder MapBudgetSearchEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] SearchBudgetsRequest command) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(BudgetSearchEndpoint))
            .WithSummary("Gets a list of budgets")
            .WithDescription("Gets a list of budgets with pagination and filtering support")
            .Produces<PagedList<BudgetDto>>()
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}
