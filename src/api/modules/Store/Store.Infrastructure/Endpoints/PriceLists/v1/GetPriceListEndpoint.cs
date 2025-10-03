namespace Store.Infrastructure.Endpoints.PriceLists.v1;

/// <summary>
/// Endpoint for retrieving a price list by ID.
/// </summary>
public static class GetPriceListEndpoint
{
    /// <summary>
    /// Maps the get price list endpoint to the route builder.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder</param>
    /// <returns>Route handler builder for get price list endpoint</returns>
    internal static RouteHandlerBuilder MapGetPriceListEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            var result = await sender.Send(new GetPriceListQuery(id)).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(nameof(GetPriceListEndpoint))
        .WithSummary("Get a price list")
        .WithDescription("Retrieves a price list by ID")
        .Produces<GetPriceListResponse>()
        .RequirePermission("Permissions.Store.View")
        .MapToApiVersion(1);
    }
}
