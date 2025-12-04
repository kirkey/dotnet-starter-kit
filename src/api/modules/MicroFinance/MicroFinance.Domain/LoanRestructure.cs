using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Represents a loan restructuring or modification agreement.
/// Used when a borrower faces difficulty and needs loan terms adjusted.
/// </summary>
public sealed class LoanRestructure : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Maximum length constants for string properties.
    /// </summary>
    public static class MaxLengths
    {
        public const int RestructureNumber = 32;
        public const int RestructureType = 64;
        public const int Reason = 512;
        public const int ApprovedBy = 128;
        public const int Notes = 4096;
    }

    /// <summary>
    /// Restructure type classification.
    /// </summary>
    public const string TypeTermExtension = "TermExtension";
    public const string TypeRateReduction = "RateReduction";
    public const string TypePrincipalReduction = "PrincipalReduction";
    public const string TypePaymentHoliday = "PaymentHoliday";
    public const string TypeConsolidation = "Consolidation";
    public const string TypeRefinance = "Refinance";

    /// <summary>
    /// Status values.
    /// </summary>
    public const string StatusDraft = "Draft";
    public const string StatusPendingApproval = "PendingApproval";
    public const string StatusApproved = "Approved";
    public const string StatusRejected = "Rejected";
    public const string StatusActive = "Active";
    public const string StatusCompleted = "Completed";
    public const string StatusCancelled = "Cancelled";

    /// <summary>
    /// Reference to the original loan.
    /// </summary>
    public Guid LoanId { get; private set; }

    /// <summary>
    /// Unique restructure number.
    /// </summary>
    public string RestructureNumber { get; private set; } = string.Empty;

    /// <summary>
    /// Type of restructuring.
    /// </summary>
    public string RestructureType { get; private set; } = TypeTermExtension;

    /// <summary>
    /// Reason for restructuring.
    /// </summary>
    public string? Reason { get; private set; }

    /// <summary>
    /// Date of restructure request.
    /// </summary>
    public DateOnly RequestDate { get; private set; }

    /// <summary>
    /// Effective date of restructure.
    /// </summary>
    public DateOnly? EffectiveDate { get; private set; }

    // Original loan terms
    /// <summary>
    /// Original outstanding principal.
    /// </summary>
    public decimal OriginalPrincipal { get; private set; }

    /// <summary>
    /// Original interest rate.
    /// </summary>
    public decimal OriginalInterestRate { get; private set; }

    /// <summary>
    /// Original remaining term in months.
    /// </summary>
    public int OriginalRemainingTerm { get; private set; }

    /// <summary>
    /// Original installment amount.
    /// </summary>
    public decimal OriginalInstallmentAmount { get; private set; }

    // New loan terms
    /// <summary>
    /// New principal after restructure.
    /// </summary>
    public decimal NewPrincipal { get; private set; }

    /// <summary>
    /// New interest rate.
    /// </summary>
    public decimal NewInterestRate { get; private set; }

    /// <summary>
    /// New term in months.
    /// </summary>
    public int NewTerm { get; private set; }

    /// <summary>
    /// New installment amount.
    /// </summary>
    public decimal NewInstallmentAmount { get; private set; }

    /// <summary>
    /// Grace period for first payment (months).
    /// </summary>
    public int GracePeriodMonths { get; private set; }

    /// <summary>
    /// Amount waived (if any).
    /// </summary>
    public decimal WaivedAmount { get; private set; }

    /// <summary>
    /// Restructuring fee charged.
    /// </summary>
    public decimal RestructureFee { get; private set; }

    /// <summary>
    /// Current status.
    /// </summary>
    public string Status { get; private set; } = StatusDraft;

    /// <summary>
    /// User who approved the restructure.
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

    private LoanRestructure() { }

    /// <summary>
    /// Creates a new loan restructure request.
    /// </summary>
    public static LoanRestructure Create(
        Guid loanId,
        string restructureNumber,
        string restructureType,
        decimal originalPrincipal,
        decimal originalInterestRate,
        int originalRemainingTerm,
        decimal originalInstallmentAmount,
        decimal newPrincipal,
        decimal newInterestRate,
        int newTerm,
        decimal newInstallmentAmount,
        string? reason = null,
        int gracePeriodMonths = 0,
        decimal waivedAmount = 0,
        decimal restructureFee = 0)
    {
        var restructure = new LoanRestructure
        {
            LoanId = loanId,
            RestructureNumber = restructureNumber,
            RestructureType = restructureType,
            OriginalPrincipal = originalPrincipal,
            OriginalInterestRate = originalInterestRate,
            OriginalRemainingTerm = originalRemainingTerm,
            OriginalInstallmentAmount = originalInstallmentAmount,
            NewPrincipal = newPrincipal,
            NewInterestRate = newInterestRate,
            NewTerm = newTerm,
            NewInstallmentAmount = newInstallmentAmount,
            Reason = reason,
            GracePeriodMonths = gracePeriodMonths,
            WaivedAmount = waivedAmount,
            RestructureFee = restructureFee,
            RequestDate = DateOnly.FromDateTime(DateTime.UtcNow),
            Status = StatusDraft
        };

        restructure.QueueDomainEvent(new LoanRestructureCreated(restructure));
        return restructure;
    }

    /// <summary>
    /// Submits for approval.
    /// </summary>
    public void SubmitForApproval()
    {
        if (Status != StatusDraft)
            throw new InvalidOperationException("Only draft restructures can be submitted.");

        Status = StatusPendingApproval;
        QueueDomainEvent(new LoanRestructureSubmitted(Id));
    }

    /// <summary>
    /// Approves the restructure.
    /// </summary>
    public void Approve(Guid userId, string approverName, DateOnly effectiveDate)
    {
        if (Status != StatusPendingApproval)
            throw new InvalidOperationException("Only pending restructures can be approved.");

        ApprovedByUserId = userId;
        ApprovedBy = approverName;
        ApprovedAt = DateTime.UtcNow;
        EffectiveDate = effectiveDate;
        Status = StatusApproved;

        QueueDomainEvent(new LoanRestructureApproved(Id, userId, effectiveDate));
    }

    /// <summary>
    /// Rejects the restructure.
    /// </summary>
    public void Reject(Guid userId, string reason)
    {
        if (Status != StatusPendingApproval)
            throw new InvalidOperationException("Only pending restructures can be rejected.");

        ApprovedByUserId = userId;
        ApprovedAt = DateTime.UtcNow;
        Notes = reason;
        Status = StatusRejected;

        QueueDomainEvent(new LoanRestructureRejected(Id, userId, reason));
    }

    /// <summary>
    /// Activates the restructure (applies to loan).
    /// </summary>
    public void Activate()
    {
        if (Status != StatusApproved)
            throw new InvalidOperationException("Only approved restructures can be activated.");

        Status = StatusActive;
        QueueDomainEvent(new LoanRestructureActivated(Id));
    }

    /// <summary>
    /// Marks restructure as completed.
    /// </summary>
    public void Complete()
    {
        if (Status != StatusActive)
            throw new InvalidOperationException("Only active restructures can be completed.");

        Status = StatusCompleted;
        QueueDomainEvent(new LoanRestructureCompleted(Id));
    }
}
