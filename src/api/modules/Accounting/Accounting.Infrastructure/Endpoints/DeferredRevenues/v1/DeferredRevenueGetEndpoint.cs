using Accounting.Application.DeferredRevenues.Get;
using Accounting.Application.DeferredRevenues.Responses;

namespace Accounting.Infrastructure.Endpoints.DeferredRevenues.v1;

public static class DeferredRevenueGetEndpoint
{
    internal static RouteHandlerBuilder MapDeferredRevenueGetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetDeferredRevenueRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(DeferredRevenueGetEndpoint))
            .WithSummary("Get deferred revenue by ID")
            .WithDescription("Retrieves a deferred revenue entry by its unique identifier")
            .Produces<DeferredRevenueResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}

