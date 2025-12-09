using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Represents a mobile wallet linked to a member for mobile money transactions.
/// Enables digital financial services through mobile network operators or fintech providers.
/// </summary>
/// <remarks>
/// <para><strong>Use Cases:</strong></para>
/// <list type="bullet">
///   <item><description>Link member accounts to mobile money providers (MTN, Airtel, M-Pesa)</description></item>
///   <item><description>Enable digital loan disbursements directly to mobile wallets</description></item>
///   <item><description>Accept loan repayments via mobile money</description></item>
///   <item><description>Support savings deposits and withdrawals through mobile channels</description></item>
///   <item><description>Track wallet balances and transaction limits</description></item>
///   <item><description>Manage KYC tier levels for regulatory compliance</description></item>
/// </list>
/// <para><strong>Business Context:</strong></para>
/// <para>
/// Mobile money integration is essential for reaching unbanked populations in emerging markets:
/// </para>
/// <list type="bullet">
///   <item><description><strong>Reach</strong>: Serve members without access to physical branches</description></item>
///   <item><description><strong>Convenience</strong>: 24/7 transactions from anywhere</description></item>
///   <item><description><strong>Cost Reduction</strong>: Lower operational costs vs. branch transactions</description></item>
///   <item><description><strong>Speed</strong>: Instant disbursements and repayments</description></item>
/// </list>
/// <para><strong>Wallet Tiers:</strong></para>
/// <list type="bullet">
///   <item><description><strong>Basic</strong>: Low limits, minimal KYC (phone number only)</description></item>
///   <item><description><strong>Standard</strong>: Medium limits, ID verification required</description></item>
///   <item><description><strong>Premium</strong>: High limits, full KYC including address verification</description></item>
/// </list>
/// <para><strong>Related Entities:</strong></para>
/// <list type="bullet">
///   <item><description><see cref="Member"/> - Wallet owner</description></item>
///   <item><description><see cref="MobileTransaction"/> - Transactions on this wallet</description></item>
///   <item><description><see cref="PaymentGateway"/> - Provider integration configuration</description></item>
/// </list>
/// </remarks>
/// <example>
/// <para><strong>Example: Linking a mobile wallet to a member</strong></para>
/// <code>
/// POST /api/microfinance/mobile-wallets
/// {
///   "memberId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
///   "phoneNumber": "+250788123456",
///   "provider": "MTN Mobile Money",
///   "tier": "Standard",
///   "dailyLimit": 1000000,
///   "monthlyLimit": 5000000
/// }
/// </code>
/// </example>
public sealed class MobileWallet : AuditableEntity, IAggregateRoot
{
    // Constants
    public const int PhoneNumberMaxLength = 32;
    public const int ProviderMaxLength = 64;
    public const int WalletIdMaxLength = 64;
    public const int StatusMaxLength = 32;
    public const int TierMaxLength = 32;
    
    // Wallet Status
    public const string StatusActive = "Active";
    public const string StatusInactive = "Inactive";
    public const string StatusSuspended = "Suspended";
    public const string StatusBlocked = "Blocked";
    public const string StatusPendingVerification = "PendingVerification";
    
    // Wallet Tier
    public const string TierBasic = "Basic";
    public const string TierStandard = "Standard";
    public const string TierPremium = "Premium";

    public Guid MemberId { get; private set; }
    public string PhoneNumber { get; private set; } = default!;
    public string Provider { get; private set; } = default!;
    public string? ExternalWalletId { get; private set; }
    public string Status { get; private set; } = StatusActive;
    public string Tier { get; private set; } = TierBasic;
    public decimal Balance { get; private set; }
    public decimal DailyLimit { get; private set; }
    public decimal MonthlyLimit { get; private set; }
    public decimal DailyUsed { get; private set; }
    public decimal MonthlyUsed { get; private set; }
    public DateOnly? KycVerifiedDate { get; private set; }
    public DateOnly? LastActivityDate { get; private set; }
    public bool IsLinkedToBankAccount { get; private set; }
    public Guid? LinkedSavingsAccountId { get; private set; }
    public bool IsPinSet { get; private set; }
    public string? PinHash { get; private set; }
    public int FailedPinAttempts { get; private set; }
    public DateTimeOffset? PinBlockedUntil { get; private set; }

    private MobileWallet() { }

