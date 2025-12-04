using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Represents a share transaction (purchase, redemption, transfer, dividend).
/// </summary>
public class ShareTransaction : AuditableEntity, IAggregateRoot
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
    public const string TypePurchase = "Purchase";
    public const string TypeRedemption = "Redemption";
    public const string TypeTransferIn = "TransferIn";
    public const string TypeTransferOut = "TransferOut";
    public const string TypeDividend = "Dividend";
    public const string TypeBonus = "Bonus";

    /// <summary>Gets the share account ID.</summary>
    public DefaultIdType ShareAccountId { get; private set; }

    /// <summary>Gets the share account navigation property.</summary>
    public virtual ShareAccount ShareAccount { get; private set; } = default!;

    /// <summary>Gets the transaction reference number.</summary>
    public string Reference { get; private set; } = default!;

    /// <summary>Gets the transaction type.</summary>
    public string TransactionType { get; private set; } = default!;

    /// <summary>Gets the number of shares involved.</summary>
    public int NumberOfShares { get; private set; }

    /// <summary>Gets the price per share at transaction time.</summary>
    public decimal PricePerShare { get; private set; }

    /// <summary>Gets the total amount.</summary>
    public decimal TotalAmount { get; private set; }

    /// <summary>Gets the shares balance after this transaction.</summary>
    public int SharesBalanceAfter { get; private set; }

    /// <summary>Gets the transaction date.</summary>
    public DateOnly TransactionDate { get; private set; }

    /// <summary>Gets the transaction description.</summary>
    public new string? Description { get; private set; }

    /// <summary>Gets the payment method.</summary>
    public string? PaymentMethod { get; private set; }

    private ShareTransaction() { }

    private ShareTransaction(
        DefaultIdType id,
        DefaultIdType shareAccountId,
        string reference,
        string transactionType,
        int numberOfShares,
        decimal pricePerShare,
        decimal totalAmount,
        int sharesBalanceAfter,
        DateOnly transactionDate,
        string? description,
        string? paymentMethod)
    {
        Id = id;
        ShareAccountId = shareAccountId;
        Reference = reference.Trim();
        TransactionType = transactionType.Trim();
        NumberOfShares = numberOfShares;
        PricePerShare = pricePerShare;
        TotalAmount = totalAmount;
        SharesBalanceAfter = sharesBalanceAfter;
        TransactionDate = transactionDate;
        Description = description?.Trim();
        PaymentMethod = paymentMethod?.Trim();

        QueueDomainEvent(new ShareTransactionCreated { ShareTransaction = this });
    }

    /// <summary>
    /// Creates a new ShareTransaction instance.
    /// </summary>
    public static ShareTransaction Create(
        DefaultIdType shareAccountId,
        string reference,
        string transactionType,
        int numberOfShares,
        decimal pricePerShare,
        int sharesBalanceAfter,
        DateOnly? transactionDate = null,
        string? description = null,
        string? paymentMethod = null)
    {
        return new ShareTransaction(
            DefaultIdType.NewGuid(),
            shareAccountId,
            reference,
            transactionType,
            numberOfShares,
            pricePerShare,
            numberOfShares * pricePerShare,
            sharesBalanceAfter,
            transactionDate ?? DateOnly.FromDateTime(DateTime.UtcNow),
            description,
            paymentMethod);
    }
}
