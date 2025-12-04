using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Represents a loan issued to a member in the microfinance system.
/// </summary>
public class Loan : AuditableEntity, IAggregateRoot
{
    // Domain Constants
    /// <summary>Maximum length for loan number. (2^6 = 64)</summary>
    public const int LoanNumberMaxLength = 64;

    /// <summary>Maximum length for status. (2^5 = 32)</summary>
    public const int StatusMaxLength = 32;

    /// <summary>Maximum length for purpose. (2^9 = 512)</summary>
    public const int PurposeMaxLength = 512;

    /// <summary>Maximum length for notes. (2^12 = 4096)</summary>
    public const int NotesMaxLength = 4096;

    // Loan Statuses
    public const string StatusPending = "Pending";
    public const string StatusApproved = "Approved";
    public const string StatusDisbursed = "Disbursed";
    public const string StatusActive = "Active";
    public const string StatusPaidOff = "PaidOff";
    public const string StatusDefaulted = "Defaulted";
    public const string StatusWrittenOff = "WrittenOff";
    public const string StatusRejected = "Rejected";
    public const string StatusCancelled = "Cancelled";

    /// <summary>Gets the unique loan number.</summary>
    public string LoanNumber { get; private set; } = default!;

    /// <summary>Gets the member ID who owns this loan.</summary>
    public DefaultIdType MemberId { get; private set; }

    /// <summary>Gets the member navigation property.</summary>
    public virtual Member Member { get; private set; } = default!;

    /// <summary>Gets the loan product ID.</summary>
    public DefaultIdType LoanProductId { get; private set; }

    /// <summary>Gets the loan product navigation property.</summary>
    public virtual LoanProduct LoanProduct { get; private set; } = default!;

    /// <summary>Gets the principal amount of the loan.</summary>
    public decimal PrincipalAmount { get; private set; }

    /// <summary>Gets the interest rate applied to this loan.</summary>
    public decimal InterestRate { get; private set; }

    /// <summary>Gets the loan term in months.</summary>
    public int TermMonths { get; private set; }

    /// <summary>Gets the purpose of the loan.</summary>
    public string? Purpose { get; private set; }

    /// <summary>Gets the date the loan application was submitted.</summary>
    public DateOnly ApplicationDate { get; private set; }

    /// <summary>Gets the date the loan was approved.</summary>
    public DateOnly? ApprovalDate { get; private set; }

    /// <summary>Gets the date the loan was disbursed.</summary>
    public DateOnly? DisbursementDate { get; private set; }

    /// <summary>Gets the date the first repayment is due.</summary>
    public DateOnly? FirstRepaymentDate { get; private set; }

    /// <summary>Gets the maturity date of the loan.</summary>
    public DateOnly? MaturityDate { get; private set; }

    /// <summary>Gets the current status of the loan.</summary>
    public string Status { get; private set; } = default!;

    /// <summary>Gets the total amount to be repaid (principal + interest).</summary>
    public decimal TotalRepayable { get; private set; }

    /// <summary>Gets the total amount repaid so far.</summary>
    public decimal TotalRepaid { get; private set; }

    /// <summary>Gets the outstanding balance.</summary>
    public decimal OutstandingBalance => TotalRepayable - TotalRepaid;

    /// <summary>Gets internal notes about the loan.</summary>
    public new string? Notes { get; private set; }

    /// <summary>Gets the collection of repayments for this loan.</summary>
    public virtual ICollection<LoanRepayment> Repayments { get; private set; } = new List<LoanRepayment>();

    private Loan() { }

    private Loan(
        DefaultIdType id,
        string loanNumber,
        DefaultIdType memberId,
        DefaultIdType loanProductId,
        decimal principalAmount,
        decimal interestRate,
        int termMonths,
        string? purpose,
        DateOnly applicationDate,
        string? notes)
    {
        Id = id;
        LoanNumber = loanNumber.Trim();
        MemberId = memberId;
        LoanProductId = loanProductId;
        PrincipalAmount = principalAmount;
        InterestRate = interestRate;
        TermMonths = termMonths;
        Purpose = purpose?.Trim();
        ApplicationDate = applicationDate;
        Status = StatusPending;
        TotalRepayable = CalculateTotalRepayable(principalAmount, interestRate, termMonths);
        TotalRepaid = 0;
        Notes = notes?.Trim();

        QueueDomainEvent(new LoanCreated { Loan = this });
    }

