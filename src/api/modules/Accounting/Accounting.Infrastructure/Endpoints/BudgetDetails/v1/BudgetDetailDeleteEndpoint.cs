using Accounting.Application.BudgetDetails.Delete;

namespace Accounting.Infrastructure.Endpoints.BudgetDetails.v1;

public static class BudgetDetailDeleteEndpoint
{
    internal static RouteHandlerBuilder MapBudgetDetailDeleteEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                await mediator.Send(new DeleteBudgetDetailCommand(id));
                return Results.NoContent();
            })
            .WithName(nameof(BudgetDetailDeleteEndpoint))
            .WithSummary("delete budget detail")
            .Produces(StatusCodes.Status204NoContent)
            .RequirePermission("Permissions.Accounting.Delete")
            .MapToApiVersion(1);
    }
}

