using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Represents a loan write-off record when a loan is deemed uncollectible.
/// Tracks the write-off process, approvals, and any subsequent recovery.
/// </summary>
/// <remarks>
/// Use cases:
/// - Write off bad debts after all collection efforts exhausted.
/// - Route write-off requests through multi-level approval workflow.
/// - Track post-write-off recoveries for loss recoupment.
/// - Maintain audit trail for regulatory and tax purposes.
/// - Report on write-off trends by product, branch, or officer.
/// 
/// Default values and constraints:
/// - WriteOffNumber: Unique write-off identifier.
/// - WriteOffType: Full, Partial, or Technical.
/// - Status: Pending, Approved, Rejected, Applied.
/// - PrincipalAmount: Principal balance written off.
/// - InterestAmount: Interest balance written off.
/// - FeesAmount: Fees balance written off.
/// - Reason: Documented justification for write-off.
/// 
/// Business rules:
/// - Critical portfolio management and accounting function.
/// - Provisioning: Write-offs often follow full loan provisioning.
/// - Regulatory Compliance: Must follow central bank NPL guidelines.
/// - Tax Implications: May be tax deductible with proper documentation.
/// - Write-off doesn't legally extinguish the debt.
/// - Full: Entire balance; Partial: After settlement; Technical: Small balances.
/// </remarks>
/// <seealso cref="Loan"/>
/// <seealso cref="CollectionCase"/>
/// <seealso cref="ApprovalRequest"/>
/// <example>
/// <para><strong>Example: Requesting a loan write-off</strong></para>
/// <code>
/// POST /api/microfinance/loan-write-offs
/// {
///   "loanId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
///   "writeOffType": "Full",
///   "principalAmount": 450000,
///   "interestAmount": 67500,
///   "feesAmount": 15000,
///   "reason": "Borrower deceased, no estate, guarantors unable to pay after 12 months of collection"
/// }
/// </code>
/// </example>
public sealed class LoanWriteOff : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Maximum length constants for string properties.
    /// </summary>
    public static class MaxLengths
    {
        public const int WriteOffNumber = 32;
        public const int WriteOffType = 64;
        public const int Reason = 512;
        public const int ApprovedBy = 128;
        public const int Notes = 4096;
    }

    /// <summary>
    /// Write-off type classification.
    /// </summary>
    public const string TypeFull = "Full";
    public const string TypePartial = "Partial";
    public const string TypeTechnical = "Technical";

    /// <summary>
    /// Status values.
    /// </summary>
    public const string StatusDraft = "Draft";
    public const string StatusPendingApproval = "PendingApproval";
    public const string StatusApproved = "Approved";
    public const string StatusRejected = "Rejected";
    public const string StatusProcessed = "Processed";
    public const string StatusRecovered = "Recovered";
    public const string StatusCancelled = "Cancelled";

    /// <summary>
    /// Reference to the loan being written off.
    /// </summary>
    public Guid LoanId { get; private set; }

    /// <summary>
    /// Unique write-off reference number.
    /// </summary>
    public string WriteOffNumber { get; private set; } = string.Empty;

    /// <summary>
    /// Type of write-off.
    /// </summary>
    public string WriteOffType { get; private set; } = TypeFull;

    /// <summary>
    /// Reason for write-off.
    /// </summary>
    public string Reason { get; private set; } = string.Empty;

    /// <summary>
    /// Date of write-off request.
    /// </summary>
    public DateOnly RequestDate { get; private set; }

    /// <summary>
    /// Effective date of write-off.
    /// </summary>
    public DateOnly? WriteOffDate { get; private set; }

    /// <summary>
    /// Outstanding principal being written off.
    /// </summary>
    public decimal PrincipalWriteOff { get; private set; }

    /// <summary>
    /// Outstanding interest being written off.
    /// </summary>
    public decimal InterestWriteOff { get; private set; }

    /// <summary>
    /// Penalties being written off.
    /// </summary>
    public decimal PenaltiesWriteOff { get; private set; }

    /// <summary>
    /// Fees being written off.
    /// </summary>
    public decimal FeesWriteOff { get; private set; }

    /// <summary>
    /// Total amount being written off.
    /// </summary>
    public decimal TotalWriteOff { get; private set; }

    /// <summary>
    /// Amount recovered after write-off (if any).
    /// </summary>
    public decimal RecoveredAmount { get; private set; }

    /// <summary>
    /// Days past due at time of write-off.
    /// </summary>
    public int DaysPastDue { get; private set; }

    /// <summary>
    /// Collection attempts made before write-off.
    /// </summary>
    public int CollectionAttempts { get; private set; }

    /// <summary>
    /// Current status.
    /// </summary>
    public string Status { get; private set; } = StatusDraft;

    /// <summary>
    /// User who approved the write-off.
    /// </summary>
    public Guid? ApprovedByUserId { get; private set; }

    /// <summary>
    /// Name of approver.
    /// </summary>
    public string? ApprovedBy { get; private set; }

    /// <summary>
    /// Date of approval.
    /// </summary>
    public DateTime? ApprovedAt { get; private set; }

    /// <summary>
    /// Additional notes.
    /// </summary>
    public string? Notes { get; private set; }

    // Navigation properties
    public Loan Loan { get; private set; } = null!;

    private LoanWriteOff() { }

    /// <summary>
    /// Creates a new loan write-off request.
    /// </summary>
    public static LoanWriteOff Create(
        Guid loanId,
        string writeOffNumber,
        string writeOffType,
        string reason,
        decimal principalWriteOff,
        decimal interestWriteOff,
        decimal penaltiesWriteOff,
        decimal feesWriteOff,
        int daysPastDue,
        int collectionAttempts = 0)
    {
        var writeOff = new LoanWriteOff
        {
            LoanId = loanId,
            WriteOffNumber = writeOffNumber,
            WriteOffType = writeOffType,
            Reason = reason,
            PrincipalWriteOff = principalWriteOff,
            InterestWriteOff = interestWriteOff,
            PenaltiesWriteOff = penaltiesWriteOff,
            FeesWriteOff = feesWriteOff,
            TotalWriteOff = principalWriteOff + interestWriteOff + penaltiesWriteOff + feesWriteOff,
            DaysPastDue = daysPastDue,
            CollectionAttempts = collectionAttempts,
            RequestDate = DateOnly.FromDateTime(DateTime.UtcNow),
            RecoveredAmount = 0,
            Status = StatusDraft
        };

        writeOff.QueueDomainEvent(new LoanWriteOffCreated(writeOff));
        return writeOff;
    }

    /// <summary>
    /// Submits for approval.
    /// </summary>
    public void SubmitForApproval()
    {
        if (Status != StatusDraft)
            throw new InvalidOperationException("Only draft write-offs can be submitted.");

        Status = StatusPendingApproval;
        QueueDomainEvent(new LoanWriteOffSubmitted(Id));
    }

    /// <summary>
    /// Approves the write-off.
    /// </summary>
    public void Approve(Guid userId, string approverName, DateOnly writeOffDate)
    {
        if (Status != StatusPendingApproval)
            throw new InvalidOperationException("Only pending write-offs can be approved.");

        ApprovedByUserId = userId;
        ApprovedBy = approverName;
        ApprovedAt = DateTime.UtcNow;
        WriteOffDate = writeOffDate;
        Status = StatusApproved;

        QueueDomainEvent(new LoanWriteOffApproved(Id, userId, TotalWriteOff));
    }

    /// <summary>
    /// Rejects the write-off.
    /// </summary>
    public void Reject(Guid userId, string reason)
    {
        if (Status != StatusPendingApproval)
            throw new InvalidOperationException("Only pending write-offs can be rejected.");

        ApprovedByUserId = userId;
        ApprovedAt = DateTime.UtcNow;
        Notes = reason;
        Status = StatusRejected;

        QueueDomainEvent(new LoanWriteOffRejected(Id, userId, reason));
    }

    /// <summary>
    /// Processes the write-off (applies to loan and accounting).
    /// </summary>
    public void Process()
    {
        if (Status != StatusApproved)
            throw new InvalidOperationException("Only approved write-offs can be processed.");

        Status = StatusProcessed;
        QueueDomainEvent(new LoanWriteOffProcessed(Id, TotalWriteOff));
    }

    /// <summary>
    /// Records recovery of written-off amount.
    /// </summary>
    public void RecordRecovery(decimal amount)
    {
        if (Status != StatusProcessed && Status != StatusRecovered)
            throw new InvalidOperationException("Recovery can only be recorded for processed write-offs.");

        RecoveredAmount += amount;
        Status = StatusRecovered;

        QueueDomainEvent(new LoanWriteOffRecovery(Id, amount, RecoveredAmount));
    }
}
