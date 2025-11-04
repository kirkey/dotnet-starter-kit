namespace Accounting.Application.Invoices.Void.v1;

/// <summary>
/// Response after voiding an invoice.
/// </summary>
/// <param name="InvoiceId">Voided invoice identifier.</param>
/// <param name="Status">Updated invoice status.</param>
public sealed record VoidInvoiceResponse(
    DefaultIdType InvoiceId,
    string Status
);

