namespace Accounting.Domain.Entities;

/// <summary>
/// Represents a single line item within a vendor bill, detailing individual charges or expenses.
/// </summary>
/// <remarks>
/// Use cases:
/// - Detail specific goods or services on a vendor bill.
/// - Assign line items to different expense accounts or cost centers.
/// - Track quantity, unit price, and extended amounts.
/// - Support tax calculations and project/department allocations.
/// 
/// Default values:
/// - Quantity: 1
/// - UnitPrice: 0
/// - Amount: 0 (calculated as Quantity * UnitPrice)
/// 
/// Business rules:
/// - Must be associated with a Bill (BillId is required).
/// - Description is required and cannot exceed 500 characters.
/// - Quantity must be positive (> 0).
/// - UnitPrice and Amount can be zero but not negative.
/// - Chart of Account ID is required for GL posting.
/// - Amount is typically calculated as Quantity * UnitPrice.
/// </remarks>
/// <seealso cref="Accounting.Domain.Entities.Bill"/>
public class BillLineItem : AuditableEntity, IAggregateRoot
{
    private const int MaxDescriptionLength = 500;

    /// <summary>
    /// Parent bill identifier - REQUIRED.
    /// Reference to the bill that contains this line item.
    /// </summary>
    public DefaultIdType BillId { get; private set; }

    /// <summary>
    /// Line number for ordering and display.
    /// Example: 1, 2, 3. Used for maintaining line item sequence.
    /// </summary>
    public int LineNumber { get; private set; }

    /// <summary>
    /// Description of the goods or services.
    /// Example: "Office Supplies - Paper Reams", "Consulting Services - October".
    /// </summary>
    public new string Description { get; private set; }

    /// <summary>
    /// Quantity of items or units.
    /// Example: 10 (for 10 units), 1 (for a single service). Must be positive.
    /// Default: 1.
    /// </summary>
    public decimal Quantity { get; private set; }

    /// <summary>
    /// Price per unit.
    /// Example: 25.50 per unit, 1500.00 per service. Can be zero but not negative.
    /// Default: 0.
    /// </summary>
    public decimal UnitPrice { get; private set; }

    /// <summary>
    /// Extended line amount.
    /// Typically calculated as Quantity * UnitPrice. Can include discounts or adjustments.
    /// </summary>
    public decimal Amount { get; private set; }

    /// <summary>
    /// Chart of account identifier for general ledger posting.
    /// Example: links to expense account like "Office Supplies" or "Professional Fees".
    /// REQUIRED for GL integration.
    /// </summary>
    public DefaultIdType ChartOfAccountId { get; private set; }

    /// <summary>
    /// Optional tax code identifier for tax calculations.
    /// Example: links to sales tax, VAT, or other tax codes.
    /// </summary>
    public DefaultIdType? TaxCodeId { get; private set; }

    /// <summary>
    /// Tax amount for this line item.
    /// Example: 2.50 for sales tax. Default: 0.
    /// </summary>
    public decimal TaxAmount { get; private set; }

    /// <summary>
    /// Optional project identifier for project costing.
    /// Example: links to capital project or job costing.
    /// </summary>
    public DefaultIdType? ProjectId { get; private set; }

    /// <summary>
    /// Optional cost center or department identifier.
    /// Example: links to department for expense allocation.
    /// </summary>
    public DefaultIdType? CostCenterId { get; private set; }

    /// <summary>
    /// Additional notes for this line item.
    /// Example: "Approved by manager", "Price includes shipping".
    /// </summary>
    public string? Notes { get; private set; }

    // EF Core parameterless constructor
    private BillLineItem()
    {
        Description = string.Empty;
    }

    // Private constructor with required parameters
    private BillLineItem(
        DefaultIdType billId,
        int lineNumber,
        string description,
        decimal quantity,
        decimal unitPrice,
        decimal amount,
        DefaultIdType chartOfAccountId,
        DefaultIdType? taxCodeId = null,
        decimal taxAmount = 0,
        DefaultIdType? projectId = null,
        DefaultIdType? costCenterId = null,
        string? notes = null)
    {
        if (billId == default)
            throw new ArgumentException("Bill ID is required", nameof(billId));

        if (lineNumber <= 0)
            throw new ArgumentException("Line number must be positive", nameof(lineNumber));

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description is required", nameof(description));

        var desc = description.Trim();
        if (desc.Length > MaxDescriptionLength)
            throw new ArgumentException($"Description cannot exceed {MaxDescriptionLength} characters", nameof(description));

        if (quantity <= 0)
            throw new ArgumentException("Quantity must be positive", nameof(quantity));

        if (unitPrice < 0)
            throw new ArgumentException("Unit price cannot be negative", nameof(unitPrice));

        if (amount < 0)
            throw new ArgumentException("Amount cannot be negative", nameof(amount));

        if (chartOfAccountId == default)
            throw new ArgumentException("Chart of Account ID is required", nameof(chartOfAccountId));

        if (taxAmount < 0)
            throw new ArgumentException("Tax amount cannot be negative", nameof(taxAmount));

        BillId = billId;
        LineNumber = lineNumber;
        Description = desc;
        Quantity = quantity;
        UnitPrice = unitPrice;
        Amount = amount;
        ChartOfAccountId = chartOfAccountId;
        TaxCodeId = taxCodeId;
        TaxAmount = taxAmount;
        ProjectId = projectId;
        CostCenterId = costCenterId;
        Notes = notes?.Trim();
    }

