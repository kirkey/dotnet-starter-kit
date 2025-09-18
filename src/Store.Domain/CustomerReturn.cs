namespace Store.Domain;

/// <summary>
/// Represents a customer return/refund transaction. May reference an original POS sale.
/// </summary>
/// <remarks>
/// Use cases:
/// - Process customer returns for defective, unwanted, or incorrect items.
/// - Issue refunds with proper item tracking and inventory adjustments.
/// - Link returns to original sales for better auditing.
/// </remarks>
/// <seealso cref="Store.Domain.Events.CustomerReturnCreated"/>
/// <seealso cref="Store.Domain.Events.CustomerReturnItemAdded"/>
/// <seealso cref="Store.Domain.Exceptions.CustomerReturn.CustomerReturnNotFoundException"/>
public sealed class CustomerReturn : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Unique return number for tracking and reference.
    /// Example: "RET-2025-09-001". Max length: 100.
    /// </summary>
    public string ReturnNumber { get; private set; } = default!;

    /// <summary>
    /// Date when the return was processed.
    /// Example: 2025-09-18T14:30:00Z. Defaults to current UTC if unspecified.
    /// </summary>
    public DateTime ReturnDate { get; private set; }

    /// <summary>
    /// Optional reference to the original POS sale being returned.
    /// Example: an existing <see cref="PosSale"/> Id or null for non-POS returns.
    /// </summary>
    public DefaultIdType? PosSaleId { get; private set; }

    /// <summary>
    /// Reason for the return provided by customer or staff.
    /// Example: "Defective product", "Wrong size". Max length: 500.
    /// </summary>
    public string Reason { get; private set; } = default!;

    /// <summary>
    /// Total refund amount calculated from returned items.
    /// Example: 45.99. Computed from line items. Default: 0.00.
    /// </summary>
    public decimal RefundAmount { get; private set; }

    /// <summary>
    /// Line items being returned with quantities and prices.
    /// Example count: 0 at creation.
    /// </summary>
    public ICollection<CustomerReturnItem> Items { get; private set; } = new List<CustomerReturnItem>();

    private CustomerReturn() { }

    private CustomerReturn(DefaultIdType id, string returnNumber, DateTime returnDate, string reason, DefaultIdType? posSaleId)
    {
        if (string.IsNullOrWhiteSpace(returnNumber)) throw new ArgumentException("ReturnNumber is required", nameof(returnNumber));
        if (returnNumber.Length > 100) throw new ArgumentException("ReturnNumber must not exceed 100 characters", nameof(returnNumber));
        if (string.IsNullOrWhiteSpace(reason)) throw new ArgumentException("Reason is required", nameof(reason));
        if (reason.Length > 500) throw new ArgumentException("Reason must not exceed 500 characters", nameof(reason));

        Id = id;
        ReturnNumber = returnNumber;
        ReturnDate = returnDate == default ? DateTime.UtcNow : returnDate;
        Reason = reason;
        PosSaleId = posSaleId;
        QueueDomainEvent(new CustomerReturnCreated { CustomerReturn = this });
    }

    /// <summary>
    /// Factory to create a new customer return.
    /// </summary>
    /// <param name="returnNumber">Unique return number. Example: "RET-2025-09-001".</param>
    /// <param name="returnDate">Return date. Defaults to now if unspecified.</param>
    /// <param name="reason">Return reason. Example: "Defective product".</param>
    /// <param name="posSaleId">Optional original sale reference.</param>
    public static CustomerReturn Create(string returnNumber, DateTime returnDate, string reason, DefaultIdType? posSaleId = null)
        => new(DefaultIdType.NewGuid(), returnNumber, returnDate, reason, posSaleId);

    /// <summary>
    /// Adds an item to the return and updates the total refund amount.
    /// </summary>
    /// <param name="groceryItemId">Item being returned.</param>
    /// <param name="name">Item name snapshot. Example: "Bananas".</param>
    /// <param name="quantity">Return quantity. Example: 2.</param>
    /// <param name="unitPrice">Price per unit for refund. Example: 2.49.</param>
    public CustomerReturn AddItem(DefaultIdType groceryItemId, string name, int quantity, decimal unitPrice)
    {
        if (quantity <= 0) throw new ArgumentException("Quantity must be positive", nameof(quantity));
        if (unitPrice < 0m) throw new ArgumentException("UnitPrice must be zero or greater", nameof(unitPrice));
        var item = CustomerReturnItem.Create(Id, groceryItemId, name, quantity, unitPrice);
        Items.Add(item);
        RefundAmount += unitPrice * quantity;
        QueueDomainEvent(new CustomerReturnItemAdded { CustomerReturn = this, Item = item });
        return this;
    }
}
