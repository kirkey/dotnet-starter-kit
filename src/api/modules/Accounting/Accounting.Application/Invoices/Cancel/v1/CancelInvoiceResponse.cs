namespace Accounting.Application.Invoices.Cancel.v1;

/// <summary>
/// Response after cancelling an invoice.
/// </summary>
/// <param name="InvoiceId">Cancelled invoice identifier.</param>
/// <param name="Status">Updated invoice status.</param>
public sealed record CancelInvoiceResponse(
    DefaultIdType InvoiceId,
    string Status
);

