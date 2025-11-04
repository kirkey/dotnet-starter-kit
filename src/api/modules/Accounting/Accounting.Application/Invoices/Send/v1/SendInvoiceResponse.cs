namespace Accounting.Application.Invoices.Send.v1;

/// <summary>
/// Response after successfully sending an invoice.
/// </summary>
/// <param name="InvoiceId">Sent invoice identifier.</param>
/// <param name="Status">New status of the invoice.</param>
public sealed record SendInvoiceResponse(DefaultIdType InvoiceId, string Status);

