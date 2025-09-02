using Accounting.Application.PostingBatch.Queries;
using FSH.Framework.Infrastructure.Auth.Policy;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

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
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}
