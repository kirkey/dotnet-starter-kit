namespace Accounting.Application.Invoices.Delete.v1;

/// <summary>
/// Command to delete an invoice.
/// </summary>
/// <param name="InvoiceId">The ID of the invoice to delete.</param>
public sealed record DeleteInvoiceCommand(DefaultIdType InvoiceId) : IRequest<DeleteInvoiceResponse>;

