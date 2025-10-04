namespace FSH.Starter.WebApi.Store.Application.InventoryTransactions.Get.v1;

/// <summary>
/// Response for inventory transaction operations.
/// </summary>
public sealed record InventoryTransactionResponse
{
    public DefaultIdType Id { get; init; }
    public string Name { get; init; } = default!;
    public string? Description { get; init; }
    public string TransactionNumber { get; init; } = default!;
    public DefaultIdType ItemId { get; init; }
    public string? ItemName { get; init; }
    public string? ItemSku { get; init; }
    public DefaultIdType WarehouseId { get; init; }
    public string? WarehouseName { get; init; }
    public DefaultIdType? WarehouseLocationId { get; init; }
    public DefaultIdType? BinId { get; init; }
    public DefaultIdType? LotNumberId { get; init; }
    public decimal Quantity { get; init; }
    public DateTime TransactionDate { get; init; }
    public string TransactionType { get; init; } = default!;
    public string? ReferenceType { get; init; }
    public DefaultIdType? ReferenceId { get; init; }
    public decimal? UnitCost { get; init; }
    public string? Notes { get; init; }
}
