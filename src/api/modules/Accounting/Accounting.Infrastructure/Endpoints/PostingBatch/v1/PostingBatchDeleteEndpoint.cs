using Accounting.Application.PostingBatches.Delete.v1;

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
        .RequirePermission("Permissions.Accounting.Delete")
        .MapToApiVersion(1);

        return group;
    }
}

