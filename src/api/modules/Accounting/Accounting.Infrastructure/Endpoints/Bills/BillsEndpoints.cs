using Accounting.Infrastructure.Endpoints.Bills.LineItems.v1;
using Accounting.Infrastructure.Endpoints.Bills.v1;

namespace Accounting.Infrastructure.Endpoints.Bills;

/// <summary>
/// Endpoint configuration for Bills module.
/// </summary>
public static class BillsEndpoints
{
    /// <summary>
    /// Maps all Bill endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapBillsEndpoints(this IEndpointRouteBuilder app)
    {
        var billsGroup = app.MapGroup("/bills")
            .WithTags("Bills")
            .WithDescription("Endpoints for managing vendor bills");

        // Bill CRUD endpoints
        billsGroup.MapBillCreateEndpoint();
        billsGroup.MapBillUpdateEndpoint();
        billsGroup.MapDeleteBillEndpoint();
        billsGroup.MapGetBillEndpoint();
        billsGroup.MapSearchBillsEndpoint();

        // Bill workflow endpoints
        billsGroup.MapApproveBillEndpoint();
        billsGroup.MapRejectBillEndpoint();
        billsGroup.MapPostBillEndpoint();
        billsGroup.MapMarkBillAsPaidEndpoint();
        billsGroup.MapVoidBillEndpoint();

        // Bill line items endpoints (nested under bills)
        billsGroup.MapAddBillLineItemEndpoint();
        billsGroup.MapUpdateBillLineItemEndpoint();
        billsGroup.MapDeleteBillLineItemEndpoint();
        billsGroup.MapGetBillLineItemEndpoint();
        billsGroup.MapGetBillLineItemsEndpoint();
        
        return app;
    }
}

