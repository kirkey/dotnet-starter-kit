using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.Warehouse.Domain.Events;
using FSH.Starter.WebApi.Warehouse.Domain.ValueObjects;

namespace FSH.Starter.WebApi.Warehouse.Domain;

public sealed class StockMovement : AuditableEntity, IAggregateRoot
{
    private StockMovement() { }

    private StockMovement(
        DefaultIdType warehouseId,
        string productSku,
        string productName,
        StockMovementType movementType,
        decimal quantity,
        UnitOfMeasure unitOfMeasure,
        string? referenceNumber = null,
        string? notes = null,
        DefaultIdType? sourceWarehouseId = null,
        DefaultIdType? destinationWarehouseId = null)
    {
        WarehouseId = warehouseId;
        ProductSku = productSku;
        ProductName = productName;
        MovementType = movementType;
        Quantity = quantity;
        UnitOfMeasure = unitOfMeasure;
        ReferenceNumber = referenceNumber;
        Notes = notes;
        SourceWarehouseId = sourceWarehouseId;
        DestinationWarehouseId = destinationWarehouseId;
        MovementDate = DateTime.UtcNow;
        Status = StockMovementStatus.Pending;

        QueueDomainEvent(new StockMovementCreated(Id, WarehouseId, ProductSku, MovementType, Quantity));
        WarehouseMetrics.StockMovementsCreated.Add(1);
    }

    public DefaultIdType WarehouseId { get; private set; }
    public string ProductSku { get; private set; } = default!;
    public string ProductName { get; private set; } = default!;
    public StockMovementType MovementType { get; private set; }
    public decimal Quantity { get; private set; }
    public UnitOfMeasure UnitOfMeasure { get; private set; } = default!;
    public string? ReferenceNumber { get; private set; }
    public string? Notes { get; private set; }
    public DateTime MovementDate { get; private set; }
    public StockMovementStatus Status { get; private set; }
    public DefaultIdType? SourceWarehouseId { get; private set; }
    public DefaultIdType? DestinationWarehouseId { get; private set; }

    public static StockMovement CreateInbound(
        DefaultIdType warehouseId,
        string productSku,
        string productName,
        decimal quantity,
        UnitOfMeasure unitOfMeasure,
        string? referenceNumber = null,
        string? notes = null) =>
        new(warehouseId, productSku, productName, StockMovementType.Inbound, quantity, unitOfMeasure, referenceNumber, notes);

    public static StockMovement CreateOutbound(
        DefaultIdType warehouseId,
        string productSku,
        string productName,
        decimal quantity,
        UnitOfMeasure unitOfMeasure,
        string? referenceNumber = null,
        string? notes = null) =>
        new(warehouseId, productSku, productName, StockMovementType.Outbound, quantity, unitOfMeasure, referenceNumber, notes);

    public static StockMovement CreateTransfer(
        DefaultIdType sourceWarehouseId,
        DefaultIdType destinationWarehouseId,
        string productSku,
        string productName,
        decimal quantity,
        UnitOfMeasure unitOfMeasure,
        string? referenceNumber = null,
        string? notes = null) =>
        new(sourceWarehouseId, productSku, productName, StockMovementType.Transfer, quantity, unitOfMeasure, referenceNumber, notes, sourceWarehouseId, destinationWarehouseId);

    public StockMovement Confirm()
    {
        if (Status == StockMovementStatus.Pending)
        {
            Status = StockMovementStatus.Confirmed;
            QueueDomainEvent(new StockMovementConfirmed(Id, WarehouseId, ProductSku, MovementType, Quantity));
            WarehouseMetrics.StockMovementsConfirmed.Add(1);
        }
        return this;
    }

    public StockMovement Cancel()
    {
        if (Status == StockMovementStatus.Pending)
        {
            Status = StockMovementStatus.Cancelled;
            QueueDomainEvent(new StockMovementCancelled(Id, WarehouseId, ProductSku, MovementType, Quantity));
            WarehouseMetrics.StockMovementsCancelled.Add(1);
        }
        return this;
    }

    public StockMovement UpdateNotes(string? notes)
    {
        if (!string.Equals(Notes, notes, StringComparison.OrdinalIgnoreCase))
        {
            Notes = notes;
            QueueDomainEvent(new StockMovementUpdated(this));
        }
        return this;
    }
}
