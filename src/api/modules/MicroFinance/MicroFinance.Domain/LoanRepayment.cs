using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Represents a loan repayment transaction recording actual payments received.
/// </summary>
/// <remarks>
/// <para><strong>Use Cases:</strong></para>
/// <list type="bullet">
///   <item><description>Record principal, interest, and penalty payments</description></item>
///   <item><description>Issue receipts for member records</description></item>
///   <item><description>Track payment methods and channels</description></item>
///   <item><description>Update loan balances and schedule status</description></item>
///   <item><description>Generate collection reports and cash flow analysis</description></item>
/// </list>
/// <para><strong>Business Context:</strong></para>
/// <para>
/// Each LoanRepayment captures a single payment transaction. Payment allocation typically follows:
/// </para>
/// <list type="number">
///   <item><description><strong>Penalties/Fees</strong>: Outstanding penalties are paid first</description></item>
///   <item><description><strong>Interest</strong>: Accrued interest is paid next</description></item>
///   <item><description><strong>Principal</strong>: Remaining amount reduces principal</description></item>
/// </list>
/// <para>
/// Payments may be collected in various ways:
/// - Cash at branch or during group meetings
/// - Mobile money (M-Pesa, GCash, etc.)
/// - Bank transfer or direct debit
/// - Payroll deduction (for salaried borrowers)
/// </para>
/// <para><strong>Related Entities:</strong></para>
/// <list type="bullet">
///   <item><description><see cref="Loan"/> - The loan being repaid</description></item>
///   <item><description><see cref="LoanSchedule"/> - Updated to reflect payment</description></item>
/// </list>
/// </remarks>
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

    /// <summary>Gets internal notes.</summary>
    public new string? Notes { get; private set; }

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
}

