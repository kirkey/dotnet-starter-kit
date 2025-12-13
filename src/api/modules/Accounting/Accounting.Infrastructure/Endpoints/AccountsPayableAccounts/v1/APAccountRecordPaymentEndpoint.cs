using Accounting.Application.AccountsPayableAccounts.RecordPayment.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.AccountsPayableAccounts.v1;

public static class ApAccountRecordPaymentEndpoint
{
    internal static RouteHandlerBuilder MapApAccountRecordPaymentEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id}/payments", async (DefaultIdType id, RecordAPPaymentCommand command, ISender mediator) =>
            {
                if (id != command.Id) return Results.BadRequest("ID in URL does not match ID in request body.");
                var accountId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = accountId, Message = "Payment recorded successfully" });
            })
            .WithName(nameof(ApAccountRecordPaymentEndpoint))
            .WithSummary("Record vendor payment")
            .WithDescription("Records a payment to vendors and tracks early payment discounts")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Post, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

