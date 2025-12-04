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

    /// <summary>Maximum length for currency. (2^3 = 8)</summary>
    public const int CurrencyMaxLength = 8;

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
/// Represents a loan issued to a member in the microfinance system.
/// </summary>
public class Loan : AuditableEntity, IAggregateRoot
{
    // Loan Statuses
    public const string StatusPending = "PENDING";
    public const string StatusApproved = "APPROVED";
    public const string StatusDisbursed = "DISBURSED";
    public const string StatusClosed = "CLOSED";
    public const string StatusWrittenOff = "WRITTEN_OFF";
    public const string StatusRejected = "REJECTED";

    /// <summary>Gets the unique loan number.</summary>
    public string LoanNumber { get; private set; } = default!;

    /// <summary>Gets the member ID who owns this loan.</summary>
    public DefaultIdType MemberId { get; private set; }

    /// <summary>Gets the member navigation property.</summary>
    public virtual Member? Member { get; private set; }

    /// <summary>Gets the loan product ID.</summary>
    public DefaultIdType LoanProductId { get; private set; }

    /// <summary>Gets the loan product navigation property.</summary>
    public virtual LoanProduct? LoanProduct { get; private set; }

    /// <summary>Gets the principal amount of the loan.</summary>
    public decimal PrincipalAmount { get; private set; }

    /// <summary>Gets the interest rate applied to this loan.</summary>
    public decimal InterestRate { get; private set; }

    /// <summary>Gets the loan term in months.</summary>
    public int TermMonths { get; private set; }

    /// <summary>Gets the repayment frequency.</summary>
    public string RepaymentFrequency { get; private set; } = default!;

    /// <summary>Gets the currency of the loan.</summary>
    public string Currency { get; private set; } = default!;

    /// <summary>Gets the purpose of the loan.</summary>
    public string? Purpose { get; private set; }

    /// <summary>Gets the date the loan application was submitted.</summary>
    public DateOnly ApplicationDate { get; private set; }

    /// <summary>Gets the date the loan was approved.</summary>
    public DateOnly? ApprovalDate { get; private set; }

    /// <summary>Gets the date the loan was disbursed.</summary>
    public DateOnly? DisbursementDate { get; private set; }

    /// <summary>Gets the expected end date of the loan.</summary>
    public DateOnly? ExpectedEndDate { get; private set; }

    /// <summary>Gets the actual end date of the loan.</summary>
    public DateOnly? ActualEndDate { get; private set; }

    /// <summary>Gets the outstanding principal amount.</summary>
    public decimal OutstandingPrincipal { get; private set; }

    /// <summary>Gets the outstanding interest amount.</summary>
    public decimal OutstandingInterest { get; private set; }

    /// <summary>Gets the total amount paid so far.</summary>
    public decimal TotalPaid { get; private set; }

    /// <summary>Gets the current status of the loan.</summary>
    public string Status { get; private set; } = default!;

    /// <summary>Gets the rejection reason if loan was rejected.</summary>
    public string? RejectionReason { get; private set; }

    /// <summary>Gets the collection of repayments for this loan.</summary>
    public virtual ICollection<LoanRepayment> LoanRepayments { get; private set; } = new List<LoanRepayment>();

    /// <summary>Gets the collection of schedules for this loan.</summary>
    public virtual ICollection<LoanSchedule> LoanSchedules { get; private set; } = new List<LoanSchedule>();

    /// <summary>Gets the collection of guarantors for this loan.</summary>
    public virtual ICollection<LoanGuarantor> LoanGuarantors { get; private set; } = new List<LoanGuarantor>();

    /// <summary>Gets the collection of collaterals for this loan.</summary>
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
        string currency,
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
        Currency = currency;
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
        string currency,
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
            currency,
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

