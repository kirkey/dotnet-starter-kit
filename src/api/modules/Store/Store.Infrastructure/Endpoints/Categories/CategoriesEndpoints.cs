using Store.Infrastructure.Endpoints.Categories.v1;

namespace Store.Infrastructure.Endpoints.Categories;

/// <summary>
/// Carter module for Categories endpoints.
/// Routes all requests to separate versioned endpoint handlers.
/// </summary>
public class CategoriesEndpoints() : CarterModule("store")
{
    /// <summary>
    /// Maps all Categories endpoints to the route builder.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("store/categories").WithTags("categories");

        // Call the individual endpoint handler methods from v1
        group.MapCreateCategoryEndpoint();
        group.MapGetCategoryEndpoint();
        group.MapUpdateCategoryEndpoint();
        group.MapDeleteCategoryEndpoint();
        group.MapSearchCategoriesEndpoint();
    }
}
