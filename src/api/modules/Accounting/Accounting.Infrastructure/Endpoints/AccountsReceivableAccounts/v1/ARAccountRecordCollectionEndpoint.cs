using Accounting.Application.AccountsReceivableAccounts.RecordCollection.v1;

namespace Accounting.Infrastructure.Endpoints.AccountsReceivableAccounts.v1;

/// <summary>
/// Endpoint for recording AR collections.
/// </summary>
public static class ArAccountRecordCollectionEndpoint
{
    internal static RouteHandlerBuilder MapArAccountRecordCollectionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/collections", async (DefaultIdType id, RecordARCollectionCommand command, ISender mediator) =>
            {
                if (id != command.Id) return Results.BadRequest("ID in URL does not match ID in request body.");
                var accountId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = accountId, Message = "Collection recorded successfully" });
            })
            .WithName(nameof(ArAccountRecordCollectionEndpoint))
            .WithSummary("Record AR collection")
            .WithDescription("Records a collection (payment received) for an AR account and updates YTD statistics")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Accounting.Create")
            .MapToApiVersion(1);
    }
}

