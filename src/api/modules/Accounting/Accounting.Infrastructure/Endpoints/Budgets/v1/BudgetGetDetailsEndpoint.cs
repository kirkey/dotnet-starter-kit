using Accounting.Application.Budgets.BudgetDetails.Commands;
using Accounting.Application.Budgets.Responses;

namespace Accounting.Infrastructure.Endpoints.Budgets.v1;

public static class BudgetGetDetailsEndpoint
{
    internal static RouteHandlerBuilder MapBudgetGetLinesEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/details/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new BudgetGetDetailsCommand(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(BudgetGetDetailsEndpoint))
            .WithSummary("get budget details for a budget")
            .WithDescription("Get all budget details for the specified budget (non-paginated)")
            .Produces<List<BudgetDetailResponse>>()
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}

