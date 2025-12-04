using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Domain constants for Loan entity.
/// </summary>
public static class LoanConstants
{
    /// <summary>Maximum length for loan number. (2^6 = 64)</summary>
    public const int LoanNumberMaxLength = 64;

    /// <summary>Maximum length for status. (2^5 = 32)</summary>
    public const int StatusMaxLength = 32;

    /// <summary>Maximum length for purpose. (2^9 = 512)</summary>
    public const int PurposeMaxLength = 512;

    /// <summary>Maximum length for rejection reason. (2^9 = 512)</summary>
    public const int RejectionReasonMaxLength = 512;

    /// <summary>Maximum length for repayment frequency. (2^5 = 32)</summary>
    public const int RepaymentFrequencyMaxLength = 32;

    /// <summary>Maximum length for notes. (2^12 = 4096)</summary>
    public const int NotesMaxLength = 4096;
}

/// <summary>
/// Represents an individual loan issued to a member in the microfinance system.
/// </summary>
/// <remarks>
/// <para><strong>Use Cases:</strong></para>
/// <list type="bullet">
///   <item><description>Track loan applications through the approval workflow (Pending → Approved → Disbursed)</description></item>
///   <item><description>Record loan disbursements and track repayment progress</description></item>
///   <item><description>Calculate outstanding balances (principal + interest)</description></item>
///   <item><description>Manage loan lifecycle including closure and write-offs</description></item>
///   <item><description>Link to collateral, guarantors, and repayment schedules</description></item>
/// </list>
/// <para><strong>Business Context:</strong></para>
/// <para>
/// A Loan is the core lending entity. Each loan is created from a <see cref="LoanProduct"/> template
/// and assigned to a <see cref="Member"/>. The loan progresses through states:
/// </para>
/// <list type="number">
///   <item><description><strong>PENDING</strong>: Application submitted, awaiting review</description></item>
///   <item><description><strong>APPROVED</strong>: Credit committee has approved the loan</description></item>
///   <item><description><strong>REJECTED</strong>: Application denied (terminal state)</description></item>
///   <item><description><strong>DISBURSED</strong>: Funds released to borrower, repayment begins</description></item>
///   <item><description><strong>CLOSED</strong>: Fully repaid (terminal state)</description></item>
///   <item><description><strong>WRITTEN_OFF</strong>: Declared uncollectible (terminal state)</description></item>
/// </list>
/// <para><strong>Related Entities:</strong></para>
/// <list type="bullet">
///   <item><description><see cref="LoanProduct"/> - Template defining terms</description></item>
///   <item><description><see cref="Member"/> - The borrower</description></item>
///   <item><description><see cref="LoanSchedule"/> - Expected repayment installments</description></item>
///   <item><description><see cref="LoanRepayment"/> - Actual payments received</description></item>
///   <item><description><see cref="LoanGuarantor"/> - Members guaranteeing repayment</description></item>
///   <item><description><see cref="LoanCollateral"/> - Assets pledged as security</description></item>
///   <item><description><see cref="FeeCharge"/> - Fees assessed on the loan</description></item>
/// </list>
/// </remarks>
public class Loan : AuditableEntity, IAggregateRoot
{
    // Loan Statuses
    /// <summary>Loan application submitted, awaiting credit review.</summary>
    public const string StatusPending = "PENDING";
    /// <summary>Loan approved by credit committee, ready for disbursement.</summary>
    public const string StatusApproved = "APPROVED";
    /// <summary>Funds released to borrower, loan is active.</summary>
    public const string StatusDisbursed = "DISBURSED";
    /// <summary>Loan fully repaid and closed.</summary>
    public const string StatusClosed = "CLOSED";
    /// <summary>Loan written off as uncollectible bad debt.</summary>
    public const string StatusWrittenOff = "WRITTEN_OFF";
    /// <summary>Loan application rejected by credit committee.</summary>
    public const string StatusRejected = "REJECTED";

    /// <summary>
    /// Gets the unique loan number/reference.
    /// </summary>
    /// <remarks>
    /// System-generated identifier for tracking and communication (e.g., "LN-2024-001234").
    /// Displayed on all loan documents and communications.
    /// </remarks>
    public string LoanNumber { get; private set; } = default!;

    /// <summary>
    /// Gets the member ID who owns this loan.
    /// </summary>
    /// <remarks>
    /// Foreign key to the borrowing member. A member may have multiple loans (if policy allows).
    /// </remarks>
    public DefaultIdType MemberId { get; private set; }

    /// <summary>
    /// Gets the member navigation property.
    /// </summary>
    public virtual Member? Member { get; private set; }

    /// <summary>
    /// Gets the loan product ID defining the loan terms.
    /// </summary>
    public DefaultIdType LoanProductId { get; private set; }

