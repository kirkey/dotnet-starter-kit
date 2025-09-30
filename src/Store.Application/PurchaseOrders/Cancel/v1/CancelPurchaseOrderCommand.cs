using System.ComponentModel;

namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.Cancel.v1;

/// <summary>
/// Command to cancel a purchase order.
/// Cancellation can be done for orders in Draft, Submitted, or Approved status.
/// </summary>
public sealed record CancelPurchaseOrderCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType Id,
    [property: DefaultValue(null)] string? CancellationReason = null
) : IRequest<CancelPurchaseOrderResponse>;
