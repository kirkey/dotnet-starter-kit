namespace Accounting.Application.Invoices.LineItems.Update.v1;

/// <summary>
/// Response after successfully updating an invoice line item.
/// </summary>
/// <param name="LineItemId">Updated line item identifier.</param>
/// <param name="TotalPrice">Updated total price of the line item.</param>
public sealed record UpdateInvoiceLineItemResponse(
    DefaultIdType LineItemId,
    decimal TotalPrice
);

