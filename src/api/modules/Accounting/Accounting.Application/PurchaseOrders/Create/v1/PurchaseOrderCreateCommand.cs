namespace Accounting.Application.PurchaseOrders.Create.v1;

/// <summary>
/// Command to create a new purchase order.
/// </summary>
public record PurchaseOrderCreateCommand(
    string OrderNumber,
    DateTime OrderDate,
    DefaultIdType VendorId,
    string VendorName,
    DefaultIdType? RequesterId = null,
    string? RequesterName = null,
    DefaultIdType? CostCenterId = null,
    DefaultIdType? ProjectId = null,
    DateTime? ExpectedDeliveryDate = null,
    string? ShipToAddress = null,
    string? PaymentTerms = null,
    string? ReferenceNumber = null,
    string? Description = null,
    string? Notes = null
) : IRequest<PurchaseOrderCreateResponse>;

