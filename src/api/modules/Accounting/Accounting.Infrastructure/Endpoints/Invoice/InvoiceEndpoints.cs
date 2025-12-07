using Accounting.Infrastructure.Endpoints.Invoice.v1;
using Accounting.Infrastructure.Endpoints.Invoice.LineItems.v1;
using Carter;

namespace Accounting.Infrastructure.Endpoints.Invoice;

/// <summary>
/// Endpoint configuration for Invoice module.
/// Provides comprehensive REST API endpoints for managing invoices and accounts receivable.
/// Uses the ICarterModule delegated pattern with extension methods for each operation.
/// </summary>
public class InvoiceEndpoints() : CarterModule
{
    /// <summary>
    /// Maps all Invoice endpoints to the route builder.
    /// Delegates to extension methods for CRUD, workflow, and line item operations.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/invoices").WithTags("invoices");

        // CRUD operations
        group.MapCreateInvoiceEndpoint();
        group.MapGetInvoiceEndpoint();
        group.MapUpdateInvoiceEndpoint();
        group.MapDeleteInvoiceEndpoint();
        group.MapSearchInvoicesEndpoint();

        // Workflow operations
        group.MapSendInvoiceEndpoint();
        group.MapMarkInvoiceAsPaidEndpoint();
        group.MapApplyInvoicePaymentEndpoint();
        group.MapCancelInvoiceEndpoint();
        group.MapVoidInvoiceEndpoint();

        // Line Items operations
        group.MapAddInvoiceLineItemEndpoint();
        group.MapUpdateInvoiceLineItemEndpoint();
        group.MapDeleteInvoiceLineItemEndpoint();
        group.MapGetInvoiceLineItemEndpoint();
        group.MapGetInvoiceLineItemsEndpoint();
    }
}
