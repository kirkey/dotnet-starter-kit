using FSH.Framework.Core.Domain.Events;
using FSH.Starter.WebApi.Warehouse.Domain.ValueObjects;

namespace FSH.Starter.WebApi.Warehouse.Domain.Events;

public sealed record StockMovementCreated(DefaultIdType Id, DefaultIdType WarehouseId, string ProductSku, StockMovementType MovementType, decimal Quantity) : DomainEvent;

public sealed record StockMovementConfirmed(DefaultIdType Id, DefaultIdType WarehouseId, string ProductSku, StockMovementType MovementType, decimal Quantity) : DomainEvent;

public sealed record StockMovementCancelled(DefaultIdType Id, DefaultIdType WarehouseId, string ProductSku, StockMovementType MovementType, decimal Quantity) : DomainEvent;

public sealed record StockMovementUpdated(StockMovement StockMovement) : DomainEvent;
