using Store.Infrastructure.Endpoints.GroceryItems.v1;

namespace Store.Infrastructure.Endpoints.GroceryItems;

/// <summary>
/// Endpoint configuration for Grocery Items module.
/// </summary>
public static class GroceryItemsEndpoints
{
    /// <summary>
    /// Maps all Grocery Items endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapGroceryItemsEndpoints(this IEndpointRouteBuilder app)
    {
        var groceryItemsGroup = app.MapGroup("/grocery-items")
            .WithTags("Grocery-Items")
            .WithDescription("Endpoints for managing grocery items");

        // Version 1 endpoints
        groceryItemsGroup.MapCreateGroceryItemEndpoint();
        groceryItemsGroup.MapUpdateGroceryItemEndpoint();
        groceryItemsGroup.MapDeleteGroceryItemEndpoint();
        groceryItemsGroup.MapGetGroceryItemEndpoint();
        groceryItemsGroup.MapSearchGroceryItemsEndpoint();
        groceryItemsGroup.MapImportGroceryItemsEndpoint();
        groceryItemsGroup.MapExportGroceryItemsEndpoint();

        return app;
    }
}
