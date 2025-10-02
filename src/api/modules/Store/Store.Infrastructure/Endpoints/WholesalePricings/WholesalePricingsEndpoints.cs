namespace Store.Infrastructure.Endpoints.WholesalePricings;

/// <summary>
/// Endpoint configuration for Wholesale Pricings module.
/// </summary>
public static class WholesalePricingsEndpoints
{
    /// <summary>
    /// Maps all Wholesale Pricings endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapWholesalePricingsEndpoints(this IEndpointRouteBuilder app)
    {
        var wholesalePricingsGroup = app.MapGroup("/wholesale-pricings")
            .WithTags("Wholesale-Pricings")
            .WithDescription("Endpoints for managing wholesale pricing structures");

        // Version 1 endpoints will be added here when implemented
        // wholesalePricingsGroup.MapCreateWholesalePricingEndpoint();
        // wholesalePricingsGroup.MapUpdateWholesalePricingEndpoint();
        // wholesalePricingsGroup.MapDeleteWholesalePricingEndpoint();
        // wholesalePricingsGroup.MapGetWholesalePricingEndpoint();
        // wholesalePricingsGroup.MapSearchWholesalePricingsEndpoint();

        return app;
    }
}
