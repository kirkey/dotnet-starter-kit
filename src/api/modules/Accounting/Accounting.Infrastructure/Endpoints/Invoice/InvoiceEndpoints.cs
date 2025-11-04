using Accounting.Infrastructure.Endpoints.Invoice.LineItems.v1;
using Accounting.Infrastructure.Endpoints.Invoice.v1;

namespace Accounting.Infrastructure.Endpoints.Invoice;

/// <summary>
/// Endpoint configuration for Invoice module.
/// </summary>
public static class InvoiceEndpoints
{
    /// <summary>
    /// Maps all Invoice endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapInvoiceEndpoints(this IEndpointRouteBuilder app)
    {
        var invoiceGroup = app.MapGroup("/invoices")
            .WithTags("Invoices")
            .WithDescription("Endpoints for managing customer invoices");

        // Invoice CRUD endpoints
        invoiceGroup.MapInvoiceCreateEndpoint();
        invoiceGroup.MapUpdateInvoiceEndpoint();
        invoiceGroup.MapDeleteInvoiceEndpoint();
        invoiceGroup.MapGetInvoiceEndpoint();
        invoiceGroup.MapSearchInvoicesEndpoint();

        // Invoice workflow endpoints
        invoiceGroup.MapSendInvoiceEndpoint();
        invoiceGroup.MapMarkInvoiceAsPaidEndpoint();
        invoiceGroup.MapApplyInvoicePaymentEndpoint();
        invoiceGroup.MapCancelInvoiceEndpoint();
        invoiceGroup.MapVoidInvoiceEndpoint();

        // Invoice line items endpoints (nested under invoices)
        invoiceGroup.MapAddInvoiceLineItemEndpoint();
        invoiceGroup.MapUpdateInvoiceLineItemEndpoint();
        invoiceGroup.MapDeleteInvoiceLineItemEndpoint();
        invoiceGroup.MapGetInvoiceLineItemEndpoint();
        invoiceGroup.MapGetInvoiceLineItemsEndpoint();

        return app;
    }
}
