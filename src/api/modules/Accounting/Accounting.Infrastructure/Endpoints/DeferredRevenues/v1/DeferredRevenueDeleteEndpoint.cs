using Accounting.Application.DeferredRevenues.Delete;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.DeferredRevenues.v1;

public static class DeferredRevenueDeleteEndpoint
{
    internal static RouteHandlerBuilder MapDeferredRevenueDeleteEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var result = await mediator.Send(new DeleteDeferredRevenueCommand(id)).ConfigureAwait(false);
                return Results.Ok(new { Id = result });
            })
            .WithName(nameof(DeferredRevenueDeleteEndpoint))
            .WithSummary("Delete deferred revenue")
            .WithDescription("Deletes a deferred revenue entry (cannot delete recognized revenue)")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

