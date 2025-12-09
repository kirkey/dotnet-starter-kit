using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Represents a loan disbursement tranche for staged/phased loan disbursement.
/// Used for construction loans, project financing, asset financing, or milestone-based releases.
/// </summary>
/// <remarks>
/// <para><strong>Use Cases:</strong></para>
/// <list type="bullet">
///   <item><description>Disburse loans in phases tied to project milestones</description></item>
///   <item><description>Release funds directly to vendors for asset purchases</description></item>
///   <item><description>Track disbursement schedules and conditions</description></item>
///   <item><description>Support multiple disbursement methods per tranche</description></item>
///   <item><description>Manage approval requirements for each disbursement</description></item>
/// </list>
/// <para><strong>Business Context:</strong></para>
/// <para>
/// Tranched disbursement protects the MFI by releasing funds incrementally:
/// </para>
/// <list type="bullet">
///   <item><description><strong>Risk Reduction</strong>: Funds released only when conditions met</description></item>
///   <item><description><strong>Purpose Verification</strong>: Ensures funds used for intended purpose</description></item>
///   <item><description><strong>Project Monitoring</strong>: Tracks progress before next release</description></item>
///   <item><description><strong>Vendor Payments</strong>: Pay suppliers directly to prevent diversion</description></item>
/// </list>
/// <para><strong>Disbursement Methods:</strong></para>
/// <list type="bullet">
///   <item><description><strong>Cash</strong>: Physical cash to borrower</description></item>
///   <item><description><strong>BankTransfer</strong>: Wire to borrower's bank account</description></item>
///   <item><description><strong>MobileMoney</strong>: To borrower's mobile wallet</description></item>
///   <item><description><strong>DirectToVendor</strong>: Payment to supplier/contractor</description></item>
///   <item><description><strong>Cheque</strong>: Bank cheque issued</description></item>
/// </list>
/// <para><strong>Related Entities:</strong></para>
/// <list type="bullet">
///   <item><description><see cref="Loan"/> - Parent loan being disbursed</description></item>
///   <item><description><see cref="ApprovalRequest"/> - Tranche approval workflow</description></item>
///   <item><description><see cref="TellerSession"/> - Cash disbursement session</description></item>
/// </list>
/// </remarks>
/// <example>
/// <para><strong>Example: Disbursing a tranche</strong></para>
/// <code>
/// POST /api/microfinance/loan-disbursement-tranches/{id}/disburse
/// {
///   "disbursementMethod": "DirectToVendor",
///   "bankName": "Bank of Kigali",
///   "bankAccountNumber": "4000123456789",
///   "amount": 500000,
///   "notes": "Tranche 2: 50% for building materials delivery"
/// }
/// </code>
/// </example>
public sealed class LoanDisbursementTranche : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Maximum length constants for string properties.
    /// </summary>
    public static class MaxLengths
    {
        public const int TrancheNumber = 32;
        public const int DisbursementMethod = 64;
        public const int BankAccountNumber = 64;
        public const int BankName = 128;
        public const int ReferenceNumber = 64;
        public const int Notes = 4096;
    }

    /// <summary>
    /// Disbursement method.
    /// </summary>
    public const string MethodCash = "Cash";
    public const string MethodBankTransfer = "BankTransfer";
    public const string MethodCheque = "Cheque";
    public const string MethodMobileMoney = "MobileMoney";
    public const string MethodDirectToVendor = "DirectToVendor";

    /// <summary>
    /// Status values.
    /// </summary>
    public const string StatusScheduled = "Scheduled";
    public const string StatusPendingApproval = "PendingApproval";
    public const string StatusApproved = "Approved";
    public const string StatusDisbursed = "Disbursed";
    public const string StatusCancelled = "Cancelled";
    public const string StatusOnHold = "OnHold";

    /// <summary>
    /// Reference to the loan.
    /// </summary>
    public Guid LoanId { get; private set; }

    /// <summary>
    /// Tranche sequence number.
    /// </summary>
    public int TrancheSequence { get; private set; }

    /// <summary>
    /// Unique tranche reference number.
    /// </summary>
    public string TrancheNumber { get; private set; } = string.Empty;

    /// <summary>
    /// Scheduled disbursement date.
    /// </summary>
    public DateOnly ScheduledDate { get; private set; }

    /// <summary>
    /// Actual disbursement date.
    /// </summary>
    public DateOnly? DisbursedDate { get; private set; }

    /// <summary>
    /// Amount to be disbursed.
    /// </summary>
    public decimal Amount { get; private set; }

    /// <summary>
    /// Any deductions from the tranche.
    /// </summary>
    public decimal Deductions { get; private set; }

    /// <summary>
    /// Net amount after deductions.
    /// </summary>
    public decimal NetAmount { get; private set; }

    /// <summary>
    /// Disbursement method.
    /// </summary>
    public string DisbursementMethod { get; private set; } = MethodBankTransfer;

    /// <summary>
    /// Bank account number for transfer.
    /// </summary>
    public string? BankAccountNumber { get; private set; }

    /// <summary>
    /// Bank name.
    /// </summary>
    public string? BankName { get; private set; }

    /// <summary>
    /// Payment reference number.
    /// </summary>
    public string? ReferenceNumber { get; private set; }

    /// <summary>
    /// Milestone or condition for this tranche.
    /// </summary>
    public string? Milestone { get; private set; }

    /// <summary>
    /// Whether milestone is verified.
    /// </summary>
    public bool MilestoneVerified { get; private set; }

    /// <summary>
    /// Current status.
    /// </summary>
    public string Status { get; private set; } = StatusScheduled;

    /// <summary>
    /// User who approved.
    /// </summary>
    public Guid? ApprovedByUserId { get; private set; }

    /// <summary>
    /// Date of approval.
    /// </summary>
    public DateTime? ApprovedAt { get; private set; }

    /// <summary>
    /// User who disbursed.
    /// </summary>
    public Guid? DisbursedByUserId { get; private set; }

    /// <summary>
    /// Additional notes.
    /// </summary>
    public string? Notes { get; private set; }

    // Navigation properties
    public Loan Loan { get; private set; } = null!;

    private LoanDisbursementTranche() { }

    /// <summary>
    /// Creates a new disbursement tranche.
    /// </summary>
    public static LoanDisbursementTranche Create(
        Guid loanId,
        int trancheSequence,
        string trancheNumber,
        DateOnly scheduledDate,
        decimal amount,
        string disbursementMethod,
        string? milestone = null,
        string? bankAccountNumber = null,
        string? bankName = null,
        decimal deductions = 0)
    {
        var tranche = new LoanDisbursementTranche
        {
            LoanId = loanId,
            TrancheSequence = trancheSequence,
            TrancheNumber = trancheNumber,
            ScheduledDate = scheduledDate,
            Amount = amount,
            Deductions = deductions,
            NetAmount = amount - deductions,
            DisbursementMethod = disbursementMethod,
            Milestone = milestone,
            BankAccountNumber = bankAccountNumber,
            BankName = bankName,
            MilestoneVerified = false,
            Status = StatusScheduled
        };

        tranche.QueueDomainEvent(new LoanDisbursementTrancheCreated(tranche));
        return tranche;
    }

    /// <summary>
    /// Verifies the milestone for this tranche.
    /// </summary>
    public void VerifyMilestone()
    {
        MilestoneVerified = true;
        QueueDomainEvent(new LoanDisbursementTrancheMilestoneVerified(Id));
    }

    /// <summary>
    /// Submits for approval.
    /// </summary>
    public void SubmitForApproval()
    {
        if (Status != StatusScheduled)
            throw new InvalidOperationException("Only scheduled tranches can be submitted.");

        if (!string.IsNullOrEmpty(Milestone) && !MilestoneVerified)
            throw new InvalidOperationException("Milestone must be verified before submission.");

        Status = StatusPendingApproval;
        QueueDomainEvent(new LoanDisbursementTrancheSubmitted(Id));
    }

    /// <summary>
    /// Approves the tranche.
    /// </summary>
    public void Approve(Guid userId)
    {
        if (Status != StatusPendingApproval)
            throw new InvalidOperationException("Only pending tranches can be approved.");

        ApprovedByUserId = userId;
        ApprovedAt = DateTime.UtcNow;
        Status = StatusApproved;

        QueueDomainEvent(new LoanDisbursementTrancheApproved(Id, userId));
    }

    /// <summary>
    /// Disburses the tranche.
    /// </summary>
    public void Disburse(Guid userId, string referenceNumber, DateOnly? disbursedDate = null)
    {
        if (Status != StatusApproved)
            throw new InvalidOperationException("Only approved tranches can be disbursed.");

        DisbursedByUserId = userId;
        DisbursedDate = disbursedDate ?? DateOnly.FromDateTime(DateTime.UtcNow);
        ReferenceNumber = referenceNumber;
        Status = StatusDisbursed;

        QueueDomainEvent(new LoanDisbursementTrancheDisbursed(Id, NetAmount, DisbursedDate.Value));
    }

    /// <summary>
    /// Puts the tranche on hold.
    /// </summary>
    public void PutOnHold(string reason)
    {
        if (Status == StatusDisbursed || Status == StatusCancelled)
            throw new InvalidOperationException("Cannot put completed/cancelled tranches on hold.");

        Notes = reason;
        Status = StatusOnHold;
        QueueDomainEvent(new LoanDisbursementTrancheOnHold(Id, reason));
    }

    /// <summary>
    /// Cancels the tranche.
    /// </summary>
    public void Cancel(string reason)
    {
        if (Status == StatusDisbursed)
            throw new InvalidOperationException("Cannot cancel disbursed tranches.");

        Notes = reason;
        Status = StatusCancelled;
        QueueDomainEvent(new LoanDisbursementTrancheCancelled(Id, reason));
    }
}
