using Store.Infrastructure.Endpoints.SalesImports.v1;

namespace Store.Infrastructure.Endpoints.SalesImports;

/// <summary>
/// Endpoint configuration for Sales Imports module.
/// </summary>
public static class SalesImportsEndpoints
{
    /// <summary>
    /// Maps all Sales Imports endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapSalesImportsEndpoints(this IEndpointRouteBuilder app)
    {
        var salesImportsGroup = app.MapGroup("/sales-imports")
            .WithTags("Sales Imports")
            .WithDescription("Endpoints for importing POS sales data and maintaining inventory accuracy");

        // Version 1 endpoints
        salesImportsGroup.MapCreateSalesImportEndpoint();
        salesImportsGroup.MapGetSalesImportEndpoint();
        salesImportsGroup.MapSearchSalesImportsEndpoint();
        salesImportsGroup.MapReverseSalesImportEndpoint();

        return app;
    }
}

