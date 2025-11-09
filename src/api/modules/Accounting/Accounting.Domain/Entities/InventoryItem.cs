using Accounting.Domain.Events.Inventory;

namespace Accounting.Domain.Entities;

/// <summary>
/// Represents a stock-keeping unit (SKU) tracked in inventory with quantity, unit price, and lifecycle management.
/// </summary>
/// <remarks>
/// Use cases:
/// - Track inventory items for materials, supplies, and spare parts management.
/// - Maintain accurate inventory valuation for financial reporting and cost accounting.
/// - Support inventory transactions (receipts, issues, adjustments, transfers).
/// - Enable perpetual inventory tracking with real-time quantity updates.
/// - Manage item lifecycle with activation/deactivation capabilities.
/// - Support inventory costing methods (FIFO, LIFO, weighted average).
/// - Generate inventory reports for procurement and operations planning.
/// - Track inventory turnover and identify slow-moving or obsolete items.
/// 
/// Default values:
/// - Sku: required unique identifier, max 50 characters (example: "WIRE-12AWG-CU")
/// - Quantity: 0.00 (starting inventory quantity)
/// - UnitPrice: 0.00 (cost per unit for valuation)
/// - IsActive: true (new items are active by default)
/// - Name: inherited item name (example: "12 AWG Copper Wire")
/// - Description: inherited detailed description (example: "12 gauge solid copper wire for electrical installations")
/// 
/// Business rules:
/// - SKU must be unique within the system
/// - Quantity cannot be negative (controlled through inventory transactions)
/// - UnitPrice should reflect current or average cost
/// - Cannot deactivate items with positive on-hand quantities
/// - Inventory adjustments require proper authorization and documentation
/// - Cost changes should be tracked for audit purposes
/// </remarks>
/// <seealso cref="Accounting.Domain.Events.Inventory.InventoryItemCreated"/>
/// <seealso cref="Accounting.Domain.Events.Inventory.InventoryItemUpdated"/>
/// <seealso cref="Accounting.Domain.Events.Inventory.InventoryItemActivated"/>
/// <seealso cref="Accounting.Domain.Events.Inventory.InventoryItemDeactivated"/>
/// <seealso cref="Accounting.Domain.Events.Inventory.InventoryQuantityAdjusted"/>
/// <seealso cref="Accounting.Domain.Events.Inventory.InventoryPriceChanged"/>
public class InventoryItem : AuditableEntity, IAggregateRoot
{
    private const int MaxSkuLength = 50;
    private const int MaxNameLength = 200;
    private const int MaxDescriptionLength = 1000;

    /// <summary>
    /// Unique stock-keeping unit identifier. Trimmed and length-limited.
    /// </summary>
    public string Sku { get; private set; } = string.Empty;

    /// <summary>
    /// Current on-hand quantity. Must be non-negative.
    /// </summary>
    public decimal Quantity { get; private set; }

    /// <summary>
    /// Unit price used for valuation or default pricing.
    /// </summary>
    public decimal UnitPrice { get; private set; }

    /// <summary>
    /// Whether this item is active/available.
    /// </summary>
    public bool IsActive { get; private set; }

    private InventoryItem()
    {
        // for EF
        Sku = string.Empty;
        Name = string.Empty; // base.Name
        Quantity = 0m;
        UnitPrice = 0m;
        IsActive = true;
    }

