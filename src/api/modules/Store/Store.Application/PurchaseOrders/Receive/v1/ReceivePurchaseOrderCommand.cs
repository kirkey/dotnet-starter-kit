using System.ComponentModel;

namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.Receive.v1;

/// <summary>
/// Command to receive a purchase order delivery.
/// Marks the entire order as received and updates the actual delivery date.
/// </summary>
public sealed record ReceivePurchaseOrderCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType Id,
    [property: DefaultValue(null)] DateTime? ActualDeliveryDate = null,
    [property: DefaultValue(null)] string? ReceiptNotes = null
) : IRequest<ReceivePurchaseOrderResponse>;
