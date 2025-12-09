using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Represents a loan guarantor who guarantees loan repayment.
/// </summary>
/// <remarks>
/// Use cases:
/// - Record third-party guarantees for loan security.
/// - Track the guaranteed amount and relationship to borrower.
/// - Manage guarantor approval workflow.
/// - Release guarantors when loan is repaid.
/// - Pursue guarantors for collection on defaulted loans.
/// 
/// Default values and constraints:
/// - LoanId: required, must reference an active loan
/// - GuarantorMemberId: required, must reference an active member in good standing
/// - GuaranteedAmount: required, amount the guarantor is liable for
/// - Relationship: required, relationship to borrower, max 128 characters
/// - Status: "Pending" by default (Pending, Approved, Rejected, Released)
/// - ApprovalDate: date guarantee was approved
/// - ReleaseDate: date guarantee obligation ended
/// 
/// Business rules:
/// - Guarantor must be a member in good standing.
/// - Guarantor cannot have excessive exposure (total guarantees capped).
/// - Guarantor's savings may be frozen for guaranteed amount.
/// - Cannot guarantee own loan.
/// - Release requires loan full repayment.
/// - Collection actions may be taken against guarantor on default.
/// </remarks>
/// <seealso cref="Loan"/>
/// <seealso cref="Member"/>
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
