namespace Accounting.Application.Invoices.LineItems.Delete.v1;

/// <summary>
/// Response after successfully deleting an invoice line item.
/// </summary>
/// <param name="LineItemId">Deleted line item identifier.</param>
/// <param name="IsDeleted">Indicates if deletion was successful.</param>
public sealed record DeleteInvoiceLineItemResponse(
    DefaultIdType LineItemId,
    bool IsDeleted
);

