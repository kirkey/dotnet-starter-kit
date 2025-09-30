using System.ComponentModel;

namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.Send.v1;

/// <summary>
/// Command to send an approved purchase order to the supplier.
/// Sending moves the order from Approved status to Sent status.
/// </summary>
public sealed record SendPurchaseOrderCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType Id,
    [property: DefaultValue(null)] string? DeliveryInstructions = null
) : IRequest<SendPurchaseOrderResponse>;
