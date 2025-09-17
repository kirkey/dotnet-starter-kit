using Accounting.Application.PostingBatch.Commands;

namespace Accounting.Infrastructure.Endpoints.PostingBatch.v1;

public static class PostingBatchCreateEndpoint
{
    internal static RouteHandlerBuilder MapPostingBatchCreateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreatePostingBatchCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(PostingBatchCreateEndpoint))
            .WithSummary("Create posting batch")
            .WithDescription("Creates a new posting batch")
            .Produces<DefaultIdType>()
            .RequirePermission("Permissions.Accounting.Create")
            .MapToApiVersion(1);
    }
}

