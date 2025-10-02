using Accounting.Application.Budgets.Details.Create;

namespace Accounting.Infrastructure.Endpoints.BudgetDetails.v1;

public static class BudgetDetailCreateEndpoint
{
    internal static RouteHandlerBuilder MapBudgetDetailCreateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost(string.Empty, async (CreateBudgetDetailCommand request, ISender mediator) =>
            {
                var id = await mediator.Send(request);
                return Results.Ok(id);
            })
            .WithName(nameof(BudgetDetailCreateEndpoint))
            .WithSummary("create budget detail")
            .Produces<DefaultIdType>()
            .RequirePermission("Permissions.Accounting.Create")
            .MapToApiVersion(1);
    }
}

