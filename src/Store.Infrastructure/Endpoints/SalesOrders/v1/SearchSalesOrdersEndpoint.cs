using FSH.Starter.WebApi.Store.Application.SalesOrders.Get.v1;
using FSH.Starter.WebApi.Store.Application.SalesOrders.Search.v1;

namespace Store.Infrastructure.Endpoints.SalesOrders.v1;

/// <summary>
/// Endpoint for searching sales orders with pagination and filtering capabilities.
/// </summary>
public static class SearchSalesOrdersEndpoint
{
    /// <summary>
    /// Maps the search sales orders endpoint to the route builder.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder</param>
    /// <returns>Route handler builder for search sales orders endpoint</returns>
    internal static RouteHandlerBuilder MapSearchSalesOrdersEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/search", async (SearchSalesOrdersCommand command, ISender sender) =>
        {
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("SearchSalesOrders")
        .WithSummary("Search sales orders")
        .WithDescription("Search and filter sales orders with pagination support")
        .Produces<PagedList<GetSalesOrderResponse>>()
        .MapToApiVersion(1);
    }
}
