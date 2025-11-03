namespace Accounting.Application.Bills.Create.v1;

/// <summary>
/// Response after creating a bill.
/// </summary>
/// <param name="BillId">The ID of the created bill.</param>
public sealed record BillCreateResponse(DefaultIdType BillId);

