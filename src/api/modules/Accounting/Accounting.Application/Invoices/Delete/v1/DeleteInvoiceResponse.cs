namespace Accounting.Application.Invoices.Delete.v1;

/// <summary>
/// Response after successfully deleting an invoice.
/// </summary>
/// <param name="InvoiceId">Deleted invoice identifier.</param>
/// <param name="IsDeleted">Indicates if deletion was successful.</param>
public sealed record DeleteInvoiceResponse(DefaultIdType InvoiceId, bool IsDeleted);