    /// <summary>
    /// Gets the loan product navigation property.
    /// </summary>
    public virtual LoanProduct? LoanProduct { get; private set; }

    /// <summary>
    /// Gets the principal amount borrowed.
    /// </summary>
    /// <remarks>
    /// The original amount disbursed, excluding interest and fees.
    /// Must be within the product's min/max limits.
    /// </remarks>
    public decimal PrincipalAmount { get; private set; }

    /// <summary>
    /// Gets the interest rate applied to this specific loan.
    /// </summary>
    /// <remarks>
    /// May differ from product rate based on member creditworthiness or special promotions.
    /// Expressed as annual percentage.
    /// </remarks>
    public decimal InterestRate { get; private set; }

    /// <summary>
    /// Gets the loan term in months.
    /// </summary>
    /// <remarks>
    /// Repayment period chosen by the borrower, within product limits.
    /// </remarks>
    public int TermMonths { get; private set; }

    /// <summary>
    /// Gets the repayment frequency.
    /// </summary>
    /// <remarks>
    /// Inherited from product but may be customized: Daily, Weekly, Biweekly, Monthly.
    /// </remarks>
    public string RepaymentFrequency { get; private set; } = default!;

    /// <summary>
    /// Gets the stated purpose of the loan.
    /// </summary>
    /// <remarks>
    /// Borrower's declared use of funds (e.g., "Working capital for grocery store", "Agricultural inputs").
    /// Important for risk assessment and regulatory reporting.
    /// </remarks>
    public string? Purpose { get; private set; }

    /// <summary>
    /// Gets the date the loan application was submitted.
    /// </summary>
    public DateOnly ApplicationDate { get; private set; }

    /// <summary>
    /// Gets the date the loan was approved by the credit committee.
    /// </summary>
    /// <remarks>
    /// Null until approval. Used to calculate time-to-decision metrics.
    /// </remarks>
    public DateOnly? ApprovalDate { get; private set; }

    /// <summary>
    /// Gets the date the loan was disbursed.
    /// </summary>
    /// <remarks>
    /// The date funds were released. Repayment schedule and interest calculation start from this date.
    /// </remarks>
    public DateOnly? DisbursementDate { get; private set; }

    /// <summary>
    /// Gets the expected/scheduled end date of the loan.
    /// </summary>
    /// <remarks>
    /// Calculated from disbursement date + term. Due date for the final installment.
    /// </remarks>
    public DateOnly? ExpectedEndDate { get; private set; }

    /// <summary>
    /// Gets the actual end date when the loan was closed.
    /// </summary>
    /// <remarks>
    /// Set when status changes to Closed or WrittenOff. May differ from ExpectedEndDate
    /// due to early payoff or default.
    /// </remarks>
    public DateOnly? ActualEndDate { get; private set; }

    /// <summary>
    /// Gets the outstanding principal amount.
    /// </summary>
    /// <remarks>
    /// Remaining principal to be repaid. Decreases with each principal payment.
    /// </remarks>
    public decimal OutstandingPrincipal { get; private set; }

    /// <summary>
    /// Gets the outstanding interest amount.
    /// </summary>
    /// <remarks>
    /// Accrued interest not yet paid. May include penalty interest for late payments.
    /// </remarks>
    public decimal OutstandingInterest { get; private set; }

    /// <summary>
    /// Gets the total amount paid so far.
    /// </summary>
    /// <remarks>
    /// Sum of all payments (principal + interest + penalties). Used for payoff calculations.
    /// </remarks>
    public decimal TotalPaid { get; private set; }

    /// <summary>
    /// Gets the current status of the loan.
    /// </summary>
    /// <remarks>
    /// See status constants: PENDING, APPROVED, DISBURSED, CLOSED, WRITTEN_OFF, REJECTED.
    /// Status transitions are controlled by domain methods.
    /// </remarks>
    public string Status { get; private set; } = default!;

    /// <summary>
    /// Gets the rejection reason if the loan was rejected.
    /// </summary>
    /// <remarks>
    /// Required when status is REJECTED. Documents the reason for denial.
    /// </remarks>
    public string? RejectionReason { get; private set; }

    /// <summary>
    /// Gets the collection of repayment transactions for this loan.
    /// </summary>
    public virtual ICollection<LoanRepayment> LoanRepayments { get; private set; } = new List<LoanRepayment>();

    /// <summary>
    /// Gets the collection of scheduled installments for this loan.
    /// </summary>
    public virtual ICollection<LoanSchedule> LoanSchedules { get; private set; } = new List<LoanSchedule>();

