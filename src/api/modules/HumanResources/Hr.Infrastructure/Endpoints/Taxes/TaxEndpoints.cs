namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Taxes;

using v1;

/// <summary>
/// Tax endpoints coordinator.
/// Maps all tax master configuration endpoints to their respective handlers.
/// </summary>
public static class TaxEndpoints
{
    /// <summary>
    /// Maps all tax endpoints.
    /// </summary>
    /// <param name="app">Endpoint route builder.</param>
    public static void MapTaxEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("taxes").WithTags("Taxes");
        group.MapCreateTaxEndpoint();
        group.MapUpdateTaxEndpoint();
        group.MapGetTaxEndpoint();
        group.MapDeleteTaxEndpoint();
        group.MapSearchTaxesEndpoint();
    }
}

