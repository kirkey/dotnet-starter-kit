using Accounting.Application.BudgetDetails.Get;
using Accounting.Application.BudgetDetails.Responses;

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
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}

