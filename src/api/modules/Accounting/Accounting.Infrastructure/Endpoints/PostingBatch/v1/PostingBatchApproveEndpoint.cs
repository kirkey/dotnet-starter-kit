using Accounting.Application.PostingBatches.Approve.v1;

namespace Accounting.Infrastructure.Endpoints.PostingBatch.v1;

public static class PostingBatchApproveEndpoint
{
    internal static RouteHandlerBuilder MapPostingBatchApproveEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/approve", async (DefaultIdType id, PostingBatchApproveCommand command, ISender mediator) =>
            {
                if (id != command.Id) return Results.BadRequest("ID in URL does not match ID in request body.");
                var batchId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = batchId, Message = "Posting batch approved successfully" });
            })
            .WithName(nameof(PostingBatchApproveEndpoint))
            .WithSummary("Approve a posting batch")
            .WithDescription("Approves a posting batch for posting")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Accounting.Update")
            .MapToApiVersion(1);
    }
}

