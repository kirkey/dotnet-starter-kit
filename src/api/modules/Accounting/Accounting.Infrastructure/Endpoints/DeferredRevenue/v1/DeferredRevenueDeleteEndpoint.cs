using Accounting.Application.DeferredRevenues.Delete.v1;

namespace Accounting.Infrastructure.Endpoints.DeferredRevenue.v1;

public static class DeferredRevenueDeleteEndpoint
{
    internal static RouteHandlerBuilder MapDeferredRevenueDeleteEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                await mediator.Send(new DeleteDeferredRevenueCommand(id)).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName(nameof(DeferredRevenueDeleteEndpoint))
            .WithSummary("Delete deferred revenue")
            .WithDescription("Deletes an unrecognized deferred revenue entry")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Accounting.Delete")
            .MapToApiVersion(1);
    }
}
