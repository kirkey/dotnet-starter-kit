using Accounting.Application.Bills.Post.v1;
using Asp.Versioning;

namespace Accounting.Infrastructure.Endpoints.Bills.v1;

/// <summary>
/// Endpoint for posting bills to the general ledger.
/// </summary>
public static class PostBillEndpoint
{
    /// <summary>
    /// Maps the bill posting endpoint to the route builder.
    /// </summary>
    internal static RouteHandlerBuilder MapPostBillEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}/post", async (DefaultIdType id, ISender mediator) =>
            {
                var command = new PostBillCommand(id);
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(PostBillEndpoint))
            .WithSummary("Post a bill to GL")
            .WithDescription("Posts an approved bill to the general ledger.")
            .Produces<PostBillResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Bills.Post")
            .MapToApiVersion(new ApiVersion(1, 0));
    }
}

