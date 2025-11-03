namespace Accounting.Application.Bills.Post.v1;

/// <summary>
/// Command to post a bill to the general ledger.
/// </summary>
/// <param name="BillId">The ID of the bill to post.</param>
public sealed record PostBillCommand(DefaultIdType BillId) : IRequest<PostBillResponse>;
