using System.ComponentModel;

namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.Approve.v1;

/// <summary>
/// Command to approve a submitted purchase order.
/// Approval moves the order from Submitted status to Approved status.
/// </summary>
public sealed record ApprovePurchaseOrderCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType Id,
    [property: DefaultValue(null)] string? ApprovalNotes = null
) : IRequest<ApprovePurchaseOrderResponse>;
