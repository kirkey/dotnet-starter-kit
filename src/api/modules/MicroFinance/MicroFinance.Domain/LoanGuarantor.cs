using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Represents a loan guarantor who guarantees loan repayment.
/// </summary>
/// <remarks>
/// <para><strong>Use Cases:</strong></para>
/// <list type="bullet">
///   <item><description>Record third-party guarantees for loan security</description></item>
///   <item><description>Track the guaranteed amount and relationship to borrower</description></item>
///   <item><description>Manage guarantor approval workflow</description></item>
///   <item><description>Release guarantors when loan is repaid</description></item>
///   <item><description>Pursue guarantors for collection on defaulted loans</description></item>
/// </list>
/// <para><strong>Business Context:</strong></para>
/// <para>
/// Guarantors provide personal surety for loan repayment, common in microfinance where borrowers
/// lack traditional collateral. Key principles:
/// </para>
/// <list type="bullet">
///   <item><description><strong>Personal Guarantee</strong>: Guarantor agrees to repay if borrower defaults</description></item>
///   <item><description><strong>Co-Signer</strong>: May require guarantor's savings/shares as additional security</description></item>
///   <item><description><strong>Group Guarantee</strong>: In solidarity groups, all members guarantee each other</description></item>
///   <item><description><strong>Exposure Limits</strong>: Guarantor's total exposure may be capped</description></item>
/// </list>
/// <para>
/// Guarantors are typically other members in good standing. The MFI may freeze a portion of the
/// guarantor's savings equal to the guaranteed amount.
/// </para>
/// <para><strong>Status Progression:</strong></para>
/// <list type="bullet">
///   <item><description><strong>Pending</strong>: Guarantee requested, awaiting approval</description></item>
///   <item><description><strong>Approved</strong>: Guarantee accepted, active obligation</description></item>
///   <item><description><strong>Rejected</strong>: Guarantee declined (insufficient standing, over-exposure)</description></item>
///   <item><description><strong>Released</strong>: Loan repaid, guarantee obligation ends</description></item>
/// </list>
/// <para><strong>Related Entities:</strong></para>
/// <list type="bullet">
///   <item><description><see cref="Loan"/> - The loan being guaranteed</description></item>
///   <item><description><see cref="Member"/> - The member acting as guarantor</description></item>
/// </list>
/// </remarks>
public class LoanGuarantor : AuditableEntity, IAggregateRoot
{
    // Domain Constants
    /// <summary>Maximum length for relationship field. (2^7 = 128)</summary>
    public const int RelationshipMaxLength = 128;

    /// <summary>Maximum length for status field. (2^5 = 32)</summary>
    public const int StatusMaxLength = 32;

    /// <summary>Maximum length for notes field. (2^11 = 2048)</summary>
    public const int NotesMaxLength = 2048;

    // Guarantor Statuses
    public const string StatusPending = "Pending";
    public const string StatusApproved = "Approved";
    public const string StatusRejected = "Rejected";
    public const string StatusReleased = "Released";

    /// <summary>Gets the loan ID this guarantor is for.</summary>
    public DefaultIdType LoanId { get; private set; }

    /// <summary>Gets the loan navigation property.</summary>
    public virtual Loan Loan { get; private set; } = default!;

    /// <summary>Gets the guarantor member ID.</summary>
    public DefaultIdType GuarantorMemberId { get; private set; }

    /// <summary>Gets the guarantor member navigation property.</summary>
    public virtual Member GuarantorMember { get; private set; } = default!;

    /// <summary>Gets the amount guaranteed.</summary>
    public decimal GuaranteedAmount { get; private set; }

    /// <summary>Gets the relationship to the borrower.</summary>
    public string? Relationship { get; private set; }

    /// <summary>Gets the date the guarantee was created.</summary>
    public DateOnly GuaranteeDate { get; private set; }

    /// <summary>Gets the date the guarantee expires.</summary>
    public DateOnly? ExpiryDate { get; private set; }

    /// <summary>Gets the current status.</summary>
    public string Status { get; private set; } = default!;

    /// <summary>Gets internal notes.</summary>
    public new string? Notes { get; private set; }

    private LoanGuarantor() { }

    private LoanGuarantor(
        DefaultIdType id,
        DefaultIdType loanId,
        DefaultIdType guarantorMemberId,
        decimal guaranteedAmount,
        string? relationship,
        DateOnly guaranteeDate,
        DateOnly? expiryDate,
        string? notes)
    {
        Id = id;
        LoanId = loanId;
        GuarantorMemberId = guarantorMemberId;
        GuaranteedAmount = guaranteedAmount;
        Relationship = relationship?.Trim();
        GuaranteeDate = guaranteeDate;
        ExpiryDate = expiryDate;
        Status = StatusPending;
        Notes = notes?.Trim();

        QueueDomainEvent(new LoanGuarantorCreated { LoanGuarantor = this });
    }

    /// <summary>
    /// Creates a new LoanGuarantor instance.
    /// </summary>
    public static LoanGuarantor Create(
        DefaultIdType loanId,
        DefaultIdType guarantorMemberId,
        decimal guaranteedAmount,
        string? relationship = null,
        DateOnly? guaranteeDate = null,
        DateOnly? expiryDate = null,
        string? notes = null)
    {
        return new LoanGuarantor(
            DefaultIdType.NewGuid(),
            loanId,
            guarantorMemberId,
            guaranteedAmount,
            relationship,
            guaranteeDate ?? DateOnly.FromDateTime(DateTime.UtcNow),
            expiryDate,
            notes);
    }

    /// <summary>
    /// Approves the guarantor.
    /// </summary>
    public LoanGuarantor Approve()
    {
        if (Status != StatusPending)
            throw new InvalidOperationException($"Cannot approve guarantor in {Status} status.");

        Status = StatusApproved;
        QueueDomainEvent(new LoanGuarantorApproved { GuarantorId = Id });
        return this;
    }

    /// <summary>
    /// Rejects the guarantor.
    /// </summary>
    public LoanGuarantor Reject(string? reason = null)
    {
        if (Status != StatusPending)
            throw new InvalidOperationException($"Cannot reject guarantor in {Status} status.");

        Status = StatusRejected;
        if (!string.IsNullOrWhiteSpace(reason))
        {
            Notes = string.IsNullOrWhiteSpace(Notes) ? $"Rejected: {reason}" : $"{Notes}\nRejected: {reason}";
        }

        QueueDomainEvent(new LoanGuarantorRejected { GuarantorId = Id, Reason = reason });
        return this;
    }

    /// <summary>
    /// Releases the guarantor from the guarantee.
    /// </summary>
    public LoanGuarantor Release(string? reason = null)
    {
        if (Status != StatusApproved)
            throw new InvalidOperationException($"Cannot release guarantor in {Status} status.");

        Status = StatusReleased;
        if (!string.IsNullOrWhiteSpace(reason))
        {
            Notes = string.IsNullOrWhiteSpace(Notes) ? $"Released: {reason}" : $"{Notes}\nReleased: {reason}";
        }

        QueueDomainEvent(new LoanGuarantorReleased { GuarantorId = Id });
        return this;
    }

    /// <summary>
    /// Updates the guaranteed amount.
    /// </summary>
    public LoanGuarantor UpdateGuaranteedAmount(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Guaranteed amount must be positive.", nameof(amount));

        if (GuaranteedAmount != amount)
        {
            GuaranteedAmount = amount;
        }
        return this;
    }
}
