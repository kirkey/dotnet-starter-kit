namespace Store.Domain;

/// <summary>
/// Adjustments made to stock outside normal transactions (e.g., found, damaged, write-off).
/// Records before/after quantities and financial impact.
/// </summary>
/// <remarks>
/// Use cases:
/// - Correct stock after a physical count or incidents.
/// - Produce accounting entries for write-offs or found inventory.
/// </remarks>
public sealed class StockAdjustment : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Adjustment reference number. Example: "ADJ-2025-01".
    /// </summary>
    public string AdjustmentNumber { get; private set; } = default!;

    /// <summary>
    /// Grocery item affected by the adjustment.
    /// </summary>
    public DefaultIdType GroceryItemId { get; private set; }

    /// <summary>
    /// Warehouse where the adjustment occurred.
    /// </summary>
    public DefaultIdType WarehouseId { get; private set; }

    /// <summary>
    /// Optional location within the warehouse.
    /// </summary>
    public DefaultIdType? WarehouseLocationId { get; private set; }

    /// <summary>
    /// Date of the adjustment.
    /// </summary>
    public DateTime AdjustmentDate { get; private set; }

    /// <summary>
    /// Type of adjustment (Increase, Decrease, Write-Off, Found).
    /// </summary>
    public string AdjustmentType { get; private set; } = default!;

    /// <summary>
    /// Reason for the adjustment (e.g., Damage, Theft, Expiry).
    /// </summary>
    public string Reason { get; private set; } = default!;

    /// <summary>
    /// Quantity prior to adjustment.
    /// </summary>
    public int QuantityBefore { get; private set; }

    /// <summary>
    /// Quantity adjusted (positive integer).
    /// </summary>
    public int AdjustmentQuantity { get; private set; }

    /// <summary>
    /// Quantity after the adjustment (derived).
    /// </summary>
    public int QuantityAfter { get; private set; }

    /// <summary>
    /// Unit cost used to compute cost impact of the adjustment.
    /// </summary>
    public decimal UnitCost { get; private set; }

    /// <summary>
    /// Total cost impact for accounting (positive or negative depending on type).
    /// </summary>
    public decimal TotalCostImpact { get; private set; }

    /// <summary>
    /// Optional reference for external systems or notes.
    /// </summary>
    public string? Reference { get; private set; }
    
    /// <summary>
    /// User who adjusted the stock.
    /// </summary>
    public string? AdjustedBy { get; private set; }

    /// <summary>
    /// User who approved the adjustment.
    /// </summary>
    public string? ApprovedBy { get; private set; }

    /// <summary>
    /// Date when the adjustment was approved.
    /// </summary>
    public DateTime? ApprovalDate { get; private set; }

    /// <summary>
    /// Indicates if the adjustment is approved.
    /// </summary>
    public bool IsApproved { get; private set; }

    /// <summary>
    /// Batch number for tracking adjustments in bulk.
    /// </summary>
    public string? BatchNumber { get; private set; }

    /// <summary>
    /// Expiry date for perishable items, if applicable.
    /// </summary>
    public DateTime? ExpiryDate { get; private set; }
    
    /// <summary>
    /// Related grocery item entity.
    /// </summary>
    public GroceryItem GroceryItem { get; private set; } = default!;

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
        GroceryItemId = groceryItemId;
        WarehouseId = warehouseId;
        WarehouseLocationId = warehouseLocationId;
        AdjustmentDate = adjustmentDate;
        AdjustmentType = adjustmentType;
        Reason = reason;
        QuantityBefore = quantityBefore;
        AdjustmentQuantity = adjustmentQuantity;
        QuantityAfter = (adjustmentType == "Increase" || adjustmentType == "Found") ? quantityBefore + adjustmentQuantity : quantityBefore - adjustmentQuantity;
        if (QuantityAfter < 0)
            throw new ArgumentException("Resulting QuantityAfter cannot be negative", nameof(adjustmentQuantity));

        UnitCost = unitCost;
        TotalCostImpact = adjustmentQuantity * unitCost * ((adjustmentType == "Increase" || adjustmentType == "Found") ? 1m : -1m);
        Reference = reference;
        Notes = notes;
        AdjustedBy = adjustedBy;
        IsApproved = false;
        BatchNumber = batchNumber;
        ExpiryDate = expiryDate;

        QueueDomainEvent(new StockAdjustmentCreated { StockAdjustment = this });
    }

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
            throw new ArgumentException("GroceryItemId is required", nameof(groceryItemId));

        if (!AllowedAdjustmentTypes.Contains(adjustmentType))
            throw new ArgumentException($"Invalid adjustment type: {adjustmentType}", nameof(adjustmentType));

        if (adjustmentQuantity <= 0)
            throw new ArgumentException("Adjustment quantity must be greater than zero", nameof(adjustmentQuantity));

        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("Reason is required", nameof(reason));

        if (reason.Length > 200)
            throw new ArgumentException("Reason must not exceed 200 characters", nameof(reason));

        if (!Equals(GroceryItemId, groceryItemId))
        {
            GroceryItemId = groceryItemId;
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
            QuantityAfter = AdjustmentType == "Increase" || AdjustmentType == "Found"
                ? QuantityBefore + AdjustmentQuantity
                : QuantityBefore - AdjustmentQuantity;

            if (QuantityAfter < 0)
                throw new InvalidOperationException("Resulting QuantityAfter cannot be negative after update");

            TotalCostImpact = AdjustmentQuantity * UnitCost * ((AdjustmentType == "Increase" || AdjustmentType == "Found") ? 1m : -1m);

            QueueDomainEvent(new StockAdjustmentUpdated { StockAdjustment = this });
        }

        return this;
    }

    public bool IsStockIncrease() => AdjustmentType == "Increase" || AdjustmentType == "Found";
    public bool IsStockDecrease() => AdjustmentType == "Decrease" || AdjustmentType == "Write-Off";
    public decimal GetFinancialImpact() => Math.Abs(TotalCostImpact);
}
