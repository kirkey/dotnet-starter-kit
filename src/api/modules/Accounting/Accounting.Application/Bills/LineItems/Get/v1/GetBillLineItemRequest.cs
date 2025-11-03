namespace Accounting.Application.Bills.LineItems.Get.v1;

/// <summary>
/// Query to get a single line item by ID.
/// </summary>
/// <param name="LineItemId">The ID of the line item to retrieve.</param>
public sealed record GetBillLineItemRequest(DefaultIdType LineItemId) : IRequest<BillLineItemResponse>;
