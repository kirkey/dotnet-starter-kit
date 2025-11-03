using System.ComponentModel;

namespace Accounting.Application.Bills.Delete.v1;

/// <summary>
/// Command to delete a bill.
/// </summary>
/// <param name="BillId">The ID of the bill to delete.</param>
public sealed record DeleteBillCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType BillId
) : IRequest<DeleteBillResponse>;
