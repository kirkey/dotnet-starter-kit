using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Represents an actual fee charged to a member's account.
/// </summary>
/// <remarks>
/// <para><strong>Use Cases:</strong></para>
/// <list type="bullet">
///   <item><description>Record fees assessed against loans, savings, or share accounts</description></item>
///   <item><description>Track fee payment status (pending, partial, paid)</description></item>
///   <item><description>Process fee waivers and reversals with audit trail</description></item>
///   <item><description>Calculate outstanding fee balances for collections</description></item>
///   <item><description>Report on fee income by type and product</description></item>
/// </list>
/// <para><strong>Business Context:</strong></para>
/// <para>
/// FeeCharge represents a specific instance of a fee applied to a member's account. While
/// <see cref="FeeDefinition"/> defines <em>how</em> fees are calculated, FeeCharge records
/// <em>actual</em> charges with specific amounts and payment tracking.
/// </para>
/// <para>
/// Fee charges can be:
/// </para>
/// <list type="bullet">
///   <item><description>System-generated (automatic late fees, monthly maintenance)</description></item>
///   <item><description>Manually added (processing fees, special service charges)</description></item>
///   <item><description>Event-triggered (loan disbursement, account closure)</description></item>
/// </list>
/// <para><strong>Status Progression:</strong></para>
/// <list type="bullet">
///   <item><description><strong>Pending</strong>: Fee assessed, awaiting payment</description></item>
///   <item><description><strong>PartiallyPaid</strong>: Some payment received</description></item>
///   <item><description><strong>Paid</strong>: Fully settled</description></item>
///   <item><description><strong>Waived</strong>: Forgiven by authorized personnel</description></item>
///   <item><description><strong>Reversed</strong>: Cancelled/voided</description></item>
/// </list>
/// <para><strong>Related Entities:</strong></para>
/// <list type="bullet">
///   <item><description><see cref="FeeDefinition"/> - Template defining the fee type and calculation</description></item>
///   <item><description><see cref="Member"/> - Member who owes the fee</description></item>
///   <item><description><see cref="Loan"/> - Loan account if applicable</description></item>
///   <item><description><see cref="SavingsAccount"/> - Savings account if applicable</description></item>
///   <item><description><see cref="ShareAccount"/> - Share account if applicable</description></item>
/// </list>
/// </remarks>
public class FeeCharge : AuditableEntity, IAggregateRoot
{
    // Domain Constants
    /// <summary>Maximum length for reference field. (2^6 = 64)</summary>
    public const int ReferenceMaxLength = 64;

    /// <summary>Maximum length for status field. (2^5 = 32)</summary>
    public const int StatusMaxLength = 32;

    /// <summary>Maximum length for notes field. (2^11 = 2048)</summary>
    public const int NotesMaxLength = 2048;

    // Charge Statuses
    public const string StatusPending = "Pending";
    public const string StatusPartiallyPaid = "PartiallyPaid";
    public const string StatusPaid = "Paid";
    public const string StatusWaived = "Waived";
    public const string StatusReversed = "Reversed";

    /// <summary>Gets the fee definition ID.</summary>
    public DefaultIdType FeeDefinitionId { get; private set; }

    /// <summary>Gets the fee definition navigation property.</summary>
    public virtual FeeDefinition FeeDefinition { get; private set; } = default!;

    /// <summary>Gets the loan ID if applicable.</summary>
    public DefaultIdType? LoanId { get; private set; }

    /// <summary>Gets the loan navigation property.</summary>
    public virtual Loan? Loan { get; private set; }

    /// <summary>Gets the savings account ID if applicable.</summary>
    public DefaultIdType? SavingsAccountId { get; private set; }

    /// <summary>Gets the savings account navigation property.</summary>
    public virtual SavingsAccount? SavingsAccount { get; private set; }

    /// <summary>Gets the share account ID if applicable.</summary>
    public DefaultIdType? ShareAccountId { get; private set; }

    /// <summary>Gets the share account navigation property.</summary>
    public virtual ShareAccount? ShareAccount { get; private set; }

    /// <summary>Gets the member ID.</summary>
    public DefaultIdType MemberId { get; private set; }

    /// <summary>Gets the member navigation property.</summary>
    public virtual Member Member { get; private set; } = default!;

    /// <summary>Gets the unique charge reference.</summary>
    public string Reference { get; private set; } = default!;

    /// <summary>Gets the date the fee was charged.</summary>
    public DateOnly ChargeDate { get; private set; }

    /// <summary>Gets the due date for payment.</summary>
    public DateOnly? DueDate { get; private set; }

    /// <summary>Gets the fee amount charged.</summary>
    public decimal Amount { get; private set; }

