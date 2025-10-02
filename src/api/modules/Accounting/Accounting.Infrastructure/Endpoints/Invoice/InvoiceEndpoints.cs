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
            .WithDescription("Endpoints for managing invoices");

        // Version 1 endpoints
        // invoiceGroup.MapInvoiceGetEndpoint();
        // invoiceGroup.MapInvoiceSearchEndpoint();
        // invoiceGroup.MapInvoiceDeleteEndpoint();

        return app;
    }
}
