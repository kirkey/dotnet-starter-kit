

namespace Accounting.Domain;

/// <summary>
/// Represents a member’s security deposit held by the utility, with refund lifecycle and metadata.
/// </summary>
/// <remarks>
/// Tracks deposit amount and date, and whether/when it was refunded. Defaults: <see cref="IsRefunded"/> is false; strings trimmed.
/// </remarks>
public class SecurityDeposit : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Member identifier who owns the deposit.
    /// </summary>
    public DefaultIdType MemberId { get; private set; }

    /// <summary>
    /// Amount deposited; must be positive.
    /// </summary>
    public decimal DepositAmount { get; private set; }

    /// <summary>
    /// Date the deposit was received.
    /// </summary>
    public DateTime DepositDate { get; private set; }

    /// <summary>
    /// Whether the deposit has been refunded.
    /// </summary>
    public bool IsRefunded { get; private set; }

    /// <summary>
    /// Date of refund, when applicable.
    /// </summary>
    public DateTime? RefundedDate { get; private set; }

    /// <summary>
    /// External reference for the refund (e.g., check number), when available.
    /// </summary>
    public string? RefundReference { get; private set; }

    private SecurityDeposit()
    {
        MemberId = DefaultIdType.Empty;
        DepositAmount = 0m;
        DepositDate = DateTime.MinValue;
        IsRefunded = false;
    }

    private SecurityDeposit(DefaultIdType memberId, decimal depositAmount, DateTime depositDate, string? description = null, string? notes = null)
    {
        MemberId = memberId;
        DepositAmount = depositAmount;
        DepositDate = depositDate;
        IsRefunded = false;
        Description = description?.Trim();
        Notes = notes?.Trim();
    }

    /// <summary>
    /// Create a security deposit for a member; amount must be positive.
    /// </summary>
    public static SecurityDeposit Create(DefaultIdType memberId, decimal depositAmount, DateTime depositDate, string? description = null, string? notes = null)
    {
        if (depositAmount <= 0) throw new ArgumentException("Deposit amount must be positive.");
        return new SecurityDeposit(memberId, depositAmount, depositDate, description, notes);
    }

    /// <summary>
    /// Mark the deposit as refunded and set refund metadata. Cannot be called twice.
    /// </summary>
    public SecurityDeposit Refund(DateTime refundedDate, string? refundReference = null)
    {
        if (IsRefunded) throw new InvalidOperationException("Deposit already refunded.");
        IsRefunded = true;
        RefundedDate = refundedDate;
        RefundReference = refundReference?.Trim();
        return this;
    }
}