using FSH.Starter.WebApi.Store.Application.PurchaseOrders.Get.v1;
using FSH.Starter.WebApi.Store.Application.PurchaseOrders.Search.v1;

namespace Store.Infrastructure.Endpoints.PurchaseOrders.v1;

/// <summary>
/// Endpoint for searching purchase orders with pagination and filtering capabilities.
/// </summary>
public static class SearchPurchaseOrdersEndpoint
{
    /// <summary>
    /// Maps the search purchase orders endpoint to the route builder.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder</param>
    /// <returns>Route handler builder for search purchase orders endpoint</returns>
    internal static RouteHandlerBuilder MapSearchPurchaseOrdersEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/search", async (ISender mediator, [FromBody] SearchPurchaseOrdersCommand command) =>
        {
            var response = await mediator.Send(command).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(nameof(SearchPurchaseOrdersEndpoint))
        .WithSummary("Search purchase orders")
        .WithDescription("Search and filter purchase orders with pagination support")
        .Produces<PagedList<PurchaseOrderResponse>>()
        .RequirePermission("Permissions.Store.View")
        .MapToApiVersion(1);
    }
}
