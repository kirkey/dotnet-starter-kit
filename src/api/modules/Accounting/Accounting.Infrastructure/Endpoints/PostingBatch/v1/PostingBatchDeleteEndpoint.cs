using Accounting.Application.PostingBatches.Delete.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.PostingBatch.v1;

/// <summary>
/// Endpoint for deleting a posting batch.
/// </summary>
public static class PostingBatchDeleteEndpoint
{
    internal static RouteGroupBuilder MapPostingBatchDeleteEndpoint(this RouteGroupBuilder group)
    {
        group.MapDelete("/{id}", async (DefaultIdType id, ISender mediator) =>
        {
            var command = new DeletePostingBatchCommand(id);
            var result = await mediator.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(nameof(PostingBatchDeleteEndpoint))
        .WithSummary("Delete posting batch")
        .WithDescription("Deletes a draft or pending posting batch with no journal entries")
        .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Accounting))
        .MapToApiVersion(1);

        return group;
    }
}

