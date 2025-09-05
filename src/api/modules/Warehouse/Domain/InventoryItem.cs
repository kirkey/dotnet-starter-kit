using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.Warehouse.Domain.Events;
using FSH.Starter.WebApi.Warehouse.Domain.ValueObjects;

namespace FSH.Starter.WebApi.Warehouse.Domain;

public sealed class InventoryItem : AuditableEntity, IAggregateRoot
{
    private InventoryItem() { }

    private InventoryItem(
        DefaultIdType warehouseId,
        string productSku,
        string productName,
        decimal currentStock,
        decimal reservedStock,
        decimal minimumStock,
        decimal maximumStock,
        UnitOfMeasure unitOfMeasure)
    {
        WarehouseId = warehouseId;
        ProductSku = productSku;
        ProductName = productName;
        CurrentStock = currentStock;
        ReservedStock = reservedStock;
        MinimumStock = minimumStock;
        MaximumStock = maximumStock;
        UnitOfMeasure = unitOfMeasure;
        LastMovementDate = DateTime.UtcNow;

        QueueDomainEvent(new InventoryItemCreated(Id, WarehouseId, ProductSku, CurrentStock));
        WarehouseMetrics.InventoryItemsCreated.Add(1);
    }

    public DefaultIdType WarehouseId { get; private set; }
    public string ProductSku { get; private set; } = default!;
    public string ProductName { get; private set; } = default!;
    public decimal CurrentStock { get; private set; }
    public decimal ReservedStock { get; private set; }
    public decimal AvailableStock => CurrentStock - ReservedStock;
    public decimal MinimumStock { get; private set; }
    public decimal MaximumStock { get; private set; }
    public UnitOfMeasure UnitOfMeasure { get; private set; } = default!;
    public DateTime LastMovementDate { get; private set; }

    public static InventoryItem Create(
        DefaultIdType warehouseId,
        string productSku,
        string productName,
        decimal initialStock,
        decimal minimumStock,
        decimal maximumStock,
        UnitOfMeasure unitOfMeasure) =>
        new(warehouseId, productSku, productName, initialStock, 0, minimumStock, maximumStock, unitOfMeasure);

    public InventoryItem AddStock(decimal quantity, string? reason = null)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));

        CurrentStock += quantity;
        LastMovementDate = DateTime.UtcNow;

        QueueDomainEvent(new StockAdjusted(Id, WarehouseId, ProductSku, quantity, CurrentStock, "Add", reason));
        WarehouseMetrics.StockAdjustments.Add(1);

        CheckStockLevels();
        return this;
    }

    public InventoryItem RemoveStock(decimal quantity, string? reason = null)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));

        if (quantity > AvailableStock)
            throw new InvalidOperationException($"Cannot remove {quantity} units. Available stock: {AvailableStock}");

        CurrentStock -= quantity;
        LastMovementDate = DateTime.UtcNow;

        QueueDomainEvent(new StockAdjusted(Id, WarehouseId, ProductSku, -quantity, CurrentStock, "Remove", reason));
        WarehouseMetrics.StockAdjustments.Add(1);

        CheckStockLevels();
        return this;
    }

    public InventoryItem ReserveStock(decimal quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));

        if (quantity > AvailableStock)
            throw new InvalidOperationException($"Cannot reserve {quantity} units. Available stock: {AvailableStock}");

        ReservedStock += quantity;
        QueueDomainEvent(new StockReserved(Id, WarehouseId, ProductSku, quantity, ReservedStock));
        return this;
    }

    public InventoryItem ReleaseReservedStock(decimal quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));

        if (quantity > ReservedStock)
            throw new InvalidOperationException($"Cannot release {quantity} units. Reserved stock: {ReservedStock}");

        ReservedStock -= quantity;
        QueueDomainEvent(new StockReservationReleased(Id, WarehouseId, ProductSku, quantity, ReservedStock));
        return this;
    }

    public InventoryItem UpdateStockLevels(decimal? minimumStock, decimal? maximumStock)
    {
        bool isUpdated = false;

        if (minimumStock.HasValue && MinimumStock != minimumStock.Value)
        {
            MinimumStock = minimumStock.Value;
            isUpdated = true;
        }

        if (maximumStock.HasValue && MaximumStock != maximumStock.Value)
        {
            MaximumStock = maximumStock.Value;
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new InventoryItemUpdated(this));
            CheckStockLevels();
        }

        return this;
    }

    private void CheckStockLevels()
    {
        if (CurrentStock <= MinimumStock)
        {
            QueueDomainEvent(new LowStockAlert(Id, WarehouseId, ProductSku, CurrentStock, MinimumStock));
            WarehouseMetrics.LowStockAlerts.Add(1);
        }

        if (CurrentStock >= MaximumStock)
        {
            QueueDomainEvent(new OverstockAlert(Id, WarehouseId, ProductSku, CurrentStock, MaximumStock));
            WarehouseMetrics.OverstockAlerts.Add(1);
        }
    }
}