    /// <summary>Gets the amount paid.</summary>
    public decimal AmountPaid { get; private set; }

    /// <summary>Gets the outstanding balance.</summary>
    public decimal Outstanding => Amount - AmountPaid;

    /// <summary>Gets the current status.</summary>
    public string Status { get; private set; } = default!;

    /// <summary>Gets the payment completion date.</summary>
    public DateOnly? PaidDate { get; private set; }

    /// <summary>
    /// Gets the collection of payments applied to this fee charge.
    /// </summary>
    public virtual ICollection<FeePayment> Payments { get; private set; } = new List<FeePayment>();

    /// <summary>
    /// Gets the collection of waiver requests for this fee charge.
    /// </summary>
    public virtual ICollection<FeeWaiver> Waivers { get; private set; } = new List<FeeWaiver>();

    private FeeCharge() { }

    private FeeCharge(
        DefaultIdType id,
        DefaultIdType feeDefinitionId,
        DefaultIdType memberId,
        DefaultIdType? loanId,
        DefaultIdType? savingsAccountId,
        DefaultIdType? shareAccountId,
        string reference,
        DateOnly chargeDate,
        DateOnly? dueDate,
        decimal amount,
        string? notes)
    {
        Id = id;
        FeeDefinitionId = feeDefinitionId;
        MemberId = memberId;
        LoanId = loanId;
        SavingsAccountId = savingsAccountId;
        ShareAccountId = shareAccountId;
        Reference = reference.Trim();
        ChargeDate = chargeDate;
        DueDate = dueDate;
        Amount = amount;
        AmountPaid = 0;
        Status = StatusPending;
        Notes = notes?.Trim();

        QueueDomainEvent(new FeeChargeCreated { FeeCharge = this });
    }

    /// <summary>
    /// Creates a new FeeCharge instance.
    /// </summary>
    public static FeeCharge Create(
        DefaultIdType feeDefinitionId,
        DefaultIdType memberId,
        string reference,
        decimal amount,
        DefaultIdType? loanId = null,
        DefaultIdType? savingsAccountId = null,
        DefaultIdType? shareAccountId = null,
        DateOnly? chargeDate = null,
        DateOnly? dueDate = null,
        string? notes = null)
    {
        return new FeeCharge(
            DefaultIdType.NewGuid(),
            feeDefinitionId,
            memberId,
            loanId,
            savingsAccountId,
            shareAccountId,
            reference,
            chargeDate ?? DateOnly.FromDateTime(DateTime.UtcNow),
            dueDate,
            amount,
            notes);
    }

    /// <summary>
    /// Records a payment against this fee charge.
    /// </summary>
    public FeeCharge RecordPayment(decimal amount)
    {
        if (Status == StatusPaid || Status == StatusWaived || Status == StatusReversed)
            throw new InvalidOperationException($"Cannot record payment for fee in {Status} status.");

        if (amount <= 0)
            throw new ArgumentException("Payment amount must be positive.", nameof(amount));

        AmountPaid += amount;

        if (AmountPaid >= Amount)
        {
            Status = StatusPaid;
            PaidDate = DateOnly.FromDateTime(DateTime.UtcNow);
            QueueDomainEvent(new FeeChargePaid { FeeChargeId = Id });
        }
        else
        {
            Status = StatusPartiallyPaid;
        }

        return this;
    }

    /// <summary>
    /// Waives the fee (full or partial).
    /// </summary>
    public FeeCharge Waive(string? reason = null)
    {
        if (Status == StatusPaid || Status == StatusReversed)
            throw new InvalidOperationException($"Cannot waive fee in {Status} status.");

        Status = StatusWaived;

        if (!string.IsNullOrWhiteSpace(reason))
        {
            Notes = string.IsNullOrWhiteSpace(Notes) ? $"Waived: {reason}" : $"{Notes}\nWaived: {reason}";
        }

        QueueDomainEvent(new FeeChargeWaived { FeeChargeId = Id });
        return this;
    }

    /// <summary>
    /// Reverses the fee charge.
    /// </summary>
    public FeeCharge Reverse(string? reason = null)
    {
        if (Status == StatusReversed)
            throw new InvalidOperationException("Fee is already reversed.");

        if (AmountPaid > 0)
            throw new InvalidOperationException("Cannot reverse a fee with payments. Refund the payments first.");

        Status = StatusReversed;

        if (!string.IsNullOrWhiteSpace(reason))
        {
            Notes = string.IsNullOrWhiteSpace(Notes) ? $"Reversed: {reason}" : $"{Notes}\nReversed: {reason}";
        }

        QueueDomainEvent(new FeeChargeReversed { FeeChargeId = Id });
        return this;
    }
}
