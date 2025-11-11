namespace Store.Domain.Entities;

/// <summary>
/// Represents a batch import of sales data from external Point of Sale (POS) systems to maintain inventory accuracy.
/// </summary>
/// <remarks>
/// Use cases:
/// - Import CSV files containing daily/periodic sales from POS systems to update inventory levels.
/// - Track import history with batch numbers, dates, and processing status for audit purposes.
/// - Validate sales data against existing items using barcode matching and SKU verification.
/// - Automatically create inventory OUT transactions for each sale to maintain accurate stock levels.
/// - Support multi-store operations with store-specific sales imports and inventory adjustments.
/// - Enable reconciliation between POS sales and warehouse inventory for discrepancy detection.
/// - Generate import reports showing successful imports, errors, and inventory impacts.
/// - Support rollback/reversal of erroneous imports with proper audit trail.
/// 
/// Default values:
/// - ImportNumber: required unique identifier (example: "IMP-20251111-001")
/// - ImportDate: required import processing date (example: 2025-11-11)
/// - SalesPeriodFrom: required sales period start (example: 2025-11-10)
/// - SalesPeriodTo: required sales period end (example: 2025-11-10)
/// - WarehouseId: required store/warehouse location reference
/// - FileName: required original CSV filename (example: "pos_sales_20251110.csv")
/// - TotalRecords: 0 (total records in import file)
/// - ProcessedRecords: 0 (successfully processed records)
/// - ErrorRecords: 0 (records with errors)
/// - TotalQuantity: 0 (total quantity sold across all items)
/// - Status: "PENDING" (import processing status)
/// - IsReversed: false (indicates if import has been reversed)
/// 
/// Business rules:
/// - ImportNumber must be unique within the system
/// - SalesPeriodTo must be >= SalesPeriodFrom
/// - Sales period dates cannot be in the future
/// - Warehouse must exist and be active
/// - Cannot reverse an import that has already been reversed
/// - Each import creates corresponding inventory transactions with type "OUT"
/// - Duplicate imports for same period/warehouse are prevented
/// - Items must exist in system and match by barcode or SKU
/// - Processed imports cannot be deleted, only reversed
/// - Import status flows: PENDING → PROCESSING → COMPLETED/FAILED
/// </remarks>
/// <seealso cref="Store.Domain.Entities.SalesImportItem"/>
/// <seealso cref="Store.Domain.Entities.InventoryTransaction"/>
/// <seealso cref="Store.Domain.Entities.Warehouse"/>
/// <seealso cref="Store.Domain.Events.SalesImportCreated"/>
/// <seealso cref="Store.Domain.Events.SalesImportProcessed"/>
/// <seealso cref="Store.Domain.Events.SalesImportReversed"/>
public sealed class SalesImport : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Human-friendly import identifier. Example: "IMP-20251111-001".
    /// </summary>
    public string ImportNumber { get; private set; } = default!;

    /// <summary>
    /// Date when the import was processed in the system.
    /// </summary>
    public DateTime ImportDate { get; private set; }

    /// <summary>
    /// Start date of the sales period covered by this import.
    /// </summary>
    public DateTime SalesPeriodFrom { get; private set; }

    /// <summary>
    /// End date of the sales period covered by this import.
    /// </summary>
    public DateTime SalesPeriodTo { get; private set; }

    /// <summary>
    /// Store/warehouse identifier where sales occurred.
    /// </summary>
    public DefaultIdType WarehouseId { get; private set; }

    /// <summary>
    /// Original CSV filename imported from POS system.
    /// </summary>
    public string FileName { get; private set; } = default!;

    /// <summary>
    /// Total number of records in the import file.
    /// </summary>
    public int TotalRecords { get; private set; }

    /// <summary>
    /// Number of records successfully processed.
    /// </summary>
    public int ProcessedRecords { get; private set; }

    /// <summary>
    /// Number of records that failed processing.
    /// </summary>
    public int ErrorRecords { get; private set; }

    /// <summary>
    /// Total quantity sold across all items in this import.
    /// </summary>
    public int TotalQuantity { get; private set; }

    /// <summary>
    /// Total value of sales in this import (optional, for reporting).
    /// </summary>
    public decimal? TotalValue { get; private set; }

    /// <summary>
    /// Import processing status: PENDING, PROCESSING, COMPLETED, FAILED.
    /// </summary>
    public string Status { get; private set; } = default!;

    /// <summary>
    /// Indicates if this import has been reversed/undone.
    /// </summary>
    public bool IsReversed { get; private set; }

    /// <summary>
    /// Date when the import was reversed.
    /// </summary>
    public DateTime? ReversedDate { get; private set; }

    /// <summary>
    /// User who reversed the import.
    /// </summary>
    public string? ReversedBy { get; private set; }

    /// <summary>
    /// Reason for reversing the import.
    /// </summary>
    public string? ReversalReason { get; private set; }

    /// <summary>
    /// User who processed the import.
    /// </summary>
    public string? ProcessedBy { get; private set; }

    /// <summary>
    /// Error message if import failed.
    /// </summary>
    public string? ErrorMessage { get; private set; }

    /// <summary>
    /// Navigation property to the warehouse/store.
    /// </summary>
    public Warehouse Warehouse { get; private set; } = default!;

    /// <summary>
    /// Collection of individual sale records in this import.
    /// </summary>
    public ICollection<SalesImportItem> Items { get; private set; } = new List<SalesImportItem>();

    private SalesImport() { }

    private SalesImport(
        DefaultIdType id,
        string importNumber,
        DateTime importDate,
        DateTime salesPeriodFrom,
        DateTime salesPeriodTo,
        DefaultIdType warehouseId,
        string fileName,
        string? notes,
        string? processedBy)
    {
        // Validations
        if (string.IsNullOrWhiteSpace(importNumber))
            throw new ArgumentException("ImportNumber is required", nameof(importNumber));
        
        if (importNumber.Length > 100)
            throw new ArgumentException("ImportNumber must not exceed 100 characters", nameof(importNumber));

        if (importDate == default)
            throw new ArgumentException("ImportDate is required", nameof(importDate));

        if (salesPeriodFrom == default)
            throw new ArgumentException("SalesPeriodFrom is required", nameof(salesPeriodFrom));

        if (salesPeriodTo == default)
            throw new ArgumentException("SalesPeriodTo is required", nameof(salesPeriodTo));

        if (salesPeriodTo < salesPeriodFrom)
            throw new ArgumentException("SalesPeriodTo must be greater than or equal to SalesPeriodFrom", nameof(salesPeriodTo));

        if (warehouseId == default)
            throw new ArgumentException("WarehouseId is required", nameof(warehouseId));

        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException("FileName is required", nameof(fileName));
        if (fileName.Length > 255)
            throw new ArgumentException("FileName must not exceed 255 characters", nameof(fileName));

        Id = id;
        ImportNumber = importNumber;
        ImportDate = importDate;
        SalesPeriodFrom = salesPeriodFrom;
        SalesPeriodTo = salesPeriodTo;
        WarehouseId = warehouseId;
        FileName = fileName;
        TotalRecords = 0;
        ProcessedRecords = 0;
        ErrorRecords = 0;
        TotalQuantity = 0;
        Status = "PENDING";
        IsReversed = false;
        ProcessedBy = processedBy;
        Notes = notes;
    }

    /// <summary>
    /// Creates a new sales import batch.
    /// </summary>
    public static SalesImport Create(
        string importNumber,
        DateTime importDate,
        DateTime salesPeriodFrom,
        DateTime salesPeriodTo,
        DefaultIdType warehouseId,
        string fileName,
        string? notes = null,
        string? processedBy = null)
    {
        var salesImport = new SalesImport(
            DefaultIdType.NewGuid(),
            importNumber,
            importDate,
            salesPeriodFrom,
            salesPeriodTo,
            warehouseId,
            fileName,
            notes,
            processedBy);

        return salesImport;
    }

    /// <summary>
    /// Updates the import statistics after processing.
    /// </summary>
    public SalesImport UpdateStatistics(int totalRecords, int processedRecords, int errorRecords, int totalQuantity, decimal? totalValue = null)
    {
        if (totalRecords < 0)
            throw new ArgumentException("TotalRecords must be zero or greater", nameof(totalRecords));
        if (processedRecords < 0)
            throw new ArgumentException("ProcessedRecords must be zero or greater", nameof(processedRecords));
        if (errorRecords < 0)
            throw new ArgumentException("ErrorRecords must be zero or greater", nameof(errorRecords));
        if (totalQuantity < 0)
            throw new ArgumentException("TotalQuantity must be zero or greater", nameof(totalQuantity));
        if (totalValue.HasValue && totalValue < 0)
            throw new ArgumentException("TotalValue must be zero or greater", nameof(totalValue));

        TotalRecords = totalRecords;
        ProcessedRecords = processedRecords;
        ErrorRecords = errorRecords;
        TotalQuantity = totalQuantity;
        TotalValue = totalValue;

        return this;
    }

    /// <summary>
    /// Updates the import status.
    /// </summary>
    public SalesImport UpdateStatus(string status, string? errorMessage = null)
    {
        var allowedStatuses = new[] { "PENDING", "PROCESSING", "COMPLETED", "FAILED" };
        if (string.IsNullOrWhiteSpace(status) || !allowedStatuses.Contains(status.ToUpper()))
            throw new ArgumentException($"Invalid status: {status}. Allowed values: {string.Join(", ", allowedStatuses)}", nameof(status));

        Status = status.ToUpper();
        ErrorMessage = errorMessage;

        return this;
    }

    /// <summary>
    /// Reverses the import, creating offsetting inventory transactions.
    /// </summary>
    public SalesImport Reverse(string reason, string? reversedBy = null)
    {
        if (IsReversed)
            throw new InvalidOperationException("Import has already been reversed");

        if (Status != "COMPLETED")
            throw new InvalidOperationException("Only completed imports can be reversed");

        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("Reversal reason is required", nameof(reason));
        if (reason.Length > 500)
            throw new ArgumentException("Reversal reason must not exceed 500 characters", nameof(reason));

        IsReversed = true;
        ReversedDate = DateTime.UtcNow;
        ReversedBy = reversedBy;
        ReversalReason = reason;

        return this;
    }

    /// <summary>
    /// Updates notes for the import.
    /// </summary>
    public SalesImport UpdateNotes(string? notes)
    {
        if (notes?.Length > 1000)
            throw new ArgumentException("Notes must not exceed 1000 characters", nameof(notes));

        Notes = notes;
        return this;
    }
}

