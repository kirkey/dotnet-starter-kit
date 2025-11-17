using Accounting.Application.PostingBatches.Search.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.PostingBatch.v1;

public static class PostingBatchSearchEndpoint
{
    internal static RouteHandlerBuilder MapPostingBatchSearchEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (PostingBatchSearchQuery request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(PostingBatchSearchEndpoint))
            .WithSummary("Search posting batches")
            .WithDescription("Searches posting batches with filtering and pagination")
            .Produces<PagedList<PostingBatchSearchResponse>>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}