    /// <summary>
    /// Factory method to create a new bill line item.
    /// </summary>
    /// <param name="billId">Parent bill identifier.</param>
    /// <param name="lineNumber">Line number for ordering.</param>
    /// <param name="description">Description of goods/services.</param>
    /// <param name="quantity">Quantity of items.</param>
    /// <param name="unitPrice">Price per unit.</param>
    /// <param name="amount">Extended line amount.</param>
    /// <param name="chartOfAccountId">GL account for posting.</param>
    /// <param name="taxCodeId">Optional tax code.</param>
    /// <param name="taxAmount">Optional tax amount.</param>
    /// <param name="projectId">Optional project reference.</param>
    /// <param name="costCenterId">Optional cost center reference.</param>
    /// <param name="notes">Optional notes.</param>
    /// <returns>New BillLineItem instance.</returns>
    public static BillLineItem Create(
        DefaultIdType billId,
        int lineNumber,
        string description,
        decimal quantity,
        decimal unitPrice,
        decimal amount,
        DefaultIdType chartOfAccountId,
        DefaultIdType? taxCodeId = null,
        decimal taxAmount = 0,
        DefaultIdType? projectId = null,
        DefaultIdType? costCenterId = null,
        string? notes = null)
    {
        return new BillLineItem(
            billId, lineNumber, description, quantity, unitPrice, amount,
            chartOfAccountId, taxCodeId, taxAmount, projectId, costCenterId, notes);
    }

    /// <summary>
    /// Update line item properties.
    /// </summary>
    public BillLineItem Update(
        int? lineNumber,
        string? description,
        decimal? quantity,
        decimal? unitPrice,
        decimal? amount,
        DefaultIdType? chartOfAccountId,
        DefaultIdType? taxCodeId,
        decimal? taxAmount,
        DefaultIdType? projectId,
        DefaultIdType? costCenterId,
        string? notes)
    {
        bool isUpdated = false;

        if (lineNumber.HasValue && LineNumber != lineNumber.Value)
        {
            if (lineNumber.Value <= 0)
                throw new ArgumentException("Line number must be positive");
            LineNumber = lineNumber.Value;
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(description) && Description != description)
        {
            var desc = description.Trim();
            if (desc.Length > MaxDescriptionLength)
                throw new ArgumentException($"Description cannot exceed {MaxDescriptionLength} characters");
            Description = desc;
            isUpdated = true;
        }

        if (quantity.HasValue && Quantity != quantity.Value)
        {
            if (quantity.Value <= 0)
                throw new ArgumentException("Quantity must be positive");
            Quantity = quantity.Value;
            isUpdated = true;
        }

        if (unitPrice.HasValue && UnitPrice != unitPrice.Value)
        {
            if (unitPrice.Value < 0)
                throw new ArgumentException("Unit price cannot be negative");
            UnitPrice = unitPrice.Value;
            isUpdated = true;
        }

        if (amount.HasValue && Amount != amount.Value)
        {
            if (amount.Value < 0)
                throw new ArgumentException("Amount cannot be negative");
            Amount = amount.Value;
            isUpdated = true;
        }

        if (chartOfAccountId.HasValue && ChartOfAccountId != chartOfAccountId.Value)
        {
            if (chartOfAccountId.Value == default)
                throw new ArgumentException("Chart of Account ID is required");
            ChartOfAccountId = chartOfAccountId.Value;
            isUpdated = true;
        }

        if (taxCodeId != TaxCodeId)
        {
            TaxCodeId = taxCodeId;
            isUpdated = true;
        }

        if (taxAmount.HasValue && TaxAmount != taxAmount.Value)
        {
            if (taxAmount.Value < 0)
                throw new ArgumentException("Tax amount cannot be negative");
            TaxAmount = taxAmount.Value;
            isUpdated = true;
        }

        if (projectId != ProjectId)
        {
            ProjectId = projectId;
            isUpdated = true;
        }

        if (costCenterId != CostCenterId)
        {
            CostCenterId = costCenterId;
            isUpdated = true;
        }

        if (notes != Notes)
        {
            Notes = notes?.Trim();
            isUpdated = true;
        }

        return this;
    }

    /// <summary>
    /// Recalculate the line amount based on quantity and unit price.
    /// </summary>
    public BillLineItem RecalculateAmount()
    {
        Amount = Quantity * UnitPrice;
        return this;
    }
}

