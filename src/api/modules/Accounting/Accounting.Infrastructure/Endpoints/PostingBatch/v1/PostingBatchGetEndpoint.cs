using Accounting.Application.PostingBatches.Get.v1;

namespace Accounting.Infrastructure.Endpoints.PostingBatch.v1;

public static class PostingBatchGetEndpoint
{
    internal static RouteHandlerBuilder MapPostingBatchGetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new PostingBatchGetQuery(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(PostingBatchGetEndpoint))
            .WithSummary("Get posting batch by ID")
            .WithDescription("Retrieves a posting batch by its unique identifier")
            .Produces<PostingBatchGetResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}
