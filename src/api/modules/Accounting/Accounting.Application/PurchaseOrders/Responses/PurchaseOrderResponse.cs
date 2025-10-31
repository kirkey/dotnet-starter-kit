namespace Accounting.Application.PurchaseOrders.Responses;

public record PurchaseOrderResponse
{
    public DefaultIdType Id { get; init; }
    public string OrderNumber { get; init; } = string.Empty;
    public DateTime OrderDate { get; init; }
    public DefaultIdType? VendorId { get; init; }
    public string? VendorName { get; init; }
    public decimal TotalAmount { get; init; }
    public string Status { get; init; } = string.Empty;
    public bool IsFullyReceived { get; init; }
    public string? Description { get; init; }
    public string? Notes { get; init; }
}

