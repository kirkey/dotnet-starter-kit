using Accounting.Application.PrepaidExpenses.Close.v1;

namespace Accounting.Infrastructure.Endpoints.PrepaidExpenses.v1;

public static class PrepaidExpenseCloseEndpoint
{
    internal static RouteHandlerBuilder MapPrepaidExpenseCloseEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/close", async (DefaultIdType id, ClosePrepaidExpenseCommand command, ISender mediator) =>
            {
                if (id != command.Id)
                    return Results.BadRequest("ID mismatch");

                var expenseId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = expenseId, Message = "Prepaid expense closed successfully" });
            })
            .WithName(nameof(PrepaidExpenseCloseEndpoint))
            .WithSummary("Close prepaid expense")
            .WithDescription("Closes a fully amortized prepaid expense")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Accounting.Update")
            .MapToApiVersion(1);
    }
}

