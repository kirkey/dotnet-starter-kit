using Accounting.Application.AccountsReceivableAccounts.RecordCollection.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.AccountsReceivableAccounts.v1;

/// <summary>
/// Endpoint for recording AR collections.
/// </summary>
public static class ArAccountRecordCollectionEndpoint
{
    internal static RouteHandlerBuilder MapArAccountRecordCollectionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/collections", async (DefaultIdType id, RecordARCollectionCommand request, ISender mediator) =>
            {
                var command = request with { Id = id };
                var accountId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = accountId, Message = "Collection recorded successfully" });
            })
            .WithName(nameof(ArAccountRecordCollectionEndpoint))
            .WithSummary("Record AR collection")
            .WithDescription("Records a collection (payment received) for an AR account and updates YTD statistics")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

