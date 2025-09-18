namespace Store.Domain;

/// <summary>
/// Represents a point-of-sale (POS) sale transaction captured at the register.
/// Use cases:
/// - Record quick in-store sales with mixed payment methods.
/// - Track line-level discounts and taxes in real time during checkout.
/// - Support end-of-day reconciliation and reporting.
/// </summary>
/// <seealso cref="Store.Domain.Events.PosSaleCreated"/>
/// <seealso cref="Store.Domain.Events.PosSaleItemAdded"/>
/// <seealso cref="Store.Domain.Events.PosPaymentAdded"/>
/// <seealso cref="Store.Domain.Events.PosSaleCompleted"/>
/// <seealso cref="Store.Domain.Events.PosSaleVoided"/>
/// <seealso cref="Store.Domain.Exceptions.PosSale.PosSaleNotFoundException"/>
/// <seealso cref="Store.Domain.Exceptions.PosSale.PosSaleInvalidOperationException"/>
public sealed class PosSale : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Human-friendly sale number/identifier.
    /// Example: <c>T-2025-09-0001</c>.
    /// </summary>
    public string SaleNumber { get; private set; } = default!;

    /// <summary>
    /// Date and time when the sale occurred.
    /// Example: <c>2025-09-18T10:15:00Z</c>.
    /// </summary>
    public DateTime SaleDate { get; private set; }

    /// <summary>
    /// Optional customer associated with this sale.
    /// Example: <c>00000000-0000-0000-0000-000000000001</c> or <c>null</c>.
    /// </summary>
    public DefaultIdType? CustomerId { get; private set; }

    /// <summary>
    /// Current sale status.
    /// Allowed values: <c>Open</c>, <c>Completed</c>, <c>Voided</c>.
    /// Default: <c>Open</c>.
    /// </summary>
    public string Status { get; private set; } = "Open";

    /// <summary>
    /// Line items included in the sale.
    /// Example count: <c>0</c> at creation.
    /// </summary>
    public ICollection<PosSaleItem> Items { get; private set; } = new List<PosSaleItem>();

    /// <summary>
    /// Payments collected for the sale.
    /// Example: one Cash and one Card payment.
    /// </summary>
    public ICollection<PosPayment> Payments { get; private set; } = new List<PosPayment>();

    /// <summary>
    /// Computed sub total before taxes and discounts.
    /// Example: <c>100.00</c>.
    /// </summary>
    public decimal SubTotal { get; private set; }

    /// <summary>
    /// Total discount applied across line items and order-level discounts.
    /// Example: <c>5.00</c>.
    /// </summary>
    public decimal DiscountTotal { get; private set; }

    /// <summary>
    /// Total tax amount.
    /// Example: <c>10.00</c>.
    /// </summary>
    public decimal TaxTotal { get; private set; }

    /// <summary>
    /// Grand total to be paid by the customer.
    /// Example: <c>105.00</c>.
    /// </summary>
    public decimal TotalAmount { get; private set; }

    /// <summary>
    /// Total amount successfully paid by the customer.
    /// Example: <c>105.00</c> when fully paid.
    /// </summary>
    public decimal PaidAmount { get; private set; }

    /// <summary>
    /// Amount still due (<c>TotalAmount - PaidAmount</c>).
    /// Example: <c>0.00</c> when fully paid.
    /// </summary>
    public decimal AmountDue => Math.Max(0, TotalAmount - PaidAmount);

    private PosSale() { }

    private PosSale(DefaultIdType id, string saleNumber, DateTime saleDate, DefaultIdType? customerId)
    {
        if (string.IsNullOrWhiteSpace(saleNumber)) throw new ArgumentException("SaleNumber is required", nameof(saleNumber));
        if (saleNumber.Length > 100) throw new ArgumentException("SaleNumber must not exceed 100 characters", nameof(saleNumber));

        Id = id;
        SaleNumber = saleNumber;
        SaleDate = saleDate == default ? DateTime.UtcNow : saleDate;
        CustomerId = customerId;
        Status = "Open";

        QueueDomainEvent(new PosSaleCreated { PosSale = this });
    }

    /// <summary>
    /// Factory to create a new POS sale in <c>Open</c> status.
    /// </summary>
    /// <param name="saleNumber">Example: <c>T-2025-09-0001</c>.</param>
    /// <param name="saleDate">Example: <c>2025-09-18T10:15:00Z</c>.</param>
    /// <param name="customerId">Optional customer id.</param>
    public static PosSale Create(string saleNumber, DateTime saleDate, DefaultIdType? customerId = null)
        => new(DefaultIdType.NewGuid(), saleNumber, saleDate, customerId);

    /// <summary>
    /// Add an item to this sale and update totals.
    /// Use when scanning or entering an item during checkout.
    /// </summary>
    /// <param name="groceryItemId">Example: an existing <c>GroceryItem.Id</c>.</param>
    /// <param name="name">Example: <c>Bananas</c>.</param>
    /// <param name="unitPrice">Example: <c>2.49</c>.</param>
    /// <param name="quantity">Example: <c>3</c>.</param>
    /// <param name="discount">Example: <c>0.50</c>.</param>
    /// <param name="tax">Example: <c>0.45</c>.</param>
    public PosSale AddItem(DefaultIdType groceryItemId, string name, decimal unitPrice, int quantity, decimal discount = 0m, decimal tax = 0m)
    {
        EnsureOpen();
        if (quantity <= 0) throw new ArgumentException("Quantity must be positive", nameof(quantity));
        if (unitPrice < 0m) throw new ArgumentException("Unit price must be zero or greater", nameof(unitPrice));
        if (discount < 0m) throw new ArgumentException("Discount cannot be negative", nameof(discount));
        if (tax < 0m) throw new ArgumentException("Tax cannot be negative", nameof(tax));

        var item = PosSaleItem.Create(Id, groceryItemId, name, unitPrice, quantity, discount, tax);
        Items.Add(item);

        RecalculateTotals();
        QueueDomainEvent(new PosSaleItemAdded { PosSale = this, Item = item });
        return this;
    }

    /// <summary>
    /// Add a payment to the sale (e.g., Cash, Card) and increase <see cref="PaidAmount"/>.
    /// </summary>
    /// <param name="method">Example: <c>Cash</c> or <c>Card</c>.</param>
    /// <param name="amount">Example: <c>50.00</c>.</param>
    /// <param name="reference">Example: card auth code like <c>AUTH123</c>.</param>
    public PosSale AddPayment(string method, decimal amount, string? reference = null)
    {
        EnsureOpen();
        if (string.IsNullOrWhiteSpace(method)) throw new ArgumentException("Payment method is required", nameof(method));
        if (method.Length > 50) throw new ArgumentException("Payment method must not exceed 50 characters", nameof(method));
        if (amount <= 0m) throw new ArgumentException("Payment amount must be positive", nameof(amount));

        var payment = PosPayment.Create(Id, method, amount, reference);
        Payments.Add(payment);

        PaidAmount += amount;
        QueueDomainEvent(new PosPaymentAdded { PosSale = this, Payment = payment });
        return this;
    }

    /// <summary>
    /// Complete the sale. Fails if there is outstanding <see cref="AmountDue"/>.
    /// </summary>
    public PosSale Complete()
    {
        EnsureOpen();
        if (AmountDue > 0m) throw new InvalidOperationException("Cannot complete sale with outstanding amount due.");
        Status = "Completed";
        QueueDomainEvent(new PosSaleCompleted { PosSale = this });
        return this;
    }

    /// <summary>
    /// Void an open sale that has no payments. For refunds, use customer returns instead.
    /// </summary>
    /// <param name="reason">Example: <c>Customer cancelled</c>.</param>
    public PosSale Void(string reason)
    {
        EnsureOpen();
        if (PaidAmount > 0m) throw new InvalidOperationException("Cannot void a sale with payments. Issue a return/refund instead.");
        Status = "Voided";
        QueueDomainEvent(new PosSaleVoided { PosSale = this, Reason = reason });
        return this;
    }

    private void EnsureOpen()
    {
        if (!string.Equals(Status, "Open", StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException($"Sale is not open. Current status: {Status}");
    }

    private void RecalculateTotals()
    {
        SubTotal = Items.Sum(i => i.UnitPrice * i.Quantity);
        DiscountTotal = Items.Sum(i => i.Discount);
        TaxTotal = Items.Sum(i => i.Tax);
        TotalAmount = SubTotal - DiscountTotal + TaxTotal;
    }
}
