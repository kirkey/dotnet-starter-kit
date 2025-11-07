using Accounting.Application.PostingBatches.Reverse.v1;

namespace Accounting.Infrastructure.Endpoints.PostingBatch.v1;

public static class PostingBatchReverseEndpoint
{
    internal static RouteHandlerBuilder MapPostingBatchReverseEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/reverse", async (DefaultIdType id, PostingBatchReverseCommand command, ISender mediator) =>
            {
                if (id != command.Id) return Results.BadRequest("ID in URL does not match ID in request body.");
                var batchId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = batchId, Message = "Posting batch reversed successfully" });
            })
            .WithName(nameof(PostingBatchReverseEndpoint))
            .WithSummary("Reverse a posting batch")
            .WithDescription("Reverses all entries in a posted batch")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Accounting.Update")
            .MapToApiVersion(1);
    }
}

