using Store.Infrastructure.Endpoints.Categories.v1;

namespace Store.Infrastructure.Endpoints.Categories;

/// <summary>
/// Endpoint configuration for Categories module.
/// </summary>
public static class CategoriesEndpoints
{
    /// <summary>
    /// Maps all Categories endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapCategoriesEndpoints(this IEndpointRouteBuilder app)
    {
        var categoriesGroup = app.MapGroup("/categories")
            .WithTags("Categories")
            .WithDescription("Endpoints for managing product categories");

        // Version 1 endpoints
        categoriesGroup.MapCreateCategoryEndpoint();
        categoriesGroup.MapUpdateCategoryEndpoint();
        categoriesGroup.MapDeleteCategoryEndpoint();
        categoriesGroup.MapGetCategoryEndpoint();
        categoriesGroup.MapSearchCategoriesEndpoint();

        return app;
    }
}
