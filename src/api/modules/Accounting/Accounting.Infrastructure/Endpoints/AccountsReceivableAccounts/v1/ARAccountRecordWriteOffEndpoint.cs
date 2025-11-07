using Accounting.Application.AccountsReceivableAccounts.RecordWriteOff.v1;

namespace Accounting.Infrastructure.Endpoints.AccountsReceivableAccounts.v1;

public static class ARAccountRecordWriteOffEndpoint
{
    internal static RouteHandlerBuilder MapARAccountRecordWriteOffEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/write-offs", async (DefaultIdType id, RecordWriteOffCommand command, ISender mediator) =>
            {
                if (id != command.Id) return Results.BadRequest("ID in URL does not match ID in request body.");
                var accountId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = accountId, Message = "Write-off recorded successfully" });
            })
            .WithName(nameof(ARAccountRecordWriteOffEndpoint))
            .WithSummary("Record bad debt write-off")
            .WithDescription("Records a write-off and updates bad debt statistics")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Accounting.Create")
            .MapToApiVersion(1);
    }
}

