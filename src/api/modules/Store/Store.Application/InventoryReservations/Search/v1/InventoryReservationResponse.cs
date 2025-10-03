namespace FSH.Starter.WebApi.Store.Application.InventoryReservations.Search.v1;

/// <summary>
/// Response for inventory reservation search results.
/// </summary>
public sealed record InventoryReservationResponse
{
    public DefaultIdType Id { get; init; }
    public string ReservationNumber { get; init; } = default!;
    public DefaultIdType ItemId { get; init; }
    public string? ItemName { get; init; }
    public string? ItemSku { get; init; }
    public DefaultIdType WarehouseId { get; init; }
    public string? WarehouseName { get; init; }
    public DefaultIdType? WarehouseLocationId { get; init; }
    public DefaultIdType? BinId { get; init; }
    public DefaultIdType? LotNumberId { get; init; }
    public decimal ReservedQuantity { get; init; }
    public DateTime ReservationDate { get; init; }
    public DateTime? ExpirationDate { get; init; }
    public string Status { get; init; } = "Active";
    public string? ReferenceType { get; init; }
    public DefaultIdType? ReferenceId { get; init; }
    public string? Notes { get; init; }
}
