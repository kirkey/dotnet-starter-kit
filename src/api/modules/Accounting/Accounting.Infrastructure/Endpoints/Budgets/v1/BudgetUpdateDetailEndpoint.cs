using Accounting.Application.Budgets.BudgetDetails.Commands;

namespace Accounting.Infrastructure.Endpoints.Budgets.v1;

public static class BudgetUpdateDetailEndpoint
{
    internal static RouteHandlerBuilder MapBudgetUpdateLineEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}/details/{accountId:guid}", async (DefaultIdType id, DefaultIdType accountId, UpdateBudgetDetailCommand request, ISender mediator) =>
            {
                if (id != request.BudgetId || accountId != request.AccountId) return Results.BadRequest();
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(BudgetUpdateDetailEndpoint))
            .WithSummary("update a budget line")
            .WithDescription("Update a budget line for a given budget and account")
            .Produces<DefaultIdType>()
            .RequirePermission("Permissions.Accounting.Update")
            .MapToApiVersion(1);
    }
}

