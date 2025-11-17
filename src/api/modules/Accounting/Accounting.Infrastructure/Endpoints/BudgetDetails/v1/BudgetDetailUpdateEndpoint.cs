using Accounting.Application.Budgets.Details.Update;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.BudgetDetails.v1;

public static class BudgetDetailUpdateEndpoint
{
    internal static RouteHandlerBuilder MapBudgetDetailUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("{id:guid}", async (DefaultIdType id, UpdateBudgetDetailCommand request, ISender mediator) =>
            {
                var command = request with { Id = id };
                var updatedId = await mediator.Send(command);
                return Results.Ok(updatedId);
            })
            .WithName(nameof(BudgetDetailUpdateEndpoint))
            .WithSummary("update budget detail")
            .Produces<DefaultIdType>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

