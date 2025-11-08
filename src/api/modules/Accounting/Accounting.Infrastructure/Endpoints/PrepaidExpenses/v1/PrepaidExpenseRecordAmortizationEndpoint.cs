using Accounting.Application.PrepaidExpenses.RecordAmortization.v1;

namespace Accounting.Infrastructure.Endpoints.PrepaidExpenses.v1;

public static class PrepaidExpenseRecordAmortizationEndpoint
{
    internal static RouteHandlerBuilder MapPrepaidExpenseRecordAmortizationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/amortization", async (DefaultIdType id, RecordAmortizationCommand command, ISender mediator) =>
            {
                if (id != command.Id) return Results.BadRequest("ID in URL does not match ID in request body.");
                var expenseId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = expenseId, Message = "Amortization recorded successfully" });
            })
            .WithName(nameof(PrepaidExpenseRecordAmortizationEndpoint))
            .WithSummary("Record amortization")
            .WithDescription("Records an amortization posting for the period")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Accounting.Create")
            .MapToApiVersion(1);
    }
}

