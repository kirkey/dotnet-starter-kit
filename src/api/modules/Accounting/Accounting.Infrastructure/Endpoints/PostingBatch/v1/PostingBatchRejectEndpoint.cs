using Accounting.Application.PostingBatches.Reject.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.PostingBatch.v1;

public static class PostingBatchRejectEndpoint
{
    internal static RouteHandlerBuilder MapPostingBatchRejectEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/reject", async (DefaultIdType id, PostingBatchRejectCommand request, ISender mediator) =>
            {
                var command = request with { Id = id };
                var batchId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = batchId, Message = "Posting batch rejected successfully" });
            })
            .WithName(nameof(PostingBatchRejectEndpoint))
            .WithSummary("Reject a posting batch")
            .WithDescription("Rejects a posting batch")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

