namespace Store.Infrastructure.Endpoints.WholesaleContracts;

/// <summary>
/// Endpoint configuration for Wholesale Contracts module.
/// </summary>
public static class WholesaleContractsEndpoints
{
    /// <summary>
    /// Maps all Wholesale Contracts endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapWholesaleContractsEndpoints(this IEndpointRouteBuilder app)
    {
        var wholesaleContractsGroup = app.MapGroup("/wholesale-contracts")
            .WithTags("Wholesale-Contracts")
            .WithDescription("Endpoints for managing wholesale contracts");

        // Version 1 endpoints will be added here when implemented
        // wholesaleContractsGroup.MapCreateWholesaleContractEndpoint();
        // wholesaleContractsGroup.MapUpdateWholesaleContractEndpoint();
        // wholesaleContractsGroup.MapDeleteWholesaleContractEndpoint();
        // wholesaleContractsGroup.MapGetWholesaleContractEndpoint();
        // wholesaleContractsGroup.MapSearchWholesaleContractsEndpoint();

        return app;
    }
}
