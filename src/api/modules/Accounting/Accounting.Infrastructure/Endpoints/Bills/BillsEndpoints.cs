using Accounting.Infrastructure.Endpoints.Bills.v1;

namespace Accounting.Infrastructure.Endpoints.Bills;

/// <summary>
/// Endpoint configuration for Bills module.
/// Provides comprehensive REST API endpoints for managing bills and accounts payable.
/// </summary>
public static class BillsEndpoints
{
    /// <summary>
    /// Maps all Bills endpoints to the route builder.
    /// Includes Create, Read, Update, and Search operations for bills.
    /// </summary>
    /// <param name="app">The endpoint route builder.</param>
    /// <returns>The configured endpoint route builder.</returns>
    internal static IEndpointRouteBuilder MapBillsEndpoints(this IEndpointRouteBuilder app)
    {
        var billsGroup = app.MapGroup("/bills")
            .WithTags("Bills")
            .WithDescription("Endpoints for managing bills in the accounting system")
            .MapToApiVersion(1);

        // Version 1 endpoints
        billsGroup.MapBillCreateEndpoint();
        billsGroup.MapBillUpdateEndpoint();

        return app;
    }
}

