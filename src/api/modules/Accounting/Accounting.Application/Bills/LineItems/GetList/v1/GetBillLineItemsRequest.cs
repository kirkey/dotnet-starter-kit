using Accounting.Application.Bills.LineItems.Get.v1;

namespace Accounting.Application.Bills.LineItems.GetList.v1;

/// <summary>
/// Query to get line items for a specific bill.
/// </summary>
/// <param name="BillId">The ID of the bill whose line items to retrieve.</param>
public sealed record GetBillLineItemsRequest(DefaultIdType BillId) : IRequest<List<BillLineItemResponse>>;
