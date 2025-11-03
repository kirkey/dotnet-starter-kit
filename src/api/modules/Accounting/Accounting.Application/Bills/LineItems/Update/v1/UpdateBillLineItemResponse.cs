namespace Accounting.Application.Bills.LineItems.Update.v1;

/// <summary>
/// Response after updating a bill line item.
/// </summary>
/// <param name="LineItemId">The ID of the updated line item.</param>
public sealed record UpdateBillLineItemResponse(DefaultIdType LineItemId);

