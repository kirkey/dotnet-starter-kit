namespace Store.Domain;

/// <summary>
/// Represents a single inventory movement transaction with comprehensive tracking for stock level changes and financial impact.
/// </summary>
/// <remarks>
/// Use cases:
/// - Record all inventory movements including purchases, sales, adjustments, and transfers.
/// - Maintain detailed audit trail for inventory changes with reasons and authorization.
/// - Calculate financial impact of inventory transactions for cost accounting.
/// - Support perpetual inventory tracking with real-time stock level updates.
/// - Enable inventory reconciliation and variance analysis for discrepancy investigation.
/// - Track transaction sources for integration with POS, procurement, and warehouse systems.
/// - Support regulatory compliance with detailed inventory movement documentation.
/// - Generate inventory reports for management analysis and operational planning.
/// 
/// Default values:
/// - TransactionNumber: required unique identifier (example: "TXN-2025-09-001")
/// - GroceryItemId: required item reference for stock movement
/// - WarehouseId: optional warehouse location (example: main warehouse ID)
/// - WarehouseLocationId: optional specific location (example: aisle, bin)
/// - TransactionType: required movement type (example: "IN", "OUT", "ADJUSTMENT")
/// - TransactionDate: required transaction date (example: 2025-09-19)
/// - Quantity: required quantity moved (positive for IN, negative for OUT)
/// - UnitCost: required cost per unit for financial tracking
/// - TotalCost: calculated as Quantity × UnitCost
/// - Reason: required reason code (example: "PURCHASE", "SALE", "DAMAGED")
/// - ReferenceNumber: optional source document (example: PO number, invoice number)
/// 
/// Business rules:
/// - TransactionNumber must be unique within the system
/// - Quantity cannot be zero (must be positive or negative movement)
/// - UnitCost must be non-negative for cost tracking
/// - Transaction date cannot be in the future
/// - Reason must be from approved reason code list
/// - OUT transactions cannot exceed available inventory
/// - Warehouse must be active for new transactions
/// - Cost adjustments require proper authorization
/// - Transaction reversals create offsetting entries
/// </remarks>
/// <seealso cref="Store.Domain.Events.InventoryTransactionCreated"/>
/// <seealso cref="Store.Domain.Events.InventoryTransactionUpdated"/>
/// <seealso cref="Store.Domain.Events.InventoryTransactionReversed"/>
/// <seealso cref="Store.Domain.Events.InventoryStockUpdated"/>
/// <seealso cref="Store.Domain.Exceptions.InventoryTransaction.InventoryTransactionNotFoundException"/>
/// <seealso cref="Store.Domain.Exceptions.InventoryTransaction.InsufficientInventoryException"/>
/// <seealso cref="Store.Domain.Exceptions.InventoryTransaction.InvalidInventoryTransactionException"/>
public sealed class InventoryTransaction : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Human-friendly transaction identifier. Example: "TXN-202509-001".
    /// </summary>
    public string TransactionNumber { get; private set; } = default!;

    /// <summary>
    /// ID of the grocery item affected by the transaction.
    /// </summary>
    public DefaultIdType GroceryItemId { get; private set; }

    /// <summary>
    /// Warehouse where the transaction occurred (optional).
    /// </summary>
    public DefaultIdType? WarehouseId { get; private set; }

    /// <summary>
    /// Warehouse location within the warehouse (optional).
    /// </summary>
    public DefaultIdType? WarehouseLocationId { get; private set; }

    /// <summary>
    /// Related purchase order id (if this transaction is linked to a PO).
    /// </summary>
    public DefaultIdType? PurchaseOrderId { get; private set; }

    /// <summary>
    /// Type of transaction: IN, OUT, ADJUSTMENT, TRANSFER.
    /// </summary>
    public string TransactionType { get; private set; } = default!; // IN, OUT, ADJUSTMENT, TRANSFER

    /// <summary>
    /// Short reason text for the transaction. Example: "Sale", "Damage", "Return".
    /// </summary>
    public string Reason { get; private set; } = default!;

    /// <summary>
    /// Quantity moved. Positive integer; must be non-zero.
    /// </summary>
    public int Quantity { get; private set; }

    /// <summary>
    /// Quantity before the transaction.
    /// </summary>
    public int QuantityBefore { get; private set; }

    /// <summary>
    /// Quantity after the transaction.
    /// </summary>
    public int QuantityAfter { get; private set; }

    /// <summary>
    /// Cost per unit used to compute the total cost impact.
    /// </summary>
    public decimal UnitCost { get; private set; }

    /// <summary>
    /// Absolute financial impact (Quantity * UnitCost).
    /// </summary>
    public decimal TotalCost { get; private set; }

    /// <summary>
    /// Date when the transaction was recorded.
    /// </summary>
    public DateTime TransactionDate { get; private set; }

    /// <summary>
    /// Optional reference information for the transaction.
    /// </summary>
    public string? Reference { get; private set; }
    
    /// <summary>
    /// User who performed the transaction.
    /// </summary>
    public string? PerformedBy { get; private set; }

    /// <summary>
    /// Indicates if the transaction is approved.
    /// </summary>
    public bool IsApproved { get; private set; }

    /// <summary>
    /// User who approved the transaction.
    /// </summary>
    public string? ApprovedBy { get; private set; }

    /// <summary>
    /// Date when the transaction was approved.
    /// </summary>
    public DateTime? ApprovalDate { get; private set; }
    
    /// <summary>
    /// Related grocery item details.
    /// </summary>
    public GroceryItem GroceryItem { get; private set; } = default!;

    /// <summary>
    /// Related warehouse details (if applicable).
    /// </summary>
    public Warehouse? Warehouse { get; private set; }

    /// <summary>
    /// Related warehouse location details (if applicable).
    /// </summary>
    public WarehouseLocation? WarehouseLocation { get; private set; }

    /// <summary>
    /// Related purchase order details (if applicable).
    /// </summary>
    public PurchaseOrder? PurchaseOrder { get; private set; }

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
        // validations
        if (string.IsNullOrWhiteSpace(transactionNumber)) throw new ArgumentException("TransactionNumber is required", nameof(transactionNumber));
        if (transactionNumber.Length > 100) throw new ArgumentException("TransactionNumber must not exceed 100 characters", nameof(transactionNumber));

        if (groceryItemId == default) throw new ArgumentException("GroceryItemId is required", nameof(groceryItemId));

        var allowedTypes = new[] { "IN", "OUT", "ADJUSTMENT", "TRANSFER" };
        if (string.IsNullOrWhiteSpace(transactionType) || !allowedTypes.Contains(transactionType)) throw new ArgumentException($"Invalid transaction type: {transactionType}", nameof(transactionType));

        if (string.IsNullOrWhiteSpace(reason)) throw new ArgumentException("Reason is required", nameof(reason));
        if (reason.Length > 200) throw new ArgumentException("Reason must not exceed 200 characters", nameof(reason));

        if (quantity == 0) throw new ArgumentException("Quantity must be non-zero", nameof(quantity));
        if (quantity < 0) throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));

        if (quantityBefore < 0) throw new ArgumentException("QuantityBefore must be zero or greater", nameof(quantityBefore));

        if (unitCost < 0m) throw new ArgumentException("UnitCost must be zero or greater", nameof(unitCost));

        if (transactionDate == default) throw new ArgumentException("TransactionDate is required", nameof(transactionDate));

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
