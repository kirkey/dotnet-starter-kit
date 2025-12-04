using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Represents a share account for member equity ownership in the microfinance institution.
/// </summary>
/// <remarks>
/// <para><strong>Use Cases:</strong></para>
/// <list type="bullet">
///   <item><description>Track member share ownership and total equity value</description></item>
///   <item><description>Record share purchases, redemptions, and transfers</description></item>
///   <item><description>Calculate and distribute dividends to shareholders</description></item>
///   <item><description>Verify membership eligibility based on minimum share requirements</description></item>
///   <item><description>Manage share certificates and ownership records</description></item>
/// </list>
/// <para><strong>Business Context:</strong></para>
/// <para>
/// In cooperative MFIs, share accounts represent member ownership stakes. Share capital:
/// </para>
/// <list type="bullet">
///   <item><description>Provides permanent capital for lending operations</description></item>
///   <item><description>Determines dividend distribution (proportional to shares held)</description></item>
///   <item><description>May determine voting power in cooperative governance</description></item>
///   <item><description>Acts as security for member loans (shares can be frozen as collateral)</description></item>
/// </list>
/// <para>
/// Unlike savings which can be withdrawn freely, shares typically have restrictions
/// (holding periods, redemption limits) to protect the institution's capital base.
/// </para>
/// <para><strong>Related Entities:</strong></para>
/// <list type="bullet">
///   <item><description><see cref="ShareProduct"/> - Product defining share terms</description></item>
///   <item><description><see cref="Member"/> - Share owner</description></item>
///   <item><description><see cref="ShareTransaction"/> - Purchase, redemption, dividend transactions</description></item>
/// </list>
/// </remarks>
public class ShareAccount : AuditableEntity, IAggregateRoot
{
    // Domain Constants
    /// <summary>Maximum length for account number field. (2^6 = 64)</summary>
    public const int AccountNumberMaxLength = 64;

    /// <summary>Maximum length for status field. (2^5 = 32)</summary>
    public const int StatusMaxLength = 32;

    /// <summary>Maximum length for notes field. (2^12 = 4096)</summary>
    public const int NotesMaxLength = 4096;

    // Share Account Statuses
    public const string StatusActive = "Active";
    public const string StatusInactive = "Inactive";
    public const string StatusClosed = "Closed";

    /// <summary>Gets the unique account number.</summary>
    public string AccountNumber { get; private set; } = default!;

    /// <summary>Gets the member ID who owns this account.</summary>
    public DefaultIdType MemberId { get; private set; }

    /// <summary>Gets the member navigation property.</summary>
    public virtual Member Member { get; private set; } = default!;

    /// <summary>Gets the share product ID.</summary>
    public DefaultIdType ShareProductId { get; private set; }

    /// <summary>Gets the share product navigation property.</summary>
    public virtual ShareProduct ShareProduct { get; private set; } = default!;

    /// <summary>Gets the number of shares held.</summary>
    public int NumberOfShares { get; private set; }

    /// <summary>Gets the total share value.</summary>
    public decimal TotalShareValue { get; private set; }

    /// <summary>Gets the total purchases made.</summary>
    public decimal TotalPurchases { get; private set; }

    /// <summary>Gets the total redemptions made.</summary>
    public decimal TotalRedemptions { get; private set; }

    /// <summary>Gets the total dividends earned.</summary>
    public decimal TotalDividendsEarned { get; private set; }

    /// <summary>Gets the total dividends paid.</summary>
    public decimal TotalDividendsPaid { get; private set; }

    /// <summary>Gets the date the account was opened.</summary>
    public DateOnly OpenedDate { get; private set; }

    /// <summary>Gets the date the account was closed.</summary>
    public DateOnly? ClosedDate { get; private set; }

    /// <summary>Gets the current status.</summary>
    public string Status { get; private set; } = default!;

    /// <summary>Gets the collection of share transactions.</summary>
    public virtual ICollection<ShareTransaction> Transactions { get; private set; } = new List<ShareTransaction>();

    private ShareAccount() { }

    private ShareAccount(
        DefaultIdType id,
        string accountNumber,
        DefaultIdType memberId,
        DefaultIdType shareProductId,
        DateOnly openedDate,
        string? notes)
    {
        Id = id;
        AccountNumber = accountNumber.Trim();
        MemberId = memberId;
        ShareProductId = shareProductId;
        NumberOfShares = 0;
        TotalShareValue = 0;
        TotalPurchases = 0;
        TotalRedemptions = 0;
        TotalDividendsEarned = 0;
        TotalDividendsPaid = 0;
        OpenedDate = openedDate;
        Status = StatusActive;
        Notes = notes?.Trim();

        QueueDomainEvent(new ShareAccountCreated { ShareAccount = this });
    }

