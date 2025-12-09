using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Represents an investment transaction (buy/sell/dividend/etc).
/// </summary>
/// <remarks>
/// <para><strong>Use Cases:</strong></para>
/// <list type="bullet">
/// <item>Process investment purchases (Buy) and redemptions (Sell)</item>
/// <item>Record dividend distributions and reinvestments</item>
/// <item>Execute Systematic Investment Plans (SIP) periodic contributions</item>
/// <item>Handle product switches and portfolio rebalancing</item>
/// <item>Transfer investments between accounts</item>
/// <item>Track transaction fees, NAV prices, and unit quantities</item>
/// </list>
/// <para><strong>Business Context:</strong></para>
/// <para>
/// Investment transactions record all activity within member investment accounts.
/// Each transaction captures the product, quantity (units), price (NAV), total amount,
/// and applicable fees. Transaction statuses flow from Pending through Processing
/// to Completed or Failed. SIP transactions are automatically generated on schedules.
/// Transactions affect portfolio values, gain/loss calculations, and statement generation.
/// </para>
/// <para><strong>Related Entities:</strong></para>
/// <list type="bullet">
/// <item><see cref="InvestmentAccount"/> - Account holding the investment</item>
/// <item><see cref="InvestmentProduct"/> - Product being traded</item>
/// <item><see cref="SavingsAccount"/> - Source/destination for funds</item>
/// <item><see cref="FeeCharge"/> - Transaction fees applied</item>
/// </list>
/// </remarks>
public sealed class InvestmentTransaction : AuditableEntity, IAggregateRoot
{
    // Constants
    public const int ReferenceMaxLength = 64;
    public const int TypeMaxLength = 32;
    public const int StatusMaxLength = 32;
    public const int NotesMaxLength = 512;
    
    // Transaction Types
    public const string TypeBuy = "Buy";
    public const string TypeSell = "Sell";
    public const string TypeDividend = "Dividend";
    public const string TypeSip = "SIP";
    public const string TypeSwitch = "Switch";
    public const string TypeTransfer = "Transfer";
    
    // Transaction Status
    public const string StatusPending = "Pending";
    public const string StatusProcessing = "Processing";
    public const string StatusCompleted = "Completed";
    public const string StatusFailed = "Failed";
    public const string StatusCancelled = "Cancelled";

    public Guid InvestmentAccountId { get; private set; }
    public Guid ProductId { get; private set; }
    public string TransactionReference { get; private set; } = default!;
    public string TransactionType { get; private set; } = default!;
    public string Status { get; private set; } = StatusPending;
    public decimal Amount { get; private set; }
    public decimal? Units { get; private set; }
    public decimal? NavAtTransaction { get; private set; }
    public decimal? EntryLoadAmount { get; private set; }
    public decimal? ExitLoadAmount { get; private set; }
    public decimal NetAmount { get; private set; }
    public decimal? GainLoss { get; private set; }
    public DateTimeOffset RequestedAt { get; private set; }
    public DateTimeOffset? ProcessedAt { get; private set; }
    public DateOnly? AllotmentDate { get; private set; }
    public Guid? SwitchToProductId { get; private set; }
    public Guid? SourceAccountId { get; private set; }
    public string? PaymentMode { get; private set; }
    public string? PaymentReference { get; private set; }
    public string? Notes { get; private set; }
    public string? FailureReason { get; private set; }

    private InvestmentTransaction() { }

    public static InvestmentTransaction CreateBuy(
        Guid investmentAccountId,
        Guid productId,
        string transactionReference,
        decimal amount,
        decimal? entryLoad = null,
        string? paymentMode = null,
        string? paymentReference = null)
    {
        var transaction = new InvestmentTransaction
        {
            InvestmentAccountId = investmentAccountId,
            ProductId = productId,
            TransactionReference = transactionReference,
            TransactionType = TypeBuy,
            Amount = amount,
            EntryLoadAmount = entryLoad ?? 0,
            NetAmount = amount - (entryLoad ?? 0),
            Status = StatusPending,
            RequestedAt = DateTimeOffset.UtcNow,
            PaymentMode = paymentMode,
            PaymentReference = paymentReference
        };

        transaction.QueueDomainEvent(new InvestmentTransactionCreated(transaction));
        return transaction;
    }

    public static InvestmentTransaction CreateSell(
        Guid investmentAccountId,
        Guid productId,
        string transactionReference,
        decimal units,
        decimal? exitLoad = null)
    {
        var transaction = new InvestmentTransaction
        {
            InvestmentAccountId = investmentAccountId,
            ProductId = productId,
            TransactionReference = transactionReference,
            TransactionType = TypeSell,
            Units = units,
            ExitLoadAmount = exitLoad ?? 0,
            Status = StatusPending,
            RequestedAt = DateTimeOffset.UtcNow
        };

        transaction.QueueDomainEvent(new InvestmentTransactionCreated(transaction));
        return transaction;
    }

    public InvestmentTransaction Process(decimal nav, decimal units, DateOnly allotmentDate)
    {
        Status = StatusProcessing;
        NavAtTransaction = nav;
        Units = TransactionType == TypeBuy ? units : Units;
        Amount = TransactionType == TypeSell ? units * nav : Amount;
        NetAmount = Amount - (EntryLoadAmount ?? 0) - (ExitLoadAmount ?? 0);
        AllotmentDate = allotmentDate;
        ProcessedAt = DateTimeOffset.UtcNow;
        return this;
    }

    public InvestmentTransaction Complete(decimal? gainLoss = null)
    {
        Status = StatusCompleted;
        GainLoss = gainLoss;
        ProcessedAt = DateTimeOffset.UtcNow;
        QueueDomainEvent(new InvestmentTransactionCompleted(Id, TransactionType, NetAmount));
        return this;
    }

    public InvestmentTransaction Fail(string reason)
    {
        Status = StatusFailed;
        FailureReason = reason;
        ProcessedAt = DateTimeOffset.UtcNow;
        QueueDomainEvent(new InvestmentTransactionFailed(Id, reason));
        return this;
    }

    public InvestmentTransaction Cancel()
    {
        Status = StatusCancelled;
        ProcessedAt = DateTimeOffset.UtcNow;
        return this;
    }

    public InvestmentTransaction Update(string? notes = null)
    {
        if (notes is not null) Notes = notes;
        QueueDomainEvent(new InvestmentTransactionUpdated(this));
        return this;
    }
}
