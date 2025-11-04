using Accounting.Application.Invoices.LineItems.Get.v1;

namespace Accounting.Application.Invoices.LineItems.GetList.v1;

/// <summary>
/// Query to get all line items for a specific invoice.
/// </summary>
/// <param name="InvoiceId">Invoice identifier.</param>
public sealed record GetInvoiceLineItemsRequest(DefaultIdType InvoiceId) : IRequest<List<InvoiceLineItemResponse>>;

