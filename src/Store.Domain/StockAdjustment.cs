using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using Store.Domain.Events;

namespace Store.Domain;

public class StockAdjustment : AuditableEntity, IAggregateRoot
{
    public string AdjustmentNumber { get; private set; } = default!;
    public DefaultIdType GroceryItemId { get; private set; }
    public DefaultIdType WarehouseId { get; private set; }
    public DefaultIdType? WarehouseLocationId { get; private set; }
    public DateTime AdjustmentDate { get; private set; }
    public string AdjustmentType { get; private set; } = default!; // Increase, Decrease, Write-Off, Found
    public string Reason { get; private set; } = default!; // Damage, Theft, Expiry, Count Error, etc.
    public int QuantityBefore { get; private set; }
    public int AdjustmentQuantity { get; private set; }
    public int QuantityAfter { get; private set; }
    public decimal UnitCost { get; private set; }
    public decimal TotalCostImpact { get; private set; }
    public string? Reference { get; private set; }
    public string? Notes { get; private set; }
    public string? AdjustedBy { get; private set; }
    public string? ApprovedBy { get; private set; }
    public DateTime? ApprovalDate { get; private set; }
    public bool IsApproved { get; private set; }
    public string? BatchNumber { get; private set; }
    public DateTime? ExpiryDate { get; private set; }
    
    public virtual GroceryItem GroceryItem { get; private set; } = default!;
    public virtual Warehouse Warehouse { get; private set; } = default!;
    public virtual WarehouseLocation? WarehouseLocation { get; private set; }

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
        QuantityAfter = adjustmentType == "Increase" ? quantityBefore + adjustmentQuantity : quantityBefore - adjustmentQuantity;
        UnitCost = unitCost;
        TotalCostImpact = adjustmentQuantity * unitCost * (adjustmentType == "Increase" ? 1 : -1);
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
        if (!IsApproved)
        {
            IsApproved = true;
            ApprovedBy = approvedBy;
            ApprovalDate = DateTime.UtcNow;
            QueueDomainEvent(new StockAdjustmentApproved { StockAdjustment = this });
        }
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

        if (GroceryItemId != groceryItemId)
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

            TotalCostImpact = AdjustmentQuantity * UnitCost * ((AdjustmentType == "Increase" || AdjustmentType == "Found") ? 1m : -1m);

            QueueDomainEvent(new StockAdjustmentUpdated { StockAdjustment = this });
        }

        return this;
    }

    public bool IsStockIncrease() => AdjustmentType == "Increase" || AdjustmentType == "Found";
    public bool IsStockDecrease() => AdjustmentType == "Decrease" || AdjustmentType == "Write-Off";
    public decimal GetFinancialImpact() => Math.Abs(TotalCostImpact);
}
