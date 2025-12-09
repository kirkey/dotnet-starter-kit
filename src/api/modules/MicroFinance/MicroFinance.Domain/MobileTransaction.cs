using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Represents a mobile money transaction processed through a mobile wallet.
/// Tracks deposits, withdrawals, transfers, and payments via mobile money providers.
/// </summary>
/// <remarks>
/// <para><strong>Use Cases:</strong></para>
/// <list type="bullet">
///   <item><description>Record loan disbursements sent to mobile wallets</description></item>
///   <item><description>Track loan repayments received via mobile money</description></item>
///   <item><description>Log savings deposits from mobile money</description></item>
///   <item><description>Record savings withdrawals to mobile wallets</description></item>
///   <item><description>Maintain audit trail for all mobile money movements</description></item>
///   <item><description>Handle transaction failures and reversals</description></item>
/// </list>
/// <para><strong>Business Context:</strong></para>
/// <para>
/// Mobile transactions integrate with external providers and require careful tracking:
/// </para>
/// <list type="bullet">
///   <item><description><strong>Reference Tracking</strong>: MFI and provider references for reconciliation</description></item>
///   <item><description><strong>Status Management</strong>: Async transactions may be pending, then complete or fail</description></item>
///   <item><description><strong>Fee Handling</strong>: Provider fees may be charged to MFI or member</description></item>
///   <item><description><strong>Reversal Processing</strong>: Failed transactions may need rollback</description></item>
/// </list>
/// <para><strong>Transaction Types:</strong></para>
/// <list type="bullet">
///   <item><description><strong>Deposit</strong>: Cash-in to mobile wallet (usually at agent)</description></item>
///   <item><description><strong>Withdrawal</strong>: Cash-out from mobile wallet</description></item>
///   <item><description><strong>Transfer</strong>: Wallet-to-wallet transfer</description></item>
///   <item><description><strong>LoanRepayment</strong>: Loan payment via mobile money</description></item>
///   <item><description><strong>SavingsDeposit</strong>: Savings deposit via mobile money</description></item>
///   <item><description><strong>BillPayment</strong>: Bill payments through the platform</description></item>
/// </list>
/// <para><strong>Related Entities:</strong></para>
/// <list type="bullet">
///   <item><description><see cref="MobileWallet"/> - Source/destination wallet</description></item>
///   <item><description><see cref="LoanRepayment"/> - Linked loan repayment (if applicable)</description></item>
///   <item><description><see cref="SavingsTransaction"/> - Linked savings transaction (if applicable)</description></item>
/// </list>
/// </remarks>
/// <example>
/// <para><strong>Example: Mobile money loan repayment</strong></para>
/// <code>
/// POST /api/microfinance/mobile-transactions
/// {
///   "walletId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
///   "transactionType": "LoanRepayment",
///   "amount": 50000,
///   "loanId": "a1b2c3d4-5e6f-7890-abcd-ef1234567890",
///   "phoneNumber": "+250788123456",
///   "notes": "Monthly loan installment"
/// }
/// </code>
/// </example>
public sealed class MobileTransaction : AuditableEntity, IAggregateRoot
{
    // Constants
    public const int ReferenceMaxLength = 64;
    public const int TypeMaxLength = 32;
    public const int StatusMaxLength = 32;
    public const int ProviderRefMaxLength = 128;
    public const int PhoneNumberMaxLength = 32;
    public const int NotesMaxLength = 512;
    
    // Transaction Types
    public const string TypeDeposit = "Deposit";
    public const string TypeWithdrawal = "Withdrawal";
    public const string TypeTransfer = "Transfer";
    public const string TypePayment = "Payment";
    public const string TypeAirtime = "Airtime";
    public const string TypeBillPayment = "BillPayment";
    public const string TypeLoanRepayment = "LoanRepayment";
    public const string TypeSavingsDeposit = "SavingsDeposit";
    
    // Transaction Status
    public const string StatusPending = "Pending";
    public const string StatusProcessing = "Processing";
    public const string StatusCompleted = "Completed";
    public const string StatusFailed = "Failed";
    public const string StatusReversed = "Reversed";
    public const string StatusCancelled = "Cancelled";

    public Guid WalletId { get; private set; }
    public string TransactionReference { get; private set; } = default!;
    public string TransactionType { get; private set; } = default!;
    public string Status { get; private set; } = StatusPending;
    public decimal Amount { get; private set; }
    public decimal Fee { get; private set; }
    public decimal NetAmount { get; private set; }
    public string? SourcePhone { get; private set; }
    public string? DestinationPhone { get; private set; }
    public Guid? RecipientWalletId { get; private set; }
    public Guid? LinkedLoanId { get; private set; }
    public Guid? LinkedSavingsAccountId { get; private set; }
    public string? ProviderReference { get; private set; }
    public string? ProviderResponse { get; private set; }
    public DateTimeOffset InitiatedAt { get; private set; }
    public DateTimeOffset? CompletedAt { get; private set; }
    public string? FailureReason { get; private set; }
    public Guid? ReversalOfTransactionId { get; private set; }
    public Guid? ReversedByTransactionId { get; private set; }

    private MobileTransaction() { }

    public static MobileTransaction Create(
        Guid walletId,
        string transactionReference,
        string transactionType,
        decimal amount,
        decimal fee,
        string? sourcePhone = null,
        string? destinationPhone = null)
    {
        var transaction = new MobileTransaction
        {
            WalletId = walletId,
            TransactionReference = transactionReference,
            TransactionType = transactionType,
            Amount = amount,
            Fee = fee,
            NetAmount = amount - fee,
            SourcePhone = sourcePhone,
            DestinationPhone = destinationPhone,
            Status = StatusPending,
            InitiatedAt = DateTimeOffset.UtcNow
        };

        transaction.QueueDomainEvent(new MobileTransactionCreated(transaction));
        return transaction;
    }

    public MobileTransaction MarkProcessing(string providerReference)
    {
        Status = StatusProcessing;
        ProviderReference = providerReference;
        QueueDomainEvent(new MobileTransactionProcessing(Id, providerReference));
        return this;
    }

    public MobileTransaction Complete(string providerResponse)
    {
        Status = StatusCompleted;
        ProviderResponse = providerResponse;
        CompletedAt = DateTimeOffset.UtcNow;
        QueueDomainEvent(new MobileTransactionCompleted(Id, Amount, TransactionReference));
        return this;
    }

    public MobileTransaction Fail(string failureReason)
    {
        Status = StatusFailed;
        FailureReason = failureReason;
        CompletedAt = DateTimeOffset.UtcNow;
        QueueDomainEvent(new MobileTransactionFailed(Id, failureReason));
        return this;
    }

    public MobileTransaction Reverse(Guid reversalTransactionId, string reason)
    {
        Status = StatusReversed;
        ReversedByTransactionId = reversalTransactionId;
        Notes = reason;
        QueueDomainEvent(new MobileTransactionReversed(Id, reversalTransactionId));
        return this;
    }

    public MobileTransaction LinkToLoan(Guid loanId)
    {
        LinkedLoanId = loanId;
        return this;
    }

    public MobileTransaction LinkToSavings(Guid savingsAccountId)
    {
        LinkedSavingsAccountId = savingsAccountId;
        return this;
    }

    public MobileTransaction Update(string? notes = null)
    {
        if (notes is not null) Notes = notes;
        QueueDomainEvent(new MobileTransactionUpdated(this));
        return this;
    }
}
