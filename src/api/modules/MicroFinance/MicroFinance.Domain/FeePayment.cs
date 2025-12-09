using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Represents a payment transaction against a fee charge.
/// Tracks how fees are settled through various payment sources.
/// </summary>
/// <remarks>
/// Use cases:
/// - Record cash payments toward outstanding fees.
/// - Apply fee deductions from loan repayments.
/// - Track mobile money fee payments.
/// - Handle partial fee payments with balance tracking.
/// - Reverse erroneous fee payments.
/// - Audit trail for fee payment receipts.
/// 
/// Default values and constraints:
/// - Reference: Unique payment identifier (generated).
/// - Amount: Payment amount (positive decimal, required).
/// - PaymentSource: Cash, LoanRepayment, SavingsDeduction, MobileMoney, BankTransfer.
/// - PaymentDate: Date payment was received (required).
/// - Status: Active or Reversed.
/// 
/// Business rules:
/// - Fee payments reduce outstanding fee balances.
/// - Fees tracked separately from principal/interest.
/// - Multiple sources: Cash, savings, or included in loan payments.
/// - Fee payment order may follow regulatory guidelines.
/// - Each payment generates a unique reference/receipt.
/// - Reversals require appropriate authorization.
/// </remarks>
/// <seealso cref="FeeCharge"/>
/// <seealso cref="LoanRepayment"/>
/// <seealso cref="SavingsTransaction"/>
/// <example>
/// <para><strong>Example: Recording a fee payment</strong></para>
/// <code>
/// POST /api/microfinance/fee-payments
/// {
///   "feeChargeId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
///   "amount": 5000,
///   "paymentMethod": "Cash",
///   "paymentSource": "Cash",
///   "paymentDate": "2024-01-15",
///   "notes": "Late fee payment for loan L-2024-001"
/// }
/// </code>
/// </example>
public class FeePayment : AuditableEntity, IAggregateRoot
{
    // Domain Constants - Max Lengths
    public const int ReferenceMaxLength = 64;
    public const int PaymentMethodMaxLength = 32;
    public const int PaymentSourceMaxLength = 32;
    public const int StatusMaxLength = 32;
    public const int ReversalReasonMaxLength = 512;
    public const int NotesMaxLength = 2048;

    // Payment Sources
    public const string SourceCash = "Cash";
    public const string SourceLoanRepayment = "LoanRepayment";
    public const string SourceSavingsDeduction = "SavingsDeduction";
    public const string SourceMobileMoney = "MobileMoney";
    public const string SourceBankTransfer = "BankTransfer";

    // Payment Methods
    public const string MethodCash = "Cash";
    public const string MethodMobileMoney = "MobileMoney";
    public const string MethodBankTransfer = "BankTransfer";
    public const string MethodCheck = "Check";
    public const string MethodAutoDeduction = "AutoDeduction";

    // Payment Statuses
    public const string StatusActive = "Active";
    public const string StatusReversed = "Reversed";

    /// <summary>FK to FeeCharge being paid.</summary>
    public DefaultIdType FeeChargeId { get; private set; }

    /// <summary>Optional FK to LoanRepayment if paid through loan payment.</summary>
    public DefaultIdType? LoanRepaymentId { get; private set; }

    /// <summary>Optional FK to SavingsTransaction if paid from savings.</summary>
    public DefaultIdType? SavingsTransactionId { get; private set; }

    /// <summary>Unique payment reference/receipt number.</summary>
    public string Reference { get; private set; } = default!;

    /// <summary>Date of payment.</summary>
    public DateOnly PaymentDate { get; private set; }

    /// <summary>Amount paid.</summary>
    public decimal Amount { get; private set; }

    /// <summary>Payment method (Cash, MobileMoney, etc.).</summary>
    public string PaymentMethod { get; private set; } = default!;

    /// <summary>Source of payment funds.</summary>
    public string PaymentSource { get; private set; } = default!;

    /// <summary>Current status (Active/Reversed).</summary>
    public string Status { get; private set; } = StatusActive;

    /// <summary>Reason if reversed.</summary>
    public string? ReversalReason { get; private set; }

    /// <summary>Date if reversed.</summary>
    public DateOnly? ReversedDate { get; private set; }

    // Navigation properties
    public FeeCharge FeeCharge { get; private set; } = null!;

    private FeePayment() { }

    private FeePayment(
        DefaultIdType id,
        DefaultIdType feeChargeId,
        string reference,
        DateOnly paymentDate,
        decimal amount,
        string paymentMethod,
        string paymentSource,
        DefaultIdType? loanRepaymentId,
        DefaultIdType? savingsTransactionId,
        string? notes)
    {
        Id = id;
        FeeChargeId = feeChargeId;
        Reference = reference.Trim();
        PaymentDate = paymentDate;
        Amount = amount;
        PaymentMethod = paymentMethod.Trim();
        PaymentSource = paymentSource.Trim();
        LoanRepaymentId = loanRepaymentId;
        SavingsTransactionId = savingsTransactionId;
        Status = StatusActive;
        Notes = notes?.Trim();

        QueueDomainEvent(new FeePaymentCreated(this));
    }

    public static FeePayment Create(
        DefaultIdType feeChargeId,
        string reference,
        decimal amount,
        string paymentMethod,
        string paymentSource,
        DateOnly? paymentDate = null,
        DefaultIdType? loanRepaymentId = null,
        DefaultIdType? savingsTransactionId = null,
        string? notes = null)
    {
        return new FeePayment(
            DefaultIdType.NewGuid(),
            feeChargeId,
            reference,
            paymentDate ?? DateOnly.FromDateTime(DateTime.UtcNow),
            amount,
            paymentMethod,
            paymentSource,
            loanRepaymentId,
            savingsTransactionId,
            notes);
    }

    public FeePayment Update(
        string? reference = null,
        DateOnly? paymentDate = null,
        decimal? amount = null,
        string? paymentMethod = null,
        string? paymentSource = null,
        string? notes = null)
    {
        if (Status == StatusReversed)
            throw new InvalidOperationException("Cannot update a reversed payment.");

        if (!string.IsNullOrWhiteSpace(reference))
        {
            Reference = reference.Trim();
        }

        if (paymentDate.HasValue)
        {
            PaymentDate = paymentDate.Value;
        }

        if (amount.HasValue && amount.Value > 0)
        {
            Amount = amount.Value;
        }

        if (!string.IsNullOrWhiteSpace(paymentMethod))
        {
            PaymentMethod = paymentMethod.Trim();
        }

        if (!string.IsNullOrWhiteSpace(paymentSource))
        {
            PaymentSource = paymentSource.Trim();
        }

        Notes = notes?.Trim() ?? Notes;

        QueueDomainEvent(new FeePaymentUpdated(this));
        return this;
    }

    public FeePayment Reverse(string reason)
    {
        if (Status == StatusReversed)
            throw new InvalidOperationException("Payment is already reversed.");

        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("Reversal reason is required.", nameof(reason));

        Status = StatusReversed;
        ReversalReason = reason.Trim();
        ReversedDate = DateOnly.FromDateTime(DateTime.UtcNow);

        QueueDomainEvent(new FeePaymentReversed(Id, reason));
        return this;
    }
}
