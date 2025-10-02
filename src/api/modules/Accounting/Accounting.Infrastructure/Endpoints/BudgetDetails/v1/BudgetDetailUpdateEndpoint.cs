using Accounting.Application.Budgets.Details.Update;

namespace Accounting.Infrastructure.Endpoints.BudgetDetails.v1;

public static class BudgetDetailUpdateEndpoint
{
    internal static RouteHandlerBuilder MapBudgetDetailUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("{id:guid}", async (DefaultIdType id, UpdateBudgetDetailCommand request, ISender mediator) =>
            {
                if (id != request.Id) return Results.BadRequest();
                var updatedId = await mediator.Send(request);
                return Results.Ok(updatedId);
            })
            .WithName(nameof(BudgetDetailUpdateEndpoint))
            .WithSummary("update budget detail")
            .Produces<DefaultIdType>()
            .RequirePermission("Permissions.Accounting.Update")
            .MapToApiVersion(1);
    }
}

