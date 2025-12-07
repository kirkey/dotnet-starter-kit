using Accounting.Infrastructure.Endpoints.Bills.v1;
using Accounting.Infrastructure.Endpoints.Bills.LineItems.v1;
using Carter;

namespace Accounting.Infrastructure.Endpoints.Bills;

/// <summary>
/// Endpoint configuration for Bills module.
/// Provides comprehensive REST API endpoints for managing bills and accounts payable.
/// Uses the ICarterModule delegated pattern with extension methods for each operation.
/// </summary>
public class BillsEndpoints() : CarterModule
{
    /// <summary>
    /// Maps all Bills endpoints to the route builder.
    /// Delegates to extension methods for Create, Read, Update, Delete, and business operation endpoints.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/bills").WithTags("bills");

        // CRUD operations
        group.MapBillCreateEndpoint();
        group.MapGetBillEndpoint();
        group.MapBillUpdateEndpoint();
        group.MapDeleteBillEndpoint();
        group.MapSearchBillsEndpoint();

        // Business operations
        group.MapApproveBillEndpoint();
        group.MapRejectBillEndpoint();
        group.MapPostBillEndpoint();
        group.MapMarkBillAsPaidEndpoint();
        group.MapVoidBillEndpoint();

        // Line Items operations
        group.MapAddBillLineItemEndpoint();
        group.MapGetBillLineItemsEndpoint();
        group.MapGetBillLineItemEndpoint();
        group.MapUpdateBillLineItemEndpoint();
        group.MapDeleteBillLineItemEndpoint();
    }
}

