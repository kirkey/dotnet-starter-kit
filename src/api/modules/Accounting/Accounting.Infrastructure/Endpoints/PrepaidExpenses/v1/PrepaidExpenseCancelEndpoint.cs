using Accounting.Application.PrepaidExpenses.Cancel.v1;

namespace Accounting.Infrastructure.Endpoints.PrepaidExpenses.v1;

public static class PrepaidExpenseCancelEndpoint
{
    internal static RouteHandlerBuilder MapPrepaidExpenseCancelEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/cancel", async (DefaultIdType id, CancelPrepaidExpenseCommand request, ISender mediator) =>
            {
                var command = request with { Id = id };
                var expenseId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = expenseId, Message = "Prepaid expense cancelled successfully" });
            })
            .WithName(nameof(PrepaidExpenseCancelEndpoint))
            .WithSummary("Cancel prepaid expense")
            .WithDescription("Cancels a prepaid expense without amortization history")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Accounting.Update")
            .MapToApiVersion(1);
    }
}

