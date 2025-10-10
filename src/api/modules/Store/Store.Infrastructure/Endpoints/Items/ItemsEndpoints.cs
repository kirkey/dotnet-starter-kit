using Store.Infrastructure.Endpoints.Items.v1;

namespace Store.Infrastructure.Endpoints.Items;

/// <summary>
/// Endpoint configuration for Items module.
/// </summary>
public static class ItemsEndpoints
{
    /// <summary>
    /// Maps all Items endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapItemsEndpoints(this IEndpointRouteBuilder app)
    {
        var itemsGroup = app.MapGroup("/items")
            .WithTags("Items")
            .WithDescription("Endpoints for managing inventory items");

        // Version 1 endpoints
        itemsGroup.MapCreateItemEndpoint();
        itemsGroup.MapUpdateItemEndpoint();
        itemsGroup.MapDeleteItemEndpoint();
        itemsGroup.MapGetItemEndpoint();
        itemsGroup.MapSearchItemsEndpoint();
        itemsGroup.MapImportItemsEndpoint();
        itemsGroup.MapExportItemsEndpoint();

        return app;
    }
}
