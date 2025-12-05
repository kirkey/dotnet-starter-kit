using Accounting.Application.PostingBatches.Create.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.PostingBatch.v1;

public static class PostingBatchCreateEndpoint
{
    internal static RouteHandlerBuilder MapPostingBatchCreateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreatePostingBatchCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Created($"/accounting/posting-batch/{response.Id}", response);
            })
            .WithName(nameof(PostingBatchCreateEndpoint))
            .WithSummary("Create posting batch")
            .WithDescription("Creates a new posting batch")
            .Produces<PostingBatchCreateResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

