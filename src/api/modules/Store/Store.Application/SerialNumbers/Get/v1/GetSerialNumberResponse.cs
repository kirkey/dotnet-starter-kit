namespace FSH.Starter.WebApi.Store.Application.SerialNumbers.Get.v1;

/// <summary>
/// Response for get serial number operation.
/// </summary>
public sealed record GetSerialNumberResponse
{
    public DefaultIdType Id { get; init; }
    public string SerialNumberValue { get; init; } = default!;
    public DefaultIdType ItemId { get; init; }
    public string? ItemName { get; init; }
    public string? ItemSku { get; init; }
    public DefaultIdType? WarehouseId { get; init; }
    public string? WarehouseName { get; init; }
    public DefaultIdType? WarehouseLocationId { get; init; }
    public DefaultIdType? BinId { get; init; }
    public DefaultIdType? LotNumberId { get; init; }
    public string Status { get; init; } = "Available";
    public DateTime ReceivedDate { get; init; }
    public DateTime? WarrantyExpirationDate { get; init; }
    public bool HasWarranty { get; init; }
    public string? ExternalReference { get; init; }
    public string? Notes { get; init; }
}