    /// <summary>
    /// Creates a new Loan instance.
    /// </summary>
    public static Loan Create(
        string loanNumber,
        DefaultIdType memberId,
        DefaultIdType loanProductId,
        decimal principalAmount,
        decimal interestRate,
        int termMonths,
        string? purpose = null,
        DateOnly? applicationDate = null,
        string? notes = null)
    {
        return new Loan(
            DefaultIdType.NewGuid(),
            loanNumber,
            memberId,
            loanProductId,
            principalAmount,
            interestRate,
            termMonths,
            purpose,
            applicationDate ?? DateOnly.FromDateTime(DateTime.UtcNow),
            notes);
    }

    /// <summary>
    /// Approves the loan application.
    /// </summary>
    public Loan Approve(DateOnly? approvalDate = null)
    {
        if (Status != StatusPending)
            throw new InvalidOperationException($"Cannot approve loan in {Status} status.");

        ApprovalDate = approvalDate ?? DateOnly.FromDateTime(DateTime.UtcNow);
        Status = StatusApproved;

        QueueDomainEvent(new LoanApproved { Loan = this });
        return this;
    }

    /// <summary>
    /// Rejects the loan application.
    /// </summary>
    public Loan Reject(string? reason = null)
    {
        if (Status != StatusPending)
            throw new InvalidOperationException($"Cannot reject loan in {Status} status.");

        Status = StatusRejected;
        if (!string.IsNullOrWhiteSpace(reason))
        {
            Notes = string.IsNullOrWhiteSpace(Notes) ? $"Rejected: {reason}" : $"{Notes}\nRejected: {reason}";
        }

        QueueDomainEvent(new LoanRejected { LoanId = Id, Reason = reason });
        return this;
    }

    /// <summary>
    /// Disburses the loan to the member.
    /// </summary>
    public Loan Disburse(DateOnly? disbursementDate = null, DateOnly? firstRepaymentDate = null)
    {
        if (Status != StatusApproved)
            throw new InvalidOperationException($"Cannot disburse loan in {Status} status.");

        DisbursementDate = disbursementDate ?? DateOnly.FromDateTime(DateTime.UtcNow);
        FirstRepaymentDate = firstRepaymentDate ?? DisbursementDate.Value.AddMonths(1);
        MaturityDate = DisbursementDate.Value.AddMonths(TermMonths);
        Status = StatusDisbursed;

        QueueDomainEvent(new LoanDisbursed { Loan = this });
        return this;
    }

    /// <summary>
    /// Records a repayment against the loan.
    /// </summary>
    public Loan RecordRepayment(decimal amount)
    {
        if (Status != StatusDisbursed && Status != StatusActive)
            throw new InvalidOperationException($"Cannot record repayment for loan in {Status} status.");

        if (amount <= 0)
            throw new ArgumentException("Repayment amount must be positive.", nameof(amount));

        TotalRepaid += amount;

        if (Status == StatusDisbursed)
            Status = StatusActive;

        if (TotalRepaid >= TotalRepayable)
        {
            Status = StatusPaidOff;
            QueueDomainEvent(new LoanPaidOff { LoanId = Id });
        }

        return this;
    }

    /// <summary>
    /// Marks the loan as defaulted.
    /// </summary>
    public Loan MarkAsDefaulted(string? reason = null)
    {
        if (Status != StatusActive && Status != StatusDisbursed)
            throw new InvalidOperationException($"Cannot mark loan as defaulted in {Status} status.");

        Status = StatusDefaulted;
        if (!string.IsNullOrWhiteSpace(reason))
        {
            Notes = string.IsNullOrWhiteSpace(Notes) ? $"Defaulted: {reason}" : $"{Notes}\nDefaulted: {reason}";
        }

        QueueDomainEvent(new LoanDefaulted { LoanId = Id, Reason = reason });
        return this;
    }

    /// <summary>
    /// Calculates the total amount to be repaid using simple interest.
    /// </summary>
    private static decimal CalculateTotalRepayable(decimal principal, decimal annualRate, int termMonths)
    {
        decimal interest = principal * (annualRate / 100) * (termMonths / 12m);
        return principal + interest;
    }
}

