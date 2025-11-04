namespace Accounting.Application.Invoices.Update.v1;

/// <summary>
/// Response after successfully updating an invoice.
/// </summary>
/// <param name="InvoiceId">Updated invoice identifier.</param>
public sealed record UpdateInvoiceResponse(DefaultIdType InvoiceId);

