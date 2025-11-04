namespace Accounting.Domain.Entities;

/// <summary>
/// Represents a single line item within an invoice for detailed revenue tracking and account coding.
/// </summary>
/// <remarks>
/// Use cases:
/// - Track individual charges on customer invoices with quantity and pricing.
/// - Support account-level revenue coding for GL posting and reporting.
/// - Enable detailed revenue analysis by line item and product/service.
/// - Support line-level modifications before invoice is finalized.
/// - Track line-level audit trail for compliance and dispute resolution.
/// - Enable flexible billing with mixed product and service items.
/// 
/// Default values:
/// - InvoiceId: required reference to parent invoice
/// - Description: required charge description (max 500 chars)
/// - Quantity: required positive decimal (example: 10.0 units, 2.5 hours)
/// - UnitPrice: required non-negative decimal (example: 50.00 per unit)
/// - TotalPrice: calculated (Quantity × UnitPrice)
/// - AccountId: optional GL account for revenue coding
/// 
/// Business rules:
/// - Description cannot be empty and max 500 characters
/// - Quantity must be positive
/// - UnitPrice cannot be negative
/// - TotalPrice is automatically calculated
/// - Cannot modify line items after invoice is sent or paid
/// - AccountId should reference valid ChartOfAccount (revenue account)
/// - Updates recalculate TotalPrice automatically
/// </remarks>
public class InvoiceLineItem : AuditableEntity, IAggregateRoot
{
    private const int MaxDescriptionLength = 500;

    /// <summary>
    /// Parent invoice identifier.
    /// Links to the Invoice entity that owns this line item.
    /// </summary>
    public DefaultIdType InvoiceId { get; private set; }

    // Description property inherited from AuditableEntity base class

    /// <summary>
    /// Quantity of items or units of service.
    /// Example: 10.0 for 10 units, 2.5 for 2.5 hours, 1500.0 for kWh. Must be positive.
    /// </summary>
    public decimal Quantity { get; private set; }

    /// <summary>
    /// Price per unit or rate.
    /// Example: 50.00 for $50 per unit, 0.15 for $0.15 per kWh. Cannot be negative.
    /// </summary>
    public decimal UnitPrice { get; private set; }

    /// <summary>
    /// Total line amount (Quantity × UnitPrice).
    /// Example: 500.00 for 10 units at $50 each, 225.00 for 1500 kWh at $0.15.
    /// Automatically calculated and updated when quantity or price changes.
    /// </summary>
    public decimal TotalPrice { get; private set; }

    /// <summary>
    /// Optional general ledger account identifier for revenue coding.
    /// Links to ChartOfAccount entity if specified.
    /// Example: AccountId for "Electric Revenue" or "Service Fee Revenue".
    /// </summary>
    public DefaultIdType? AccountId { get; private set; }

    // EF Core constructor
    private InvoiceLineItem()
    {
    }

    private InvoiceLineItem(DefaultIdType invoiceId, string description, decimal quantity, decimal unitPrice, DefaultIdType? accountId)
    {
        if (invoiceId == default)
            throw new ArgumentException("InvoiceId is required", nameof(invoiceId));

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description is required", nameof(description));

        var desc = description.Trim();
        if (desc.Length > MaxDescriptionLength)
            throw new ArgumentException($"Description cannot exceed {MaxDescriptionLength} characters", nameof(description));

        if (quantity <= 0)
            throw new ArgumentException("Quantity must be positive", nameof(quantity));

        if (unitPrice < 0)
            throw new ArgumentException("Unit price cannot be negative", nameof(unitPrice));

        InvoiceId = invoiceId;
        Description = desc;
        Quantity = quantity;
        UnitPrice = unitPrice;
        TotalPrice = quantity * unitPrice;
        AccountId = accountId;
    }

    /// <summary>
    /// Factory method to create a new invoice line item with validation.
    /// </summary>
    /// <param name="invoiceId">Parent invoice identifier (required)</param>
    /// <param name="description">Charge description (required, max 500 chars)</param>
    /// <param name="quantity">Quantity (must be positive)</param>
    /// <param name="unitPrice">Unit price (cannot be negative)</param>
    /// <param name="accountId">Optional GL account identifier</param>
    /// <returns>New InvoiceLineItem instance</returns>
    /// <exception cref="ArgumentException">Thrown if validation fails</exception>
    public static InvoiceLineItem Create(DefaultIdType invoiceId, string description, decimal quantity, decimal unitPrice, DefaultIdType? accountId = null)
    {
        return new InvoiceLineItem(invoiceId, description, quantity, unitPrice, accountId);
    }

    /// <summary>
    /// Update line item details. Recalculates total price if quantity or price changes.
    /// </summary>
    /// <param name="description">Updated description (optional)</param>
    /// <param name="quantity">Updated quantity (optional, must be positive)</param>
    /// <param name="unitPrice">Updated unit price (optional, cannot be negative)</param>
    /// <param name="accountId">Updated account ID (optional)</param>
    /// <returns>This instance for fluent chaining</returns>
    /// <exception cref="ArgumentException">Thrown if validation fails</exception>
    public InvoiceLineItem Update(string? description, decimal? quantity, decimal? unitPrice, DefaultIdType? accountId)
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

        // Recalculate total price if quantity or price changed
        if (isUpdated)
        {
            TotalPrice = Quantity * UnitPrice;
        }

        return this;
    }
}

