namespace Accounting.Application.Invoices.LineItems.Delete.v1;

/// <summary>
/// Command to delete an invoice line item.
/// </summary>
/// <param name="LineItemId">Line item identifier.</param>
public sealed record DeleteInvoiceLineItemCommand(DefaultIdType LineItemId) : IRequest<DeleteInvoiceLineItemResponse>;

