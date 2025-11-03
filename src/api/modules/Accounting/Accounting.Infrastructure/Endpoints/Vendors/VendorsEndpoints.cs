using Accounting.Infrastructure.Endpoints.Vendors.v1;

namespace Accounting.Infrastructure.Endpoints.Vendors;

/// <summary>
/// Endpoint configuration for Vendors module.
/// Provides comprehensive REST API endpoints for managing vendor accounts.
/// </summary>
public static class VendorsEndpoints
{
    /// <summary>
    /// Maps all Vendors endpoints to the route builder.
    /// Includes Create, Read, Update, Delete, and Search operations for vendors.
    /// </summary>
    /// <param name="app">The endpoint route builder.</param>
    /// <returns>The configured endpoint route builder.</returns>
    internal static IEndpointRouteBuilder MapVendorsEndpoints(this IEndpointRouteBuilder app)
    {
        var vendorsGroup = app.MapGroup("/vendors")
            .WithTags("Vendors")
            .WithDescription("Endpoints for managing vendors in the accounting system")
            .MapToApiVersion(1);

        // Version 1 endpoints
        vendorsGroup.MapVendorCreateEndpoint();
        vendorsGroup.MapVendorUpdateEndpoint();
        vendorsGroup.MapVendorDeleteEndpoint();
        vendorsGroup.MapVendorGetEndpoint();
        vendorsGroup.MapVendorSearchEndpoint();

        return app;
    }
}

