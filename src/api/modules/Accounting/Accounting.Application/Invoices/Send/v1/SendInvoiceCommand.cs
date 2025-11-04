namespace Accounting.Application.Invoices.Send.v1;

/// <summary>
/// Command to send an invoice to customer (transition from Draft to Sent status).
/// </summary>
/// <param name="InvoiceId">Invoice identifier.</param>
public sealed record SendInvoiceCommand(DefaultIdType InvoiceId) : IRequest<SendInvoiceResponse>;

