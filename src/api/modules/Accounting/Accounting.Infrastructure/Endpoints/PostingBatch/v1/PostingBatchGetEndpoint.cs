using Accounting.Application.PostingBatch.Queries;
using FSH.Framework.Infrastructure.Auth.Policy;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Accounting.Infrastructure.Endpoints.PostingBatch.v1;

public static class PostingBatchGetEndpoint
{
    internal static RouteHandlerBuilder MapPostingBatchGetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetPostingBatchByIdQuery(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(PostingBatchGetEndpoint))
            .WithSummary("Get posting batch by ID")
            .WithDescription("Gets the details of a posting batch by its ID")
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}
