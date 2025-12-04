namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

/// <summary>
/// Endpoint configuration for Share Products.
/// </summary>
public static class ShareProductEndpoints
{
    /// <summary>
    /// Maps all Share Product endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapShareProductEndpoints(this IEndpointRouteBuilder app)
    {
        var shareProductsGroup = app.MapGroup("share-products").WithTags("share-products");

        shareProductsGroup.MapGet("/", () => Results.Ok("Share Products endpoint - Coming soon"))
            .WithName("GetShareProducts")
            .WithSummary("Gets all share products");

        return app;
    }
}