    public static MobileWallet Create(
        Guid memberId,
        string phoneNumber,
        string provider,
        decimal dailyLimit,
        decimal monthlyLimit)
    {
        var wallet = new MobileWallet
        {
            MemberId = memberId,
            PhoneNumber = phoneNumber,
            Provider = provider,
            DailyLimit = dailyLimit,
            MonthlyLimit = monthlyLimit,
            Balance = 0,
            DailyUsed = 0,
            MonthlyUsed = 0,
            Status = StatusPendingVerification,
            Tier = TierBasic
        };

        wallet.QueueDomainEvent(new MobileWalletCreated(wallet));
        return wallet;
    }

    public MobileWallet Activate()
    {
        Status = StatusActive;
        QueueDomainEvent(new MobileWalletActivated(Id, PhoneNumber));
        return this;
    }

    public MobileWallet Credit(decimal amount, string transactionRef)
    {
        Balance += amount;
        LastActivityDate = DateOnly.FromDateTime(DateTime.UtcNow);
        QueueDomainEvent(new MobileWalletCredited(Id, amount, transactionRef));
        return this;
    }

    public MobileWallet Debit(decimal amount, string transactionRef)
    {
        if (amount > Balance)
            throw new InvalidOperationException("Insufficient wallet balance.");

        Balance -= amount;
        DailyUsed += amount;
        MonthlyUsed += amount;
        LastActivityDate = DateOnly.FromDateTime(DateTime.UtcNow);
        QueueDomainEvent(new MobileWalletDebited(Id, amount, transactionRef));
        return this;
    }

    public MobileWallet Suspend(string reason)
    {
        Status = StatusSuspended;
        QueueDomainEvent(new MobileWalletSuspended(Id, reason));
        return this;
    }

    public MobileWallet LinkToSavingsAccount(Guid savingsAccountId)
    {
        LinkedSavingsAccountId = savingsAccountId;
        IsLinkedToBankAccount = true;
        QueueDomainEvent(new MobileWalletLinkedToAccount(Id, savingsAccountId));
        return this;
    }

    public MobileWallet UpgradeTier(string newTier, decimal newDailyLimit, decimal newMonthlyLimit)
    {
        Tier = newTier;
        DailyLimit = newDailyLimit;
        MonthlyLimit = newMonthlyLimit;
        QueueDomainEvent(new MobileWalletTierUpgraded(Id, newTier));
        return this;
    }

    public MobileWallet Update(
        string? provider = null,
        string? externalWalletId = null,
        decimal? dailyLimit = null,
        decimal? monthlyLimit = null)
    {
        if (provider is not null) Provider = provider;
        if (externalWalletId is not null) ExternalWalletId = externalWalletId;
        if (dailyLimit.HasValue) DailyLimit = dailyLimit.Value;
        if (monthlyLimit.HasValue) MonthlyLimit = monthlyLimit.Value;

        QueueDomainEvent(new MobileWalletUpdated(this));
        return this;
    }

    /// <summary>
    /// Blocks the wallet for fraud prevention or compliance.
    /// </summary>
    public MobileWallet Block(string reason)
    {
        if (Status == StatusBlocked)
            throw new InvalidOperationException("Wallet is already blocked.");

        Status = StatusBlocked;
        QueueDomainEvent(new MobileWalletBlocked(Id, reason));
        return this;
    }

    /// <summary>
    /// Unblocks a blocked wallet.
    /// </summary>
    public MobileWallet Unblock()
    {
        if (Status != StatusBlocked)
            throw new InvalidOperationException("Only blocked wallets can be unblocked.");

        Status = StatusActive;
        QueueDomainEvent(new MobileWalletUnblocked(Id));
        return this;
    }

    /// <summary>
    /// Reactivates a suspended wallet.
    /// </summary>
    public MobileWallet Reactivate()
    {
        if (Status != StatusSuspended)
            throw new InvalidOperationException("Only suspended wallets can be reactivated.");

        Status = StatusActive;
        QueueDomainEvent(new MobileWalletReactivated(Id));
        return this;
    }

    /// <summary>
    /// Closes the wallet permanently.
    /// </summary>
    public MobileWallet Close(string? reason = null)
    {
        if (Status == StatusInactive)
            throw new InvalidOperationException("Wallet is already closed.");

        if (Balance > 0)
            throw new InvalidOperationException("Cannot close wallet with non-zero balance. Transfer funds first.");

        Status = StatusInactive;
        if (!string.IsNullOrWhiteSpace(reason))
        {
            Notes = string.IsNullOrWhiteSpace(Notes) ? $"Closed: {reason}" : $"{Notes}\nClosed: {reason}";
        }

        QueueDomainEvent(new MobileWalletClosed(Id, reason));
        return this;
    }
}
