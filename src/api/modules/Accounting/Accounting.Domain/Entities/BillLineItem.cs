namespace Accounting.Domain.Entities;

/// <summary>
/// Represents a single line item within a vendor bill for detailed expense tracking and account coding.
/// </summary>
/// <remarks>
/// Use cases:
/// - Track individual items or services on a vendor bill with quantity and pricing.
/// - Support account-level expense coding for GL posting and reporting.
/// - Enable detailed expense analysis by line item.
/// - Facilitate 3-way matching with purchase orders and receipts.
/// - Support line-level modifications before bill approval.
/// - Track line-level audit trail for compliance and dispute resolution.
/// 
/// Default values:
/// - BillId: required reference to parent bill
/// - Description: required item/service description (max 500 chars)
/// - Quantity: required positive decimal (example: 10.0 units, 2.5 hours)
/// - UnitPrice: required non-negative decimal (example: 50.00 per unit)
/// - LineTotal: calculated (Quantity × UnitPrice)
/// - AccountId: optional GL account for expense coding
/// 
/// Business rules:
/// - Description cannot be empty and max 500 characters
/// - Quantity must be positive
/// - UnitPrice cannot be negative
/// - LineTotal is automatically calculated
/// - Cannot modify line items after bill is approved
/// - AccountId should reference valid ChartOfAccount
/// - Updates recalculate LineTotal automatically
/// </remarks>
public class BillLineItem : AuditableEntity, IAggregateRoot
{
    private const int MaxDescriptionLength = 500;

    /// <summary>
    /// Parent bill identifier.
    /// Links to the Bill entity that owns this line item.
    /// </summary>
    public DefaultIdType BillId { get; private set; }

    /// <summary>
    /// Description of the item or service.
    /// Example: "Office supplies - Paper reams", "Consulting services - Project X".
    /// Max length: 500 characters. Required field.
    /// </summary>
    public new string Description { get; private set; } = string.Empty;

    /// <summary>
    /// Quantity of items or hours of service.
    /// Example: 10.0 for 10 units, 2.5 for 2.5 hours. Must be positive.
    /// </summary>
    public decimal Quantity { get; private set; }

    /// <summary>
    /// Price per unit or hourly rate.
    /// Example: 50.00 for $50 per unit. Cannot be negative.
    /// </summary>
    public decimal UnitPrice { get; private set; }

    /// <summary>
    /// Total line amount (Quantity × UnitPrice).
    /// Example: 500.00 for 10 units at $50 each.
    /// Automatically calculated and updated when quantity or price changes.
    /// </summary>
    public decimal LineTotal { get; private set; }

    /// <summary>
    /// Optional general ledger account identifier for expense coding.
    /// Links to ChartOfAccount entity if specified.
    /// Example: AccountId for "Supplies Expense" or "Professional Services".
    /// </summary>
    public DefaultIdType? AccountId { get; private set; }

    // EF Core constructor
    private BillLineItem()
    {
    }

    private BillLineItem(DefaultIdType billId, string description, decimal quantity, decimal unitPrice, DefaultIdType? accountId)
    {
        if (billId == default)
            throw new ArgumentException("BillId is required", nameof(billId));

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description is required", nameof(description));

        var desc = description.Trim();
        if (desc.Length > MaxDescriptionLength)
            throw new ArgumentException($"Description cannot exceed {MaxDescriptionLength} characters", nameof(description));

        if (quantity <= 0)
            throw new ArgumentException("Quantity must be positive", nameof(quantity));

        if (unitPrice < 0)
            throw new ArgumentException("Unit price cannot be negative", nameof(unitPrice));

        BillId = billId;
        Description = desc;
        Quantity = quantity;
        UnitPrice = unitPrice;
        LineTotal = quantity * unitPrice;
        AccountId = accountId;
    }

    /// <summary>
    /// Factory method to create a new bill line item with validation.
    /// </summary>
    /// <param name="billId">Parent bill identifier (required)</param>
    /// <param name="description">Item description (required, max 500 chars)</param>
    /// <param name="quantity">Quantity (must be positive)</param>
    /// <param name="unitPrice">Unit price (cannot be negative)</param>
    /// <param name="accountId">Optional GL account identifier</param>
    /// <returns>New BillLineItem instance</returns>
    /// <exception cref="ArgumentException">Thrown if validation fails</exception>
    public static BillLineItem Create(DefaultIdType billId, string description, decimal quantity, decimal unitPrice, DefaultIdType? accountId = null)
    {
        return new BillLineItem(billId, description, quantity, unitPrice, accountId);
    }

    /// <summary>
    /// Update line item details. Recalculates line total if quantity or price changes.
    /// </summary>
    /// <param name="description">Updated description (optional)</param>
    /// <param name="quantity">Updated quantity (optional, must be positive)</param>
    /// <param name="unitPrice">Updated unit price (optional, cannot be negative)</param>
    /// <param name="accountId">Updated account ID (optional)</param>
    /// <returns>This instance for fluent chaining</returns>
    /// <exception cref="ArgumentException">Thrown if validation fails</exception>
    public BillLineItem Update(string? description, decimal? quantity, decimal? unitPrice, DefaultIdType? accountId)
    {
        bool isUpdated = false;

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

        if (accountId != AccountId)
        {
            AccountId = accountId;
            isUpdated = true;
        }

        // Recalculate line total if quantity or price changed
        if (isUpdated)
        {
            LineTotal = Quantity * UnitPrice;
        }

        return this;
    }
}

