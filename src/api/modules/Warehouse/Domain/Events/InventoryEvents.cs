using FSH.Framework.Core.Domain.Events;

namespace FSH.Starter.WebApi.Warehouse.Domain.Events;

public sealed record InventoryItemCreated(DefaultIdType Id, DefaultIdType WarehouseId, string ProductSku, decimal InitialStock) : DomainEvent;

public sealed record InventoryItemUpdated(InventoryItem InventoryItem) : DomainEvent;

public sealed record StockAdjusted(DefaultIdType InventoryItemId, DefaultIdType WarehouseId, string ProductSku, decimal AdjustmentQuantity, decimal NewStock, string AdjustmentType, string? Reason) : DomainEvent;

public sealed record StockReserved(DefaultIdType InventoryItemId, DefaultIdType WarehouseId, string ProductSku, decimal ReservedQuantity, decimal TotalReserved) : DomainEvent;

public sealed record StockReservationReleased(DefaultIdType InventoryItemId, DefaultIdType WarehouseId, string ProductSku, decimal ReleasedQuantity, decimal TotalReserved) : DomainEvent;

public sealed record LowStockAlert(DefaultIdType InventoryItemId, DefaultIdType WarehouseId, string ProductSku, decimal CurrentStock, decimal MinimumStock) : DomainEvent;

public sealed record OverstockAlert(DefaultIdType InventoryItemId, DefaultIdType WarehouseId, string ProductSku, decimal CurrentStock, decimal MaximumStock) : DomainEvent;
