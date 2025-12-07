using Store.Infrastructure.Endpoints.Items.v1;

namespace Store.Infrastructure.Endpoints.Items;

/// <summary>
/// Carter module for Items endpoints.
/// Routes all requests to separate versioned endpoint handlers.
/// </summary>
public class ItemsEndpoints() : CarterModule
{
    /// <summary>
    /// Maps all Items endpoints to the route builder.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("items").WithTags("items");

        // Call the individual endpoint handler methods from v1
        group.MapCreateItemEndpoint();
        group.MapGetItemEndpoint();
        group.MapUpdateItemEndpoint();
        group.MapDeleteItemEndpoint();
        group.MapSearchItemsEndpoint();
        group.MapImportItemsEndpoint();
        group.MapExportItemsEndpoint();
        
        // Dashboard
        group.MapGetItemDashboardEndpoint();
    }
}
