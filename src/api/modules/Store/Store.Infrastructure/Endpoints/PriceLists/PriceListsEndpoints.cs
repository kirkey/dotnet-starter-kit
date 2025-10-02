using Store.Infrastructure.Endpoints.PriceLists.v1;

namespace Store.Infrastructure.Endpoints.PriceLists;

/// <summary>
/// Endpoint configuration for Price Lists module.
/// </summary>
public static class PriceListsEndpoints
{
    /// <summary>
    /// Maps all Price Lists endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapPriceListsEndpoints(this IEndpointRouteBuilder app)
    {
        var priceListsGroup = app.MapGroup("/price-lists")
            .WithTags("Price-Lists")
            .WithDescription("Endpoints for managing price lists");

        // Version 1 endpoints
        priceListsGroup.MapCreatePriceListEndpoint();
        priceListsGroup.MapUpdatePriceListEndpoint();
        priceListsGroup.MapDeletePriceListEndpoint();
        priceListsGroup.MapGetPriceListEndpoint();
        priceListsGroup.MapSearchPriceListsEndpoint();

        return app;
    }
}
