namespace Store.Domain.Entities;

/// <summary>
/// Represents an individual sale record within a sales import batch from POS systems.
/// </summary>
/// <remarks>
/// Use cases:
/// - Store individual sale transactions from POS CSV files with date, item, and quantity information.
/// - Match POS items to inventory items using barcode or SKU for accurate inventory deduction.
/// - Track processing status for each sale record (success/error) for import reconciliation.
/// - Create corresponding inventory OUT transactions for successfully matched items.
/// - Support error tracking and resolution for unmatched items or validation failures.
/// - Enable detailed import audit trail with line-by-line processing results.
/// - Generate exception reports for manual review and correction of import errors.
/// 
/// Default values:
/// - SalesImportId: required parent import batch reference
/// - LineNumber: required line number in CSV file (for error tracking)
/// - SaleDate: required date of sale from POS system
/// - Barcode: required item barcode from POS (primary matching field)
/// - ItemName: optional item name from POS (for reference)
/// - QuantitySold: required quantity sold (must be positive)
/// - UnitPrice: optional unit price from POS (for value calculation)
/// - TotalAmount: optional total sale amount (Quantity × UnitPrice)
/// - ItemId: null initially, populated after successful barcode match
/// - InventoryTransactionId: null initially, populated after inventory transaction creation
/// - IsProcessed: false (indicates if record has been processed)
/// - HasError: false (indicates if processing encountered errors)
/// - ErrorMessage: null (populated with error details if processing fails)
/// 
/// Business rules:
/// - QuantitySold must be positive (negative returns handled separately)
/// - Barcode matching is case-insensitive
/// - If barcode match fails, processing stops for this record
/// - Each processed record creates one inventory OUT transaction
/// - Unit price and amount are informational only (not used for inventory valuation)
/// - Error records do not affect inventory but are logged for review
/// - Line numbers must be unique within an import batch
/// </remarks>
/// <seealso cref="Store.Domain.Entities.SalesImport"/>
/// <seealso cref="Store.Domain.Entities.Item"/>
/// <seealso cref="Store.Domain.Entities.InventoryTransaction"/>
public sealed class SalesImportItem : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Parent sales import batch identifier.
    /// </summary>
    public DefaultIdType SalesImportId { get; private set; }

    /// <summary>
    /// Line number in the CSV file (for error tracking and reference).
    /// </summary>
    public int LineNumber { get; private set; }

    /// <summary>
    /// Date of sale from POS system.
    /// </summary>
    public DateTime SaleDate { get; private set; }

    /// <summary>
    /// Item barcode from POS system (used for matching inventory items).
    /// </summary>
    public string Barcode { get; private set; } = default!;

    /// <summary>
    /// Item name from POS system (for reference and verification).
    /// </summary>
    public string? ItemName { get; private set; }

    /// <summary>
    /// Quantity sold in this transaction.
    /// </summary>
    public int QuantitySold { get; private set; }

    /// <summary>
    /// Unit price from POS system (optional, for reporting).
    /// </summary>
    public decimal? UnitPrice { get; private set; }

    /// <summary>
    /// Total sale amount (QuantitySold × UnitPrice).
    /// </summary>
    public decimal? TotalAmount { get; private set; }

    /// <summary>
    /// Matched inventory item identifier (populated after successful barcode match).
    /// </summary>
    public DefaultIdType? ItemId { get; private set; }

    /// <summary>
    /// Created inventory transaction identifier (populated after inventory adjustment).
    /// </summary>
    public DefaultIdType? InventoryTransactionId { get; private set; }

    /// <summary>
    /// Indicates if this record has been successfully processed.
    /// </summary>
    public bool IsProcessed { get; private set; }

    /// <summary>
    /// Indicates if processing encountered an error.
    /// </summary>
    public bool HasError { get; private set; }

    /// <summary>
    /// Error message if processing failed.
    /// </summary>
    public string? ErrorMessage { get; private set; }

    /// <summary>
    /// Navigation property to parent sales import batch.
    /// </summary>
    public SalesImport SalesImport { get; private set; } = default!;

    /// <summary>
    /// Navigation property to matched inventory item.
    /// </summary>
    public Item? Item { get; private set; }

    /// <summary>
    /// Navigation property to created inventory transaction.
    /// </summary>
    public InventoryTransaction? InventoryTransaction { get; private set; }

    private SalesImportItem() { }

    private SalesImportItem(
        DefaultIdType id,
        DefaultIdType salesImportId,
        int lineNumber,
        DateTime saleDate,
        string barcode,
        string? itemName,
        int quantitySold,
        decimal? unitPrice,
        string? notes)
    {
        // Validations
        if (salesImportId == default)
            throw new ArgumentException("SalesImportId is required", nameof(salesImportId));

        if (lineNumber <= 0)
            throw new ArgumentException("LineNumber must be greater than zero", nameof(lineNumber));

        if (saleDate == default)
            throw new ArgumentException("SaleDate is required", nameof(saleDate));

        if (string.IsNullOrWhiteSpace(barcode))
            throw new ArgumentException("Barcode is required", nameof(barcode));
        if (barcode.Length > 100)
            throw new ArgumentException("Barcode must not exceed 100 characters", nameof(barcode));

        if (!string.IsNullOrWhiteSpace(itemName) && itemName.Length > 255)
            throw new ArgumentException("ItemName must not exceed 255 characters", nameof(itemName));

        if (quantitySold <= 0)
            throw new ArgumentException("QuantitySold must be greater than zero", nameof(quantitySold));

        if (unitPrice.HasValue && unitPrice < 0)
            throw new ArgumentException("UnitPrice must be zero or greater", nameof(unitPrice));

        Id = id;
        SalesImportId = salesImportId;
        LineNumber = lineNumber;
        SaleDate = saleDate;
        Barcode = barcode.Trim();
        ItemName = itemName?.Trim();
        QuantitySold = quantitySold;
        UnitPrice = unitPrice;
        TotalAmount = unitPrice.HasValue ? unitPrice.Value * quantitySold : null;
        IsProcessed = false;
        HasError = false;
        Notes = notes;
    }

    /// <summary>
    /// Creates a new sales import item record.
    /// </summary>
    public static SalesImportItem Create(
        DefaultIdType salesImportId,
        int lineNumber,
        DateTime saleDate,
        string barcode,
        string? itemName,
        int quantitySold,
        decimal? unitPrice = null,
        string? notes = null)
    {
        var item = new SalesImportItem(
            DefaultIdType.NewGuid(),
            salesImportId,
            lineNumber,
            saleDate,
            barcode,
            itemName,
            quantitySold,
            unitPrice,
            notes);

        return item;
    }

    /// <summary>
    /// Marks the item as successfully processed with matched inventory item.
    /// </summary>
    public SalesImportItem MarkAsProcessed(DefaultIdType itemId, DefaultIdType inventoryTransactionId)
    {
        if (itemId == default)
            throw new ArgumentException("ItemId is required", nameof(itemId));
        if (inventoryTransactionId == default)
            throw new ArgumentException("InventoryTransactionId is required", nameof(inventoryTransactionId));

        ItemId = itemId;
        InventoryTransactionId = inventoryTransactionId;
        IsProcessed = true;
        HasError = false;
        ErrorMessage = null;

        return this;
    }

    /// <summary>
    /// Marks the item as failed with error message.
    /// </summary>
    public SalesImportItem MarkAsError(string errorMessage)
    {
        if (string.IsNullOrWhiteSpace(errorMessage))
            throw new ArgumentException("ErrorMessage is required", nameof(errorMessage));
        if (errorMessage.Length > 1000)
            throw new ArgumentException("ErrorMessage must not exceed 1000 characters", nameof(errorMessage));

        HasError = true;
        IsProcessed = false;
        ErrorMessage = errorMessage;

        return this;
    }

    /// <summary>
    /// Updates the matched item ID (after barcode lookup).
    /// </summary>
    public SalesImportItem SetItemId(DefaultIdType itemId)
    {
        if (itemId == default)
            throw new ArgumentException("ItemId is required", nameof(itemId));

        ItemId = itemId;
        return this;
    }
}

