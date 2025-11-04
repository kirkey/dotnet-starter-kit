namespace Accounting.Application.Invoices.LineItems.Get.v1;

/// <summary>
/// Query to get a specific invoice line item by ID.
/// </summary>
/// <param name="LineItemId">Line item identifier.</param>
public sealed record GetInvoiceLineItemRequest(DefaultIdType LineItemId) : IRequest<InvoiceLineItemResponse>;

