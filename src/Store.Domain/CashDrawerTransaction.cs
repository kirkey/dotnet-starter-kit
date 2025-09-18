namespace Store.Domain;

/// <summary>
/// Represents a cash drawer movement (e.g., cash-in, cash-out, adjustment).
/// </summary>
/// <remarks>
/// Use cases:
/// - Record cash added/removed during a drawer session.
/// - Support reconciliation and discrepancy investigations.
/// </remarks>
/// <seealso cref="Store.Domain.Events.CashDrawerTransactionAdded"/>
/// <seealso cref="Store.Domain.Exceptions.CashDrawer.CashDrawerSessionNotFoundException"/>
public sealed class CashDrawerTransaction : AuditableEntity
{
    /// <summary>
    /// Parent cash drawer session id.
    /// Example: a <see cref="CashDrawerSession"/> Id.
    /// </summary>
    public DefaultIdType CashDrawerSessionId { get; private set; }

    /// <summary>
    /// Movement type. Example: "IN", "OUT", or "ADJUSTMENT".
    /// </summary>
    public string Type { get; private set; } = default!; // IN, OUT, ADJUSTMENT

    /// <summary>
    /// Transaction amount. Must be positive. Example: 50.00.
    /// </summary>
    public decimal Amount { get; private set; }

    /// <summary>
    /// Optional reason or note for the movement. Example: "Payout".
    /// </summary>
    public string? Reason { get; private set; }

    private CashDrawerTransaction() { }

    private CashDrawerTransaction(DefaultIdType id, DefaultIdType sessionId, string type, decimal amount, string? reason)
    {
        if (string.IsNullOrWhiteSpace(type)) throw new ArgumentException("Type is required", nameof(type));
        if (type.Length > 50) throw new ArgumentException("Type must not exceed 50 characters", nameof(type));
        if (amount <= 0m) throw new ArgumentException("Amount must be positive", nameof(amount));
        if (reason is { Length: > 255 }) throw new ArgumentException("Reason must not exceed 255 characters", nameof(reason));

        Id = id;
        CashDrawerSessionId = sessionId;
        Type = type;
        Amount = amount;
        Reason = reason;
    }

    /// <summary>
    /// Creates a new <see cref="CashDrawerTransaction"/> instance.
    /// </summary>
    /// <param name="sessionId">The cash drawer session ID.</param>
    /// <param name="type">The type of transaction (IN, OUT, ADJUSTMENT).</param>
    /// <param name="amount">The amount of money involved in the transaction.</param>
    /// <param name="reason">An optional reason for the transaction.</param>
    /// <returns>A new <see cref="CashDrawerTransaction"/> instance.</returns>
    public static CashDrawerTransaction Create(DefaultIdType sessionId, string type, decimal amount, string? reason = null)
        => new(DefaultIdType.NewGuid(), sessionId, type, amount, reason);
}
