using Accounting.Application.AccountsPayableAccounts.RecordDiscountLost.v1;

namespace Accounting.Infrastructure.Endpoints.AccountsPayableAccounts.v1;

/// <summary>
/// Endpoint for recording AP discount lost.
/// </summary>
public static class ApAccountRecordDiscountLostEndpoint
{
    internal static RouteHandlerBuilder MapApAccountRecordDiscountLostEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/discounts-lost", async (DefaultIdType id, RecordAPDiscountLostCommand command, ISender mediator) =>
            {
                if (id != command.Id) return Results.BadRequest("ID in URL does not match ID in request body.");
                var accountId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = accountId, Message = "Discount lost recorded successfully" });
            })
            .WithName(nameof(ApAccountRecordDiscountLostEndpoint))
            .WithSummary("Record missed early payment discount")
            .WithDescription("Records a missed early payment discount opportunity for an AP account")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Accounting.Create")
            .MapToApiVersion(1);
    }
}