    /// <summary>
    /// Creates a new ShareAccount instance.
    /// </summary>
    public static ShareAccount Create(
        string accountNumber,
        DefaultIdType memberId,
        DefaultIdType shareProductId,
        DateOnly? openedDate = null,
        string? notes = null)
    {
        return new ShareAccount(
            DefaultIdType.NewGuid(),
            accountNumber,
            memberId,
            shareProductId,
            openedDate ?? DateOnly.FromDateTime(DateTime.UtcNow),
            notes);
    }

    /// <summary>
    /// Records purchase of shares.
    /// </summary>
    public ShareAccount PurchaseShares(int numberOfShares, decimal pricePerShare)
    {
        if (Status != StatusActive)
            throw new InvalidOperationException($"Cannot purchase shares for account in {Status} status.");

        if (numberOfShares <= 0)
            throw new ArgumentException("Number of shares must be positive.", nameof(numberOfShares));

        if (pricePerShare <= 0)
            throw new ArgumentException("Price per share must be positive.", nameof(pricePerShare));

        decimal totalAmount = numberOfShares * pricePerShare;
        NumberOfShares += numberOfShares;
        TotalShareValue += totalAmount;
        TotalPurchases += totalAmount;

        QueueDomainEvent(new SharesPurchased { AccountId = Id, Shares = numberOfShares, Amount = totalAmount });
        return this;
    }

    /// <summary>
    /// Records redemption (sale) of shares.
    /// </summary>
    public ShareAccount RedeemShares(int numberOfShares, decimal pricePerShare)
    {
        if (Status != StatusActive)
            throw new InvalidOperationException($"Cannot redeem shares for account in {Status} status.");

        if (numberOfShares <= 0)
            throw new ArgumentException("Number of shares must be positive.", nameof(numberOfShares));

        if (numberOfShares > NumberOfShares)
            throw new InvalidOperationException("Cannot redeem more shares than held.");

        if (pricePerShare <= 0)
            throw new ArgumentException("Price per share must be positive.", nameof(pricePerShare));

        decimal totalAmount = numberOfShares * pricePerShare;
        NumberOfShares -= numberOfShares;
        TotalShareValue -= totalAmount;
        TotalRedemptions += totalAmount;

        QueueDomainEvent(new SharesRedeemed { AccountId = Id, Shares = numberOfShares, Amount = totalAmount });
        return this;
    }

    /// <summary>
    /// Posts dividend earnings.
    /// </summary>
    public ShareAccount PostDividend(decimal dividendAmount)
    {
        if (Status != StatusActive)
            throw new InvalidOperationException($"Cannot post dividend for account in {Status} status.");

        if (dividendAmount <= 0)
            throw new ArgumentException("Dividend amount must be positive.", nameof(dividendAmount));

        TotalDividendsEarned += dividendAmount;

        QueueDomainEvent(new ShareDividendPosted { AccountId = Id, Amount = dividendAmount });
        return this;
    }

    /// <summary>
    /// Records dividend payout.
    /// </summary>
    public ShareAccount PayDividend(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Payout amount must be positive.", nameof(amount));

        if (amount > TotalDividendsEarned - TotalDividendsPaid)
            throw new InvalidOperationException("Payout amount exceeds available dividends.");

        TotalDividendsPaid += amount;
        return this;
    }

    /// <summary>
    /// Closes the share account.
    /// </summary>
    public ShareAccount Close(string? reason = null)
    {
        if (Status == StatusClosed)
            throw new InvalidOperationException("Account is already closed.");

        if (NumberOfShares > 0)
            throw new InvalidOperationException("Cannot close account with outstanding shares. Redeem shares first.");

        Status = StatusClosed;
        ClosedDate = DateOnly.FromDateTime(DateTime.UtcNow);

        if (!string.IsNullOrWhiteSpace(reason))
        {
            Notes = string.IsNullOrWhiteSpace(Notes) ? $"Closed: {reason}" : $"{Notes}\nClosed: {reason}";
        }

        QueueDomainEvent(new ShareAccountClosed { AccountId = Id });
        return this;
    }
}
