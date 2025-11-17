using Accounting.Application.PrepaidExpenses.RecordAmortization.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.PrepaidExpenses.v1;

public static class PrepaidExpenseRecordAmortizationEndpoint
{
    internal static RouteHandlerBuilder MapPrepaidExpenseRecordAmortizationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/amortization", async (DefaultIdType id, RecordAmortizationCommand request, ISender mediator) =>
            {
                var command = request with { Id = id };
                var expenseId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = expenseId, Message = "Amortization recorded successfully" });
            })
            .WithName(nameof(PrepaidExpenseRecordAmortizationEndpoint))
            .WithSummary("Record amortization")
            .WithDescription("Records an amortization posting for the period")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Post, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

