namespace Store.Domain.Entities;

/// <summary>
/// Adjustments made to stock outside normal transactions (e.g., found, damaged, write-off).
/// Records before/after quantities and financial impact.
/// </summary>
/// <remarks>
/// Use cases:
/// - Correct stock after a physical count or incidents.
/// - Produce accounting entries for write-offs or found inventory.
/// - Track shrinkage, damage, and theft losses.
/// - Support regulatory compliance for inventory adjustments.
/// - Maintain audit trail for all stock corrections.
/// </remarks>
/// <seealso cref="Store.Domain.Events.StockAdjustmentCreated"/>
/// <seealso cref="Store.Domain.Events.StockAdjustmentApproved"/>
/// <seealso cref="Store.Domain.Events.StockAdjustmentUpdated"/>
/// <seealso cref="Store.Domain.Exceptions.StockAdjustment.StockAdjustmentNotFoundException"/>
public sealed class StockAdjustment : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Adjustment reference number. Example: "ADJ-2025-01".
    /// Max length: 50.
    /// </summary>
    public string AdjustmentNumber { get; private set; } = default!;

    /// <summary>
    /// Item affected by the adjustment.
    /// Links to <see cref="Item"/> for inventory tracking.
    /// </summary>
    public DefaultIdType ItemId { get; private set; }

    /// <summary>
    /// Warehouse where the adjustment occurred.
    /// Links to <see cref="Warehouse"/> for location tracking.
    /// </summary>
    public DefaultIdType WarehouseId { get; private set; }

    /// <summary>
    /// Optional location within the warehouse.
    /// Links to <see cref="WarehouseLocation"/> for precise placement.
    /// </summary>
    public DefaultIdType? WarehouseLocationId { get; private set; }

    /// <summary>
    /// Date of the adjustment.
    /// Example: 2025-09-19T14:00:00Z. Defaults to current UTC if unspecified.
    /// </summary>
    public DateTime AdjustmentDate { get; private set; }

    /// <summary>
    /// Type of adjustment (Increase, Decrease, Write-Off, Found).
    /// Allowed values: Physical Count, Damage, Loss, Found, Transfer, Other, Increase, Decrease, Write-Off.
    /// </summary>
    public string AdjustmentType { get; private set; } = default!;

    /// <summary>
    /// Reason for the adjustment (e.g., Damage, Theft, Expiry).
    /// Example: "Damaged during shipping", "Expired product". Max length: 200.
    /// </summary>
    public string Reason { get; private set; } = default!;

    /// <summary>
    /// Quantity prior to adjustment.
    /// Example: 100 items before adjustment. Must be &gt;= 0.
    /// </summary>
    public int QuantityBefore { get; private set; }

    /// <summary>
    /// Quantity adjusted (positive integer).
    /// Example: 5 for 5 items adjusted. Must be &gt; 0.
    /// </summary>
    public int AdjustmentQuantity { get; private set; }

    /// <summary>
    /// Quantity after the adjustment (derived).
    /// Calculated based on AdjustmentType and AdjustmentQuantity.
    /// </summary>
    public int QuantityAfter { get; private set; }

    /// <summary>
    /// Unit cost used to compute cost impact of the adjustment.
    /// Example: 2.50 for $2.50 per unit. Must be &gt;= 0.
    /// </summary>
    public decimal UnitCost { get; private set; }

    /// <summary>
    /// Total cost impact for accounting (positive or negative depending on type).
    /// Calculated as AdjustmentQuantity * UnitCost * adjustment direction.
    /// </summary>
    public decimal TotalCostImpact { get; private set; }

    /// <summary>
    /// Optional reference for external systems or notes.
    /// Example: "Ref-12345", "Insurance claim #ABC123".
    /// </summary>
    public string? Reference { get; private set; }
    
    /// <summary>
    /// User who adjusted the stock.
    /// Example: "john.doe", "system". Max length: 100.
    /// </summary>
    public string? AdjustedBy { get; private set; }

    /// <summary>
    /// User who approved the adjustment.
    /// Example: "manager.smith". Max length: 100.
    /// </summary>
    public string? ApprovedBy { get; private set; }

    /// <summary>
    /// Date when the adjustment was approved.
    /// Set when Approve() method is called.
    /// </summary>
    public DateTime? ApprovalDate { get; private set; }

    /// <summary>
    /// Indicates if the adjustment is approved.
    /// Default: false. Set to true when approved.
    /// </summary>
    public bool IsApproved { get; private set; }

    /// <summary>
    /// Batch number for tracking adjustments in bulk.
    /// Example: "BATCH-2025-09-001". Optional grouping identifier.
    /// </summary>
    public string? BatchNumber { get; private set; }

    /// <summary>
    /// Expiry date for perishable items, if applicable.
    /// Example: 2025-12-31 for items with expiration concerns.
    /// </summary>
    public DateTime? ExpiryDate { get; private set; }
    
    /// <summary>
    /// Related grocery item entity.
    /// </summary>
    public Item Item { get; private set; } = default!;

    /// <summary>
    /// Related warehouse entity.
    /// </summary>
    public Warehouse Warehouse { get; private set; } = default!;

    /// <summary>
    /// Related warehouse location entity, if applicable.
    /// </summary>
    public WarehouseLocation? WarehouseLocation { get; private set; }

    private static readonly string[] AllowedAdjustmentTypes = new[] { "Physical Count", "Damage", "Loss", "Found", "Transfer", "Other", "Increase", "Decrease", "Write-Off" };

    private StockAdjustment() { }

    private StockAdjustment(
        DefaultIdType id,
        string adjustmentNumber,
        DefaultIdType groceryItemId,
        DefaultIdType warehouseId,
        DefaultIdType? warehouseLocationId,
        DateTime adjustmentDate,
        string adjustmentType,
        string reason,
        int quantityBefore,
        int adjustmentQuantity,
        decimal unitCost,
        string? reference,
        string? notes,
        string? adjustedBy,
        string? batchNumber,
        DateTime? expiryDate)
    {
        // basic domain validation to prevent invalid state
        if (string.IsNullOrWhiteSpace(adjustmentNumber))
            throw new ArgumentException("Adjustment number is required", nameof(adjustmentNumber));
        if (adjustmentNumber.Length > 50)
            throw new ArgumentException("Adjustment number must not exceed 50 characters", nameof(adjustmentNumber));

        if (!AllowedAdjustmentTypes.Contains(adjustmentType))
            throw new ArgumentException($"Invalid adjustment type: {adjustmentType}", nameof(adjustmentType));

        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("Reason is required", nameof(reason));
        if (reason.Length > 200)
            throw new ArgumentException("Reason must not exceed 200 characters", nameof(reason));

        if (quantityBefore < 0)
            throw new ArgumentException("QuantityBefore must be zero or greater", nameof(quantityBefore));

        if (adjustmentQuantity <= 0)
            throw new ArgumentException("Adjustment quantity must be greater than zero", nameof(adjustmentQuantity));

        if (unitCost < 0m)
            throw new ArgumentException("Unit cost must be zero or greater", nameof(unitCost));

        Id = id;
        AdjustmentNumber = adjustmentNumber;
        ItemId = groceryItemId;
        WarehouseId = warehouseId;
        WarehouseLocationId = warehouseLocationId;
        AdjustmentDate = adjustmentDate;
        AdjustmentType = adjustmentType;
        Reason = reason;
        QuantityBefore = quantityBefore;
        AdjustmentQuantity = adjustmentQuantity;
        QuantityAfter = adjustmentType is "Increase" or "Found" ? quantityBefore + adjustmentQuantity : quantityBefore - adjustmentQuantity;
        if (QuantityAfter < 0)
            throw new ArgumentException("Resulting QuantityAfter cannot be negative", nameof(adjustmentQuantity));

        UnitCost = unitCost;
        TotalCostImpact = adjustmentQuantity * unitCost * (adjustmentType is "Increase" or "Found" ? 1m : -1m);
        Reference = reference;
        Notes = notes;
        AdjustedBy = adjustedBy;
        IsApproved = false;
        BatchNumber = batchNumber;
        ExpiryDate = expiryDate;

        QueueDomainEvent(new StockAdjustmentCreated { StockAdjustment = this });
    }

    /// <summary>
    /// Creates a new StockAdjustment.
    /// </summary>
    /// <param name="adjustmentNumber">Adjustment reference number.</param>
    /// <param name="groceryItemId">Item ID.</param>
    /// <param name="warehouseId">Warehouse ID.</param>
    /// <param name="warehouseLocationId">Warehouse location ID (optional).</param>
    /// <param name="adjustmentDate">Date of the adjustment.</param>
    /// <param name="adjustmentType">Type of adjustment.</param>
    /// <param name="reason">Reason for the adjustment.</param>
    /// <param name="quantityBefore">Quantity before the adjustment.</param>
    /// <param name="adjustmentQuantity">Quantity adjusted.</param>
    /// <param name="unitCost">Unit cost for the adjustment.</param>
    /// <param name="reference">Optional reference for external systems.</param>
    /// <param name="notes">Optional notes.</param>
    /// <param name="adjustedBy">User who made the adjustment.</param>
    /// <param name="batchNumber">Optional batch number.</param>
    /// <param name="expiryDate">Optional expiry date.</param>
    /// <returns>Newly created StockAdjustment.</returns>
    public static StockAdjustment Create(
        string adjustmentNumber,
        DefaultIdType groceryItemId,
        DefaultIdType warehouseId,
        DefaultIdType? warehouseLocationId,
        DateTime adjustmentDate,
        string adjustmentType,
        string reason,
        int quantityBefore,
        int adjustmentQuantity,
        decimal unitCost,
        string? reference = null,
        string? notes = null,
        string? adjustedBy = null,
        string? batchNumber = null,
        DateTime? expiryDate = null)
    {
        return new StockAdjustment(
            DefaultIdType.NewGuid(),
            adjustmentNumber,
            groceryItemId,
            warehouseId,
            warehouseLocationId,
            adjustmentDate,
            adjustmentType,
            reason,
            quantityBefore,
            adjustmentQuantity,
            unitCost,
            reference,
            notes,
            adjustedBy,
            batchNumber,
            expiryDate);
    }

    /// <summary>
    /// Approves the stock adjustment.
    /// </summary>
    /// <param name="approvedBy">User who approved the adjustment.</param>
    /// <returns>Updated StockAdjustment.</returns>
    public StockAdjustment Approve(string approvedBy)
    {
        if (IsApproved)
            return this; // idempotent

        if (string.IsNullOrWhiteSpace(approvedBy))
            throw new ArgumentException("ApprovedBy is required when approving", nameof(approvedBy));

        IsApproved = true;
        ApprovedBy = approvedBy;
        ApprovalDate = DateTime.UtcNow;
        QueueDomainEvent(new StockAdjustmentApproved { StockAdjustment = this });
        return this;
    }

    /// <summary>
    /// Updates an existing stock adjustment.
    /// </summary>
    /// <param name="itemId">New item ID.</param>
    /// <param name="warehouseLocationId">New warehouse location ID (optional).</param>
    /// <param name="adjustmentType">New type of adjustment.</param>
    /// <param name="adjustmentQuantity">New adjustment quantity.</param>
    /// <param name="reason">New reason for the adjustment.</param>
    /// <param name="notes">New notes.</param>
    /// <returns>Updated StockAdjustment.</returns>
    public StockAdjustment Update(
        DefaultIdType groceryItemId,
        DefaultIdType? warehouseLocationId,
        string adjustmentType,
        int adjustmentQuantity,
        string reason,
        string? notes)
    {
        bool isUpdated = false;

        if (groceryItemId == default)
            throw new ArgumentException("ItemId is required", nameof(groceryItemId));

        if (!AllowedAdjustmentTypes.Contains(adjustmentType))
            throw new ArgumentException($"Invalid adjustment type: {adjustmentType}", nameof(adjustmentType));

        if (adjustmentQuantity <= 0)
            throw new ArgumentException("Adjustment quantity must be greater than zero", nameof(adjustmentQuantity));

        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("Reason is required", nameof(reason));

        if (reason.Length > 200)
            throw new ArgumentException("Reason must not exceed 200 characters", nameof(reason));

        if (!Equals(ItemId, groceryItemId))
        {
            ItemId = groceryItemId;
            isUpdated = true;
        }

        if (WarehouseLocationId != warehouseLocationId)
        {
            WarehouseLocationId = warehouseLocationId;
            isUpdated = true;
        }

        if (!string.Equals(AdjustmentType, adjustmentType, StringComparison.OrdinalIgnoreCase))
        {
            AdjustmentType = adjustmentType;
            isUpdated = true;
        }

        if (AdjustmentQuantity != adjustmentQuantity)
        {
            AdjustmentQuantity = adjustmentQuantity;
            isUpdated = true;
        }

        if (!string.Equals(Reason, reason, StringComparison.OrdinalIgnoreCase))
        {
            Reason = reason;
            isUpdated = true;
        }

        if (!string.Equals(Notes, notes, StringComparison.OrdinalIgnoreCase))
        {
            Notes = notes;
            isUpdated = true;
        }

        if (isUpdated)
        {
            // Recalculate derived fields
            QuantityAfter = AdjustmentType is "Increase" or "Found"
                ? QuantityBefore + AdjustmentQuantity
                : QuantityBefore - AdjustmentQuantity;

            if (QuantityAfter < 0)
                throw new InvalidOperationException("Resulting QuantityAfter cannot be negative after update");

            TotalCostImpact = AdjustmentQuantity * UnitCost * (AdjustmentType is "Increase" or "Found" ? 1m : -1m);

            QueueDomainEvent(new StockAdjustmentUpdated { StockAdjustment = this });
        }

        return this;
    }

    /// <summary>
    /// Checks if the adjustment represents a stock increase.
    /// </summary>
    /// <returns>True if stock is increased, false otherwise.</returns>
    public bool IsStockIncrease() => AdjustmentType is "Increase" or "Found";

    /// <summary>
    /// Checks if the adjustment represents a stock decrease.
    /// </summary>
    /// <returns>True if stock is decreased, false otherwise.</returns>
    public bool IsStockDecrease() => AdjustmentType is "Decrease" or "Write-Off";

    /// <summary>
    /// Gets the absolute financial impact of the adjustment.
    /// </summary>
    /// <returns>Positive decimal value of the financial impact.</returns>
    public decimal GetFinancialImpact() => Math.Abs(TotalCostImpact);
}
