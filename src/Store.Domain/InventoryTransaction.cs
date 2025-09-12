using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using Store.Domain.Events;

namespace Store.Domain;

public class InventoryTransaction : AuditableEntity, IAggregateRoot
{
    public string TransactionNumber { get; private set; } = default!;
    public DefaultIdType GroceryItemId { get; private set; }
    public DefaultIdType? WarehouseId { get; private set; }
    public DefaultIdType? WarehouseLocationId { get; private set; }
    public DefaultIdType? PurchaseOrderId { get; private set; }
    public string TransactionType { get; private set; } = default!; // IN, OUT, ADJUSTMENT, TRANSFER
    public string Reason { get; private set; } = default!; // Purchase, Sale, Return, Damage, Expiry, etc.
    public int Quantity { get; private set; }
    public int QuantityBefore { get; private set; }
    public int QuantityAfter { get; private set; }
    public decimal UnitCost { get; private set; }
    public decimal TotalCost { get; private set; }
    public DateTime TransactionDate { get; private set; }
    public string? Reference { get; private set; }
    public string? Notes { get; private set; }
    public string? PerformedBy { get; private set; }
    public bool IsApproved { get; private set; }
    public string? ApprovedBy { get; private set; }
    public DateTime? ApprovalDate { get; private set; }
    
    public virtual GroceryItem GroceryItem { get; private set; } = default!;
    public virtual Warehouse? Warehouse { get; private set; }
    public virtual WarehouseLocation? WarehouseLocation { get; private set; }
    public virtual PurchaseOrder? PurchaseOrder { get; private set; }

    private InventoryTransaction() { }

    private InventoryTransaction(
        DefaultIdType id,
        string transactionNumber,
        DefaultIdType groceryItemId,
        DefaultIdType? warehouseId,
        DefaultIdType? warehouseLocationId,
        DefaultIdType? purchaseOrderId,
        string transactionType,
        string reason,
        int quantity,
        int quantityBefore,
        decimal unitCost,
        DateTime transactionDate,
        string? reference,
        string? notes,
        string? performedBy,
        bool isApproved)
    {
        Id = id;
        TransactionNumber = transactionNumber;
        GroceryItemId = groceryItemId;
        WarehouseId = warehouseId;
        WarehouseLocationId = warehouseLocationId;
        PurchaseOrderId = purchaseOrderId;
        TransactionType = transactionType;
        Reason = reason;
        Quantity = quantity;
        QuantityBefore = quantityBefore;
        QuantityAfter = transactionType == "IN" ? quantityBefore + quantity : quantityBefore - quantity;
        UnitCost = unitCost;
        TotalCost = Math.Abs(quantity) * unitCost;
        TransactionDate = transactionDate;
        Reference = reference;
        Notes = notes;
        PerformedBy = performedBy;
        IsApproved = isApproved;

        QueueDomainEvent(new InventoryTransactionCreated { InventoryTransaction = this });
    }

    public static InventoryTransaction Create(
        string transactionNumber,
        DefaultIdType groceryItemId,
        DefaultIdType? warehouseId,
        DefaultIdType? warehouseLocationId,
        DefaultIdType? purchaseOrderId,
        string transactionType,
        string reason,
        int quantity,
        int quantityBefore,
        decimal unitCost,
        DateTime transactionDate,
        string? reference = null,
        string? notes = null,
        string? performedBy = null,
        bool isApproved = false)
    {
        return new InventoryTransaction(
            DefaultIdType.NewGuid(),
            transactionNumber,
            groceryItemId,
            warehouseId,
            warehouseLocationId,
            purchaseOrderId,
            transactionType,
            reason,
            quantity,
            quantityBefore,
            unitCost,
            transactionDate,
            reference,
            notes,
            performedBy,
            isApproved);
    }

    public InventoryTransaction Approve(string approvedBy)
    {
        if (!IsApproved)
        {
            IsApproved = true;
            ApprovedBy = approvedBy;
            ApprovalDate = DateTime.UtcNow;
            
            QueueDomainEvent(new InventoryTransactionApproved { InventoryTransaction = this });
        }

        return this;
    }

    public InventoryTransaction Reject(string rejectedBy, string? rejectionReason = null)
    {
        if (IsApproved)
        {
            IsApproved = false;
            ApprovedBy = null;
            ApprovalDate = null;
            
            if (!string.IsNullOrEmpty(rejectionReason))
            {
                Notes = string.IsNullOrEmpty(Notes) ? rejectionReason : $"{Notes}; Rejection: {rejectionReason}";
            }
            
            QueueDomainEvent(new InventoryTransactionRejected { InventoryTransaction = this, RejectedBy = rejectedBy });
        }

        return this;
    }

    public InventoryTransaction UpdateNotes(string? notes)
    {
        if (!string.Equals(Notes, notes, StringComparison.OrdinalIgnoreCase))
        {
            Notes = notes;
            QueueDomainEvent(new InventoryTransactionNotesUpdated { InventoryTransaction = this });
        }

        return this;
    }

    public bool IsStockIncrease() => TransactionType == "IN";
    public bool IsStockDecrease() => TransactionType == "OUT";
    public bool IsAdjustment() => TransactionType == "ADJUSTMENT";
    public bool IsTransfer() => TransactionType == "TRANSFER";
    public decimal GetImpactOnStock() => IsStockIncrease() ? Quantity : -Quantity;
}
