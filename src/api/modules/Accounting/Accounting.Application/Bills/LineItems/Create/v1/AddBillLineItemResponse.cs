namespace Accounting.Application.Bills.LineItems.Create.v1;

/// <summary>
/// Response after adding a bill line item.
/// </summary>
/// <param name="LineItemId">The ID of the created line item.</param>
public sealed record AddBillLineItemResponse(DefaultIdType LineItemId);

