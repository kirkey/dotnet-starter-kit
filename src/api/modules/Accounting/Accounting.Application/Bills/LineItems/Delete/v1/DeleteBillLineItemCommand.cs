using System.ComponentModel;

namespace Accounting.Application.Bills.LineItems.Delete.v1;

/// <summary>
/// Command to delete a bill line item.
/// </summary>
/// <param name="LineItemId">The ID of the line item to delete.</param>
/// <param name="BillId">The ID of the bill (for validation).</param>
public sealed record DeleteBillLineItemCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType LineItemId,
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType BillId
) : IRequest<DeleteBillLineItemResponse>;
