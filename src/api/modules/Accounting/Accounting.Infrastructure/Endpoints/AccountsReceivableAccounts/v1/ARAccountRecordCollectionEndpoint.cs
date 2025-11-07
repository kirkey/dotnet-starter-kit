using Accounting.Application.AccountsReceivableAccounts.RecordCollection.v1;

namespace Accounting.Infrastructure.Endpoints.AccountsReceivableAccounts.v1;

public static class ARAccountRecordCollectionEndpoint
{
    internal static RouteHandlerBuilder MapARAccountRecordCollectionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/collections", async (DefaultIdType id, RecordCollectionCommand command, ISender mediator) =>
            {
                if (id != command.Id) return Results.BadRequest("ID in URL does not match ID in request body.");
                var accountId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = accountId, Message = "Collection recorded successfully" });
            })
            .WithName(nameof(ARAccountRecordCollectionEndpoint))
            .WithSummary("Record collection")
            .WithDescription("Records a collection and updates YTD statistics")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Accounting.Create")
            .MapToApiVersion(1);
    }
}

