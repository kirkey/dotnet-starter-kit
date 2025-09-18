using Accounting.Application.PostingBatch.Queries;
using Accounting.Application.PostingBatch.Dtos;

// Endpoint for searching posting batches
namespace Accounting.Infrastructure.Endpoints.PostingBatch.v1;

public static class PostingBatchSearchEndpoint
{
    internal static RouteHandlerBuilder MapPostingBatchSearchEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (SearchPostingBatchesQuery query, ISender mediator) =>
            {
                var response = await mediator.Send(query).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(PostingBatchSearchEndpoint))
            .WithSummary("Search posting batches")
            .WithDescription("Searches posting batches with filters and pagination")
            .Produces<List<PostingBatchDto>>()
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}
