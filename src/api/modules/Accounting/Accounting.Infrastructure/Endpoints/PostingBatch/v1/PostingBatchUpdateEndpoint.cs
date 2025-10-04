using Accounting.Application.PostingBatches.Commands;

// Endpoint for updating a posting batch
namespace Accounting.Infrastructure.Endpoints.PostingBatch.v1;

public static class PostingBatchUpdateEndpoint
{
    internal static RouteGroupBuilder MapPostingBatchUpdateEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("");
        group.MapPut("/{id}/approve", async (DefaultIdType id, ApprovePostingBatchCommand command, ISender mediator) =>
        {
            command.Id = id;
            await mediator.Send(command).ConfigureAwait(false);
            return Results.NoContent();
        })
        .WithName(nameof(PostingBatchUpdateEndpoint))
        .WithSummary("Approve posting batch")
        .WithDescription("Approves a posting batch")
        .RequirePermission("Permissions.Accounting.Update")
        .MapToApiVersion(1);

        group.MapPut("/{id}/post", async (DefaultIdType id, PostingBatchCommand command, ISender mediator) =>
        {
            command.Id = id;
            await mediator.Send(command).ConfigureAwait(false);
            return Results.NoContent();
        })
        .WithName("PostingBatch")
        .WithSummary("Post posting batch")
        .WithDescription("Posts a posting batch")
        .RequirePermission("Permissions.Accounting.Update")
        .MapToApiVersion(1);

        group.MapPut("/{id}/reject", async (DefaultIdType id, RejectPostingBatchCommand command, ISender mediator) =>
        {
            command.Id = id;
            await mediator.Send(command).ConfigureAwait(false);
            return Results.NoContent();
        })
        .WithName("RejectPostingBatch")
        .WithSummary("Reject posting batch")
        .WithDescription("Rejects a posting batch")
        .RequirePermission("Permissions.Accounting.Update")
        .MapToApiVersion(1);

        group.MapPut("/{id}/reverse", async (DefaultIdType id, ReversePostingBatchCommand command, ISender mediator) =>
        {
            command.Id = id;
            await mediator.Send(command).ConfigureAwait(false);
            return Results.NoContent();
        })
        .WithName("ReversePostingBatch")
        .WithSummary("Reverse posting batch")
        .WithDescription("Reverses a posting batch")
        .RequirePermission("Permissions.Accounting.Update")
        .MapToApiVersion(1);

        return group;
    }
}
