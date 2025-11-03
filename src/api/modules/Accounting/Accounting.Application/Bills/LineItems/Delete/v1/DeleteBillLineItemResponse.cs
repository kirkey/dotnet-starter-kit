namespace Accounting.Application.Bills.LineItems.Delete.v1;

/// <summary>
/// Response after deleting a bill line item.
/// </summary>
/// <param name="LineItemId">The ID of the deleted line item.</param>
public sealed record DeleteBillLineItemResponse(DefaultIdType LineItemId);

