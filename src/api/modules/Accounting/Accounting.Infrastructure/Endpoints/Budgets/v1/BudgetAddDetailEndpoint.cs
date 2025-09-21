using Accounting.Application.Budgets.BudgetDetails.Commands;

namespace Accounting.Infrastructure.Endpoints.Budgets.v1;

public static class BudgetAddDetailEndpoint
{
    internal static RouteHandlerBuilder MapBudgetAddLineEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/details", async (DefaultIdType id, AddBudgetDetailCommand request, ISender mediator) =>
            {
                if (id != request.BudgetId) return Results.BadRequest();
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(BudgetAddDetailEndpoint))
            .WithSummary("add a budget line to a budget")
            .WithDescription("Add a budget line to a budget identified by id")
            .Produces<DefaultIdType>()
            .RequirePermission("Permissions.Accounting.Create")
            .MapToApiVersion(1);
    }
}
