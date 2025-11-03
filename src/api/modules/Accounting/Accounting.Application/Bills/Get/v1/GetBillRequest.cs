namespace Accounting.Application.Bills.Get.v1;

/// <summary>
/// Request to get a bill by ID.
/// </summary>
/// <param name="BillId">The ID of the bill to retrieve.</param>
public sealed record GetBillRequest(DefaultIdType BillId) : IRequest<BillResponse>;
