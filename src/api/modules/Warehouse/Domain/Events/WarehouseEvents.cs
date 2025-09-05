using FSH.Framework.Core.Domain.Events;

namespace FSH.Starter.WebApi.Warehouse.Domain.Events;

public sealed record WarehouseCreated(DefaultIdType Id, string Name, string Code, string Address) : DomainEvent;

public sealed record WarehouseUpdated(FSH.Starter.WebApi.Warehouse.Domain.Warehouse Warehouse) : DomainEvent;

public sealed record WarehouseActivated(DefaultIdType Id, string Name, string Code) : DomainEvent;

public sealed record WarehouseDeactivated(DefaultIdType Id, string Name, string Code) : DomainEvent;
