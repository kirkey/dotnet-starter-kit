namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.Submit.v1;

/// <summary>
/// Command to submit a purchase order for approval.
/// Submitting moves the order from Draft status to Submitted status.
/// </summary>
public sealed record SubmitPurchaseOrderCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType Id
) : IRequest<SubmitPurchaseOrderResponse>;
