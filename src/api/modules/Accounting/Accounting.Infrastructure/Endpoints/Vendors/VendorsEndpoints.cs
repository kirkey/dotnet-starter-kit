using Accounting.Infrastructure.Endpoints.Vendors.v1;
using Carter;

namespace Accounting.Infrastructure.Endpoints.Vendors;

/// <summary>
/// Endpoint configuration for Vendors module.
/// Provides comprehensive REST API endpoints for managing vendors.
/// Uses the ICarterModule delegated pattern with extension methods for each operation.
/// </summary>
public class VendorsEndpoints() : CarterModule
{
    /// <summary>
    /// Maps all Vendors endpoints to the route builder.
    /// Delegates to extension methods for Create, Read, Update, Delete, and business operation endpoints.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/vendors").WithTags("vendor");

        group.MapVendorCreateEndpoint();
        group.MapVendorDeleteEndpoint();
        group.MapVendorGetEndpoint();
        group.MapVendorSearchEndpoint();
        group.MapVendorUpdateEndpoint();
    }
}
