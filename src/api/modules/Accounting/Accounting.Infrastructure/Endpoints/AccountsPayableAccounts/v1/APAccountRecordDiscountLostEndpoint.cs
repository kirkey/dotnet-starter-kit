using Accounting.Application.AccountsPayableAccounts.RecordDiscountLost.v1;

namespace Accounting.Infrastructure.Endpoints.AccountsPayableAccounts.v1;

public static class APAccountRecordDiscountLostEndpoint
{
    internal static RouteHandlerBuilder MapAPAccountRecordDiscountLostEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/discounts-lost", async (DefaultIdType id, RecordDiscountLostCommand command, ISender mediator) =>
            {
                if (id != command.Id) return Results.BadRequest("ID in URL does not match ID in request body.");
                var accountId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = accountId, Message = "Discount lost recorded successfully" });
            })
            .WithName(nameof(APAccountRecordDiscountLostEndpoint))
            .WithSummary("Record missed early payment discount")
            .WithDescription("Records a missed early payment discount opportunity")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Accounting.Create")
            .MapToApiVersion(1);
    }
}

