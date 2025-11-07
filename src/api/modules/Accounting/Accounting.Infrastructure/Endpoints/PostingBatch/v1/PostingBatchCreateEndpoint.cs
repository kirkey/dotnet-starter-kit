using Accounting.Application.PostingBatches.Create.v1;

namespace Accounting.Infrastructure.Endpoints.PostingBatch.v1;

public static class PostingBatchCreateEndpoint
{
    internal static RouteHandlerBuilder MapPostingBatchCreateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (PostingBatchCreateCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Created($"/accounting/posting-batch/{response.Id}", response);
            })
            .WithName(nameof(PostingBatchCreateEndpoint))
            .WithSummary("Create posting batch")
            .WithDescription("Creates a new posting batch")
            .Produces<PostingBatchCreateResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission("Permissions.Accounting.Create")
            .MapToApiVersion(1);
    }
}

