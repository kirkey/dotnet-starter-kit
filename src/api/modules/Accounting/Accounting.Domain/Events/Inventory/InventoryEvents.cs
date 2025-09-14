namespace Accounting.Domain.Events.Inventory;

public record InventoryItemCreated(DefaultIdType InventoryItemId, string Sku, string Name, decimal Quantity, decimal UnitPrice, string? Description) : DomainEvent;

public record InventoryItemUpdated(DefaultIdType InventoryItemId, string? Sku = null, string? Name = null, decimal? Quantity = null, decimal? UnitPrice = null, string? Description = null) : DomainEvent;

public record InventoryItemDeleted(DefaultIdType InventoryItemId) : DomainEvent;
