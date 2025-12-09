using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Represents a loan repayment transaction recording actual payments received.
/// </summary>
/// <remarks>
/// Use cases:
/// - Record principal, interest, and penalty payments.
/// - Issue receipts for member records.
/// - Track payment methods and channels.
/// - Update loan balances and schedule status.
/// - Generate collection reports and cash flow analysis.
/// 
/// Default values and constraints:
/// - LoanId: required, must reference an active loan
/// - ReceiptNumber: auto-generated unique identifier, max 64 characters
/// - PaymentDate: required, date payment was received
/// - Amount: required, total payment amount, must be positive
/// - PrincipalPaid: portion allocated to principal
/// - InterestPaid: portion allocated to interest
/// - PenaltyPaid: portion allocated to penalties/fees
/// - PaymentMethod: required, one of Cash, MobileMoney, BankTransfer, Cheque
/// 
/// Business rules:
/// - Payment allocation order: Penalties → Interest → Principal.
/// - Receipt number must be unique within the system.
/// - Payments cannot exceed total outstanding balance.
/// - Overpayments may be applied as advance payment or refunded.
/// - Reversal requires manager approval and documentation.
/// - Mobile money payments require transaction reference.
/// </remarks>
/// <seealso cref="Loan"/>
/// <seealso cref="LoanSchedule"/>
public class LoanRepayment : AuditableEntity, IAggregateRoot
{
    // Domain Constants
    /// <summary>Maximum length for receipt number. (2^6 = 64)</summary>
    public const int ReceiptNumberMaxLength = 64;

    /// <summary>Maximum length for payment method. (2^5 = 32)</summary>
    public const int PaymentMethodMaxLength = 32;

    /// <summary>Maximum length for notes. (2^11 = 2048)</summary>
    public const int NotesMaxLength = 2048;

    /// <summary>Gets the loan ID this repayment belongs to.</summary>
    public DefaultIdType LoanId { get; private set; }

    /// <summary>Gets the loan navigation property.</summary>
    public virtual Loan Loan { get; private set; } = default!;

    /// <summary>Gets the receipt number.</summary>
    public string ReceiptNumber { get; private set; } = default!;

    /// <summary>Gets the repayment date.</summary>
    public DateOnly RepaymentDate { get; private set; }

    /// <summary>Gets the principal amount paid.</summary>
    public decimal PrincipalAmount { get; private set; }

    /// <summary>Gets the interest amount paid.</summary>
    public decimal InterestAmount { get; private set; }

    /// <summary>Gets the penalty amount paid.</summary>
    public decimal PenaltyAmount { get; private set; }

    /// <summary>Gets the total amount paid.</summary>
    public decimal TotalAmount => PrincipalAmount + InterestAmount + PenaltyAmount;

    /// <summary>Gets the payment method.</summary>
    public string PaymentMethod { get; private set; } = default!;

    private LoanRepayment() { }

    private LoanRepayment(
        DefaultIdType id,
        DefaultIdType loanId,
        string receiptNumber,
        DateOnly repaymentDate,
        decimal principalAmount,
        decimal interestAmount,
        decimal penaltyAmount,
        string paymentMethod,
        string? notes)
    {
        Id = id;
        LoanId = loanId;
        ReceiptNumber = receiptNumber.Trim();
        RepaymentDate = repaymentDate;
        PrincipalAmount = principalAmount;
        InterestAmount = interestAmount;
        PenaltyAmount = penaltyAmount;
        PaymentMethod = paymentMethod.Trim();
        Notes = notes?.Trim();

        QueueDomainEvent(new LoanRepaymentCreated { LoanRepayment = this });
    }

    /// <summary>Maximum length for status. (2^5 = 32)</summary>
    public const int StatusMaxLength = 32;

    // Repayment Statuses
    public const string StatusActive = "Active";
    public const string StatusReversed = "Reversed";

    /// <summary>Gets the current status of the repayment.</summary>
    public string Status { get; private set; } = StatusActive;

    /// <summary>Gets the reason for reversal if reversed.</summary>
    public string? ReversalReason { get; private set; }

    /// <summary>Gets the date of reversal if reversed.</summary>
    public DateOnly? ReversedDate { get; private set; }

    /// <summary>
    /// Creates a new LoanRepayment instance.
    /// </summary>
    public static LoanRepayment Create(
        DefaultIdType loanId,
        string receiptNumber,
        decimal principalAmount,
        decimal interestAmount,
        string paymentMethod,
        decimal penaltyAmount = 0,
        DateOnly? repaymentDate = null,
        string? notes = null)
    {
        return new LoanRepayment(
            DefaultIdType.NewGuid(),
            loanId,
            receiptNumber,
            repaymentDate ?? DateOnly.FromDateTime(DateTime.UtcNow),
            principalAmount,
            interestAmount,
            penaltyAmount,
            paymentMethod,
            notes);
    }

    /// <summary>
    /// Reverses the loan repayment.
    /// </summary>
    public LoanRepayment Reverse(string reason)
    {
        if (Status == StatusReversed)
            throw new InvalidOperationException("Repayment is already reversed.");

        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("Reversal reason is required.", nameof(reason));

        Status = StatusReversed;
        ReversalReason = reason.Trim();
        ReversedDate = DateOnly.FromDateTime(DateTime.UtcNow);

        QueueDomainEvent(new LoanRepaymentReversed { LoanRepaymentId = Id, Reason = reason });
        return this;
    }
}

