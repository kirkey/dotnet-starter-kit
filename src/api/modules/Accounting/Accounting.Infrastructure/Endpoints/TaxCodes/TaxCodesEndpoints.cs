using Accounting.Infrastructure.Endpoints.TaxCodes.v1;
using Carter;

namespace Accounting.Infrastructure.Endpoints.TaxCodes;

/// <summary>
/// Endpoint configuration for TaxCodes module.
/// Provides comprehensive REST API endpoints for managing tax-codes.
/// Uses the ICarterModule delegated pattern with extension methods for each operation.
/// </summary>
public class TaxCodesEndpoints() : CarterModule("accounting")
{
    /// <summary>
    /// Maps all TaxCodes endpoints to the route builder.
    /// Delegates to extension methods for Create, Read, Update, Delete, and business operation endpoints.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/tax-codes").WithTags("tax-code");

        group.MapTaxCodeCreateEndpoint();
        group.MapTaxCodeDeleteEndpoint();
        group.MapTaxCodeGetEndpoint();
        group.MapTaxCodeSearchEndpoint();
        group.MapTaxCodeUpdateEndpoint();
    }
}
