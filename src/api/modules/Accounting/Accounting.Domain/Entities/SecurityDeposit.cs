using Accounting.Domain.Events.SecurityDeposit;

namespace Accounting.Domain;

/// <summary>
/// Represents a member's security deposit held by the utility with refund lifecycle management and interest tracking.
/// </summary>
/// <remarks>
/// Use cases:
/// - Collect security deposits from new customers to secure payment for utility services.
/// - Track customer deposits as liability on utility books until refunded.
/// - Manage deposit refund eligibility based on payment history and credit criteria.
/// - Support interest calculation on deposits where required by regulation.
/// - Handle deposit transfers when customers move to new service locations.
/// - Process partial deposit refunds and adjustments based on policy changes.
/// - Track deposit forfeitures for unpaid final bills or service damages.
/// - Support estate processing and deposit inheritance transfers.
/// 
/// Default values:
/// - MemberId: required reference to member who paid the deposit
/// - DepositAmount: required positive amount (example: 150.00 for residential deposit)
/// - DepositDate: required date when deposit was received (example: 2025-09-01)
/// - IsRefunded: false (deposits start as unreturned)
/// - RefundDate: null (set when deposit is refunded)
/// - RefundAmount: null (may differ from deposit if interest or adjustments apply)
/// - RefundReason: null (reason for refund: "Good Payment History", "Account Closure", etc.)
/// - InterestRate: null (annual interest rate if applicable by regulation)
/// - AccruedInterest: 0.00 (calculated interest on deposit)
/// 
/// Business rules:
/// - DepositAmount must be positive
/// - Cannot refund more than deposit amount plus accrued interest
/// - DepositDate cannot be in the future
/// - RefundDate must be after DepositDate
/// - Interest calculations follow regulatory requirements
/// - Cannot delete deposits with transaction history
/// - Refund triggers liability reduction in general ledger
/// - Deposit requirements based on customer credit score and payment history
/// </remarks>
/// <seealso cref="Accounting.Domain.Events.SecurityDeposit.SecurityDepositCreated"/>
/// <seealso cref="Accounting.Domain.Events.SecurityDeposit.SecurityDepositRefunded"/>
/// <seealso cref="Accounting.Domain.Events.SecurityDeposit.SecurityDepositInterestAccrued"/>
/// <seealso cref="Accounting.Domain.Events.SecurityDeposit.SecurityDepositTransferred"/>
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

        QueueDomainEvent(new SecurityDepositCreated(Id, memberId, depositAmount, depositDate, null));
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

        QueueDomainEvent(new SecurityDepositRefunded(Id, MemberId, DepositAmount, DepositAmount, refundedDate, refundReference));
        return this;
    }
}
