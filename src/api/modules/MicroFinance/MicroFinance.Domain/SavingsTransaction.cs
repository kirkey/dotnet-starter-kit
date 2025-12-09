using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Represents a transaction on a savings account.
/// </summary>
/// <remarks>
/// Use cases:
/// - Record all deposits and withdrawals with audit trail.
/// - Track interest postings and fee deductions.
/// - Maintain running balance after each transaction.
/// - Support account reconciliation and statement generation.
/// - Enable transaction reversal and adjustments.
/// 
/// Default values and constraints:
/// - SavingsAccountId: required, must reference an active savings account
/// - Reference: auto-generated unique identifier, max 64 characters
/// - TransactionType: required, one of Deposit, Withdrawal, Interest, Fee, TransferIn, TransferOut
/// - Amount: required, must be positive
/// - TransactionDate: required, date transaction occurred
/// - RunningBalance: calculated, balance after this transaction
/// - Description: optional, max 256 characters
/// 
/// Business rules:
/// - Withdrawals cannot exceed available balance.
/// - Interest transactions are system-generated during posting.
/// - Fee transactions require fee definition reference.
/// - Reversals create offsetting transactions (not deletions).
/// - Running balance must be recalculated on reversal.
/// - Frozen accounts cannot have new transactions.
/// </remarks>
/// <seealso cref="SavingsAccount"/>
public class SavingsTransaction : AuditableEntity, IAggregateRoot
{
    // Domain Constants
    /// <summary>Maximum length for transaction reference. (2^6 = 64)</summary>
    public const int ReferenceMaxLength = 64;

    /// <summary>Maximum length for transaction type. (2^5 = 32)</summary>
    public const int TransactionTypeMaxLength = 32;

    /// <summary>Maximum length for description. (2^8 = 256)</summary>
    public const int DescriptionMaxLength = 256;

    /// <summary>Maximum length for payment method. (2^5 = 32)</summary>
    public const int PaymentMethodMaxLength = 32;

    // Transaction Types
    public const string TypeDeposit = "Deposit";
    public const string TypeWithdrawal = "Withdrawal";
    public const string TypeInterest = "Interest";
    public const string TypeFee = "Fee";
    public const string TypeTransferIn = "TransferIn";
    public const string TypeTransferOut = "TransferOut";

    /// <summary>Gets the savings account ID.</summary>
    public DefaultIdType SavingsAccountId { get; private set; }

    /// <summary>Gets the savings account navigation property.</summary>
    public virtual SavingsAccount SavingsAccount { get; private set; } = default!;

    /// <summary>Gets the transaction reference number.</summary>
    public string Reference { get; private set; } = default!;

    /// <summary>Gets the transaction type.</summary>
    public string TransactionType { get; private set; } = default!;

    /// <summary>Gets the transaction amount.</summary>
    public decimal Amount { get; private set; }

    /// <summary>Gets the balance after this transaction.</summary>
    public decimal BalanceAfter { get; private set; }

    /// <summary>Gets the transaction date.</summary>
    public DateOnly TransactionDate { get; private set; }

    /// <summary>Gets the payment method.</summary>
    public string? PaymentMethod { get; private set; }

    private SavingsTransaction() { }

    private SavingsTransaction(
        DefaultIdType id,
        DefaultIdType savingsAccountId,
        string reference,
        string transactionType,
        decimal amount,
        decimal balanceAfter,
        DateOnly transactionDate,
        string? description,
        string? paymentMethod)
    {
        Id = id;
        SavingsAccountId = savingsAccountId;
        Reference = reference.Trim();
        TransactionType = transactionType.Trim();
        Amount = amount;
        BalanceAfter = balanceAfter;
        TransactionDate = transactionDate;
        Description = description?.Trim();
        PaymentMethod = paymentMethod?.Trim();

        QueueDomainEvent(new SavingsTransactionCreated { SavingsTransaction = this });
    }

    /// <summary>
    /// Creates a new SavingsTransaction instance.
    /// </summary>
    public static SavingsTransaction Create(
        DefaultIdType savingsAccountId,
        string reference,
        string transactionType,
        decimal amount,
        decimal balanceAfter,
        DateOnly? transactionDate = null,
        string? description = null,
        string? paymentMethod = null)
    {
        return new SavingsTransaction(
            DefaultIdType.NewGuid(),
            savingsAccountId,
            reference,
            transactionType,
            amount,
            balanceAfter,
            transactionDate ?? DateOnly.FromDateTime(DateTime.UtcNow),
            description,
            paymentMethod);
    }
}

