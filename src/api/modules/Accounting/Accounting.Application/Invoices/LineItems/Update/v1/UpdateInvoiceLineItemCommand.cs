namespace Accounting.Application.Invoices.LineItems.Update.v1;

/// <summary>
/// Command to update an invoice line item.
/// </summary>
/// <param name="LineItemId">Line item identifier.</param>
/// <param name="Description">Updated description.</param>
/// <param name="Quantity">Updated quantity.</param>
/// <param name="UnitPrice">Updated unit price.</param>
/// <param name="AccountId">Updated account identifier.</param>
public sealed record UpdateInvoiceLineItemCommand(
    DefaultIdType LineItemId,
    string? Description = null,
    decimal? Quantity = null,
    decimal? UnitPrice = null,
    DefaultIdType? AccountId = null
) : IRequest<UpdateInvoiceLineItemResponse>;

