using Accounting.Application.DeferredRevenue.Queries;

// Endpoint for getting a deferred revenue
namespace Accounting.Infrastructure.Endpoints.DeferredRevenue.v1;

public static class DeferredRevenueGetEndpoint
{
    internal static RouteHandlerBuilder MapDeferredRevenueGetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetDeferredRevenueByIdQuery(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(DeferredRevenueGetEndpoint))
            .WithSummary("Get deferred revenue by ID")
            .WithDescription("Gets the details of a deferred revenue entry by its ID")
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}
