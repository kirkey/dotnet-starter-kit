namespace Accounting.Application.PurchaseOrders.Queries;

/// <summary>
/// Purchase order data transfer object for list views.
/// </summary>
public record PurchaseOrderDto
{
    public DefaultIdType Id { get; init; }
    public string OrderNumber { get; init; } = string.Empty;
    public DateTime OrderDate { get; init; }
    public DateTime? ExpectedDeliveryDate { get; init; }
    public DefaultIdType VendorId { get; init; }
    public string VendorName { get; init; } = string.Empty;
    public decimal TotalAmount { get; init; }
    public decimal ReceivedAmount { get; init; }
    public decimal BilledAmount { get; init; }
    public string Status { get; init; } = string.Empty;
    public string ApprovalStatus { get; init; } = string.Empty;
    public bool IsFullyReceived { get; init; }
    public bool IsFullyBilled { get; init; }
}

/// <summary>
/// Purchase order data transfer object for detail view with all properties.
/// </summary>
public record PurchaseOrderDetailsDto : PurchaseOrderDto
{
    public string? ApprovedBy { get; init; }
    public DateTime? ApprovedDate { get; init; }
    public DefaultIdType? RequesterId { get; init; }
    public string? RequesterName { get; init; }
    public DefaultIdType? CostCenterId { get; init; }
    public DefaultIdType? ProjectId { get; init; }
    public string? ShipToAddress { get; init; }
    public string? PaymentTerms { get; init; }
    public string? ReferenceNumber { get; init; }
    public string? Description { get; init; }
    public string? Notes { get; init; }
}

