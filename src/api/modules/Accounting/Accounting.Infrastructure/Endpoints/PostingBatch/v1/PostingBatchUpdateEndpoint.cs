using Accounting.Application.PostingBatches.Update.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.PostingBatch.v1;

/// <summary>
/// Endpoint for updating a posting batch.
/// </summary>
public static class PostingBatchUpdateEndpoint
{
    internal static RouteGroupBuilder MapPostingBatchUpdateEndpoint(this RouteGroupBuilder group)
    {
        group.MapPut("/{id}", async (DefaultIdType id, UpdatePostingBatchCommand command, ISender mediator) =>
        {
            if (id != command.Id)
                return Results.BadRequest("ID in URL does not match ID in request body");

            var result = await mediator.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(nameof(PostingBatchUpdateEndpoint))
        .WithSummary("Update posting batch")
        .WithDescription("Updates a draft or pending posting batch")
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
        .MapToApiVersion(1);

        return group;
    }
}