    private InventoryItem(string sku, string name, decimal quantity, decimal unitPrice, string? description)
    {
        var s = sku.Trim();
        if (string.IsNullOrWhiteSpace(s))
            throw new ArgumentException("SKU is required.");
        if (s.Length > MaxSkuLength)
            throw new ArgumentException($"SKU cannot exceed {MaxSkuLength} characters.");

        var n = name.Trim();
        if (string.IsNullOrWhiteSpace(n))
            throw new ArgumentException("Name is required.");
        if (n.Length > MaxNameLength)
            throw new ArgumentException($"Name cannot exceed {MaxNameLength} characters.");

        if (quantity < 0) throw new ArgumentException("Quantity cannot be negative.");
        if (unitPrice < 0) throw new ArgumentException("Unit price cannot be negative.");

        Sku = s;
        Name = n;
        Quantity = quantity;
        UnitPrice = unitPrice;
        Description = description?.Trim(); if (Description?.Length > MaxDescriptionLength) Description = Description.Substring(0, MaxDescriptionLength);
        IsActive = true;

        QueueDomainEvent(new InventoryItemCreated(Id, Sku, Name, Quantity, UnitPrice, Description));
    }

    /// <summary>
    /// Factory to create a new inventory item with initial quantity and unit price.
    /// </summary>
    public static InventoryItem Create(string sku, string name, decimal quantity, decimal unitPrice, string? description = null)
        => new InventoryItem(sku, name, quantity, unitPrice, description);

    /// <summary>
    /// Update SKU, name, quantity, unit price, or description; enforces non-negative values and length limits.
    /// </summary>
    public InventoryItem Update(string? sku, string? name, decimal? quantity, decimal? unitPrice, string? description)
    {
        bool isUpdated = false;

        if (!string.IsNullOrWhiteSpace(sku) && !string.Equals(Sku, sku, StringComparison.OrdinalIgnoreCase))
        {
            var s = sku.Trim(); if (s.Length > MaxSkuLength) s = s.Substring(0, MaxSkuLength);
            Sku = s; isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(name) && !string.Equals(Name, name, StringComparison.OrdinalIgnoreCase))
        {
            var n = name.Trim(); if (n.Length > MaxNameLength) n = n.Substring(0, MaxNameLength);
            Name = n; isUpdated = true;
        }

        if (quantity.HasValue && Quantity != quantity.Value)
        {
            if (quantity.Value < 0) throw new InvalidInventoryQuantityException();
            Quantity = quantity.Value; isUpdated = true;
        }

        if (unitPrice.HasValue && UnitPrice != unitPrice.Value)
        {
            if (unitPrice.Value < 0) throw new InvalidInventoryUnitPriceException();
            UnitPrice = unitPrice.Value; isUpdated = true;
        }

        if (description != Description)
        {
            var d = description?.Trim(); if (d?.Length > MaxDescriptionLength) d = d.Substring(0, MaxDescriptionLength);
            Description = d; isUpdated = true;
        }

        if (isUpdated) QueueDomainEvent(new InventoryItemUpdated(Id, Sku, Name, Quantity, UnitPrice, Description));
        return this;
    }

    /// <summary>
    /// Deactivate the item; emits a deletion-like event for consumers.
    /// </summary>
    public InventoryItem Deactivate()
    {
        if (!IsActive) throw new InventoryItemAlreadyInactiveException(Id);
        IsActive = false;
        QueueDomainEvent(new InventoryItemDeleted(Id));
        return this;
    }

    /// <summary>
    /// Increase stock on hand by the specified quantity; must be positive.
    /// </summary>
    /// <param name="quantity">The quantity to add to inventory.</param>
    public void AddStock(decimal quantity)
    {
        if (quantity <= 0) throw new InvalidInventoryQuantityException();
        Quantity += quantity;
        QueueDomainEvent(new InventoryItemUpdated(Id, Sku, Name, Quantity, UnitPrice, Description));
    }

    /// <summary>
    /// Reduce stock on hand by the specified quantity; must be positive and not exceed quantity on hand.
    /// </summary>
    /// <param name="quantity">The quantity to reduce from inventory.</param>
    public void ReduceStock(decimal quantity)
    {
        if (quantity <= 0) throw new InvalidInventoryQuantityException();
        if (Quantity - quantity < 0) throw new InsufficientStockException(Id, quantity, Quantity);
        Quantity -= quantity;
        
        QueueDomainEvent(new InventoryItemUpdated(Id, Sku, Name, Quantity, UnitPrice, Description));
    }
}
