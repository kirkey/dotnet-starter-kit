namespace Accounting.Application.Invoices.LineItems.Add.v1;

/// <summary>
/// Response after successfully adding a line item to an invoice.
/// </summary>
/// <param name="InvoiceId">Invoice identifier.</param>
/// <param name="UpdatedTotalAmount">Updated total amount after adding line item.</param>
public sealed record AddInvoiceLineItemResponse(
    DefaultIdType InvoiceId,
    decimal UpdatedTotalAmount
);

