using Accounting.Application.PostingBatches.Post.v1;

namespace Accounting.Infrastructure.Endpoints.PostingBatch.v1;

public static class PostingBatchPostEndpoint
{
    internal static RouteHandlerBuilder MapPostingBatchPostEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/post", async (DefaultIdType id, ISender mediator) =>
            {
                var batchId = await mediator.Send(new PostingBatchPostCommand(id)).ConfigureAwait(false);
                return Results.Ok(new { Id = batchId, Message = "Posting batch posted successfully" });
            })
            .WithName(nameof(PostingBatchPostEndpoint))
            .WithSummary("Post a posting batch")
            .WithDescription("Posts all journal entries in an approved batch")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Accounting.Update")
            .MapToApiVersion(1);
    }
}

