namespace Store.Domain;

/// <summary>
/// Represents a payment collected for a POS sale.
/// </summary>
/// <remarks>
/// Use cases:
/// - Record cash, card, and other payment methods during checkout.
/// - Support split payments across multiple methods.
/// - Track payment references for card transactions and reconciliation.
/// - Enable refund processing by maintaining payment history.
/// </remarks>
/// <seealso cref="Store.Domain.Events.PosPaymentAdded"/>
/// <seealso cref="Store.Domain.Exceptions.PosSale.PosSaleNotFoundException"/>
/// <seealso cref="Store.Domain.Exceptions.PosSale.PosSaleInvalidOperationException"/>
public sealed class PosPayment : AuditableEntity
{
    /// <summary>
    /// Parent POS sale identifier.
    /// Example: a <see cref="PosSale"/> Id.
    /// </summary>
    public DefaultIdType PosSaleId { get; private set; }

    /// <summary>
    /// Payment method used.
    /// Example: "Cash", "Card", "GiftCard", "Check". Max length: 50.
    /// </summary>
    public string Method { get; private set; } = default!; // Cash, Card, GiftCard, etc.

    /// <summary>
    /// Payment amount received. Must be positive.
    /// Example: 50.00, 105.99.
    /// </summary>
    public decimal Amount { get; private set; }

    /// <summary>
    /// Optional payment reference for tracking.
    /// Example: "AUTH123456" for card transactions, check numbers. Max length: 255.
    /// </summary>
    public string? Reference { get; private set; }

    private PosPayment() { }

    private PosPayment(DefaultIdType id, DefaultIdType posSaleId, string method, decimal amount, string? reference)
    {
        if (string.IsNullOrWhiteSpace(method)) throw new ArgumentException("Method is required", nameof(method));
        if (method.Length > 50) throw new ArgumentException("Method must not exceed 50 characters", nameof(method));
        if (amount <= 0m) throw new ArgumentException("Amount must be positive", nameof(amount));
        if (reference is { Length: > 255 }) throw new ArgumentException("Reference must not exceed 255 characters", nameof(reference));

        Id = id;
        PosSaleId = posSaleId;
        Method = method;
        Amount = amount;
        Reference = reference;
    }

    /// <summary>
    /// Factory to create a POS payment.
    /// </summary>
    /// <param name="posSaleId">Parent sale id.</param>
    /// <param name="method">Payment method. Example: "Cash", "Card".</param>
    /// <param name="amount">Payment amount. Example: 50.00.</param>
    /// <param name="reference">Optional reference. Example: "AUTH123456".</param>
    public static PosPayment Create(DefaultIdType posSaleId, string method, decimal amount, string? reference = null)
        => new(DefaultIdType.NewGuid(), posSaleId, method, amount, reference);
}
