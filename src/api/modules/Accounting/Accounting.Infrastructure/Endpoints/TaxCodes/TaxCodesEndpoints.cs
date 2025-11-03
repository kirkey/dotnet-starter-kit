using Accounting.Infrastructure.Endpoints.TaxCodes.v1;

namespace Accounting.Infrastructure.Endpoints.TaxCodes;

/// <summary>
/// Endpoint configuration for Tax Codes module.
/// </summary>
public static class TaxCodesEndpoints
{
    /// <summary>
    /// Maps all Tax Codes endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapTaxCodesEndpoints(this IEndpointRouteBuilder app)
    {
        var taxCodesGroup = app.MapGroup("/tax-codes")
            .WithTags("Tax-Codes")
            .WithDescription("Endpoints for managing tax codes and rates");

        // Version 1 endpoints
        taxCodesGroup.MapTaxCodeCreateEndpoint();
        taxCodesGroup.MapTaxCodeGetEndpoint();
        taxCodesGroup.MapTaxCodeUpdateEndpoint();
        taxCodesGroup.MapTaxCodeDeleteEndpoint();
        taxCodesGroup.MapTaxCodeSearchEndpoint();

        return app;
    }
}