    /// <summary>
    /// Gets the collection of guarantors for this loan.
    /// </summary>
    public virtual ICollection<LoanGuarantor> LoanGuarantors { get; private set; } = new List<LoanGuarantor>();

    /// <summary>
    /// Gets the collection of collaterals pledged for this loan.
    /// </summary>
    public virtual ICollection<LoanCollateral> LoanCollaterals { get; private set; } = new List<LoanCollateral>();

    private Loan() { }

    private Loan(
        DefaultIdType id,
        DefaultIdType memberId,
        DefaultIdType loanProductId,
        string loanNumber,
        decimal principalAmount,
        decimal interestRate,
        int termMonths,
        string repaymentFrequency,
        string? purpose)
    {
        Id = id;
        MemberId = memberId;
        LoanProductId = loanProductId;
        LoanNumber = loanNumber.Trim();
        PrincipalAmount = principalAmount;
        InterestRate = interestRate;
        TermMonths = termMonths;
        RepaymentFrequency = repaymentFrequency;
        Purpose = purpose?.Trim();
        ApplicationDate = DateOnly.FromDateTime(DateTime.UtcNow);
        Status = StatusPending;
        OutstandingPrincipal = principalAmount;
        OutstandingInterest = 0;
        TotalPaid = 0;

        QueueDomainEvent(new LoanCreated { Loan = this });
    }

    /// <summary>
    /// Creates a new Loan instance.
    /// </summary>
    public static Loan Create(
        DefaultIdType memberId,
        DefaultIdType loanProductId,
        string loanNumber,
        decimal principalAmount,
        decimal interestRate,
        int termMonths,
        string repaymentFrequency,
        string? purpose = null)
    {
        return new Loan(
            DefaultIdType.NewGuid(),
            memberId,
            loanProductId,
            loanNumber,
            principalAmount,
            interestRate,
            termMonths,
            repaymentFrequency,
            purpose);
    }

    /// <summary>
    /// Approves the loan application.
    /// </summary>
    public Loan Approve(DateOnly approvalDate)
    {
        if (Status != StatusPending)
            throw new InvalidOperationException($"Cannot approve loan in {Status} status.");

        ApprovalDate = approvalDate;
        Status = StatusApproved;

        QueueDomainEvent(new LoanApproved { Loan = this });
        return this;
    }

    /// <summary>
    /// Rejects the loan application.
    /// </summary>
    public Loan Reject(string rejectionReason)
    {
        if (Status != StatusPending)
            throw new InvalidOperationException($"Cannot reject loan in {Status} status.");

        Status = StatusRejected;
        RejectionReason = rejectionReason;

        QueueDomainEvent(new LoanRejected { LoanId = Id, Reason = rejectionReason });
        return this;
    }

    /// <summary>
    /// Disburses the loan to the member.
    /// </summary>
    public Loan Disburse(DateOnly disbursementDate, DateOnly expectedEndDate)
    {
        if (Status != StatusApproved)
            throw new InvalidOperationException($"Cannot disburse loan in {Status} status.");

        DisbursementDate = disbursementDate;
        ExpectedEndDate = expectedEndDate;
        Status = StatusDisbursed;

        QueueDomainEvent(new LoanDisbursed { Loan = this });
        return this;
    }

    /// <summary>
    /// Applies a repayment to the loan.
    /// </summary>
    public Loan ApplyRepayment(decimal principalPaid, decimal interestPaid)
    {
        if (Status != StatusDisbursed)
            throw new InvalidOperationException($"Cannot apply repayment for loan in {Status} status.");

        OutstandingPrincipal -= principalPaid;
        OutstandingInterest -= interestPaid;
        TotalPaid += principalPaid + interestPaid;

        return this;
    }

    /// <summary>
    /// Closes a fully paid loan.
    /// </summary>
    public Loan Close(DateOnly closeDate)
    {
        if (Status != StatusDisbursed)
            throw new InvalidOperationException($"Cannot close loan in {Status} status.");

        if (OutstandingPrincipal > 0 || OutstandingInterest > 0)
            throw new InvalidOperationException("Cannot close loan with outstanding balance.");

        Status = StatusClosed;
        ActualEndDate = closeDate;

        QueueDomainEvent(new LoanPaidOff { LoanId = Id });
        return this;
    }

    /// <summary>
    /// Writes off a non-performing loan.
    /// </summary>
    public Loan WriteOff(string reason)
    {
        if (Status != StatusDisbursed)
            throw new InvalidOperationException($"Cannot write off loan in {Status} status.");

        Status = StatusWrittenOff;
        ActualEndDate = DateOnly.FromDateTime(DateTime.UtcNow);

        QueueDomainEvent(new LoanDefaulted { LoanId = Id, Reason = reason });
        return this;
    }
}

