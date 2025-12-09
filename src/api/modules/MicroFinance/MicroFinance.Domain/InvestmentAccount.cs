using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Represents a member's investment account/portfolio.
/// </summary>
/// <remarks>
/// Use cases:
/// - Track member investment portfolios across multiple products.
/// - Monitor total invested amount, current value, and gain/loss.
/// - Risk profiling for suitable investment product recommendations.
/// - Portfolio performance reporting and statements.
/// - Dividend/interest reinvestment tracking.
/// - Investment maturity alerts and rollover management.
/// 
/// Default values and constraints:
/// - AccountNumber: required unique identifier, max 32 characters (example: "INV-2025-001234")
/// - Status: Active by default (Active, Dormant, Closed, Frozen)
/// - RiskProfile: Moderate by default (Conservative, Moderate, Aggressive, Balanced)
/// - TotalInvested: sum of all investment contributions
/// - CurrentValue: mark-to-market portfolio value
/// 
/// Business rules:
/// - Account linked to member with KYC verification.
/// - Risk profile determines eligible investment products.
/// - Dormant after 12 months of no activity.
/// - Frozen accounts cannot transact until restrictions lifted.
/// - Portfolio gain/loss calculated as CurrentValue - TotalInvested.
/// </remarks>
/// <seealso cref="Member"/>
/// <seealso cref="InvestmentProduct"/>
/// <seealso cref="InvestmentTransaction"/>
/// <seealso cref="SavingsAccount"/>
public sealed class InvestmentAccount : AuditableEntity, IAggregateRoot
{
    // Constants
    public const int AccountNumberMaxLength = 32;
    public const int StatusMaxLength = 32;
    public const int ProfileMaxLength = 32;
    
    // Account Status
    public const string StatusActive = "Active";
    public const string StatusDormant = "Dormant";
    public const string StatusClosed = "Closed";
    public const string StatusFrozen = "Frozen";
    
    // Risk Profiles
    public const string ProfileConservative = "Conservative";
    public const string ProfileModerate = "Moderate";
    public const string ProfileAggressive = "Aggressive";
    public const string ProfileBalanced = "Balanced";

    public Guid MemberId { get; private set; }
    public string AccountNumber { get; private set; } = default!;
    public string Status { get; private set; } = StatusActive;
    public string RiskProfile { get; private set; } = ProfileModerate;
    public decimal TotalInvested { get; private set; }
    public decimal CurrentValue { get; private set; }
    public decimal TotalGainLoss { get; private set; }
    public decimal TotalGainLossPercent { get; private set; }
    public decimal RealizedGains { get; private set; }
    public decimal UnrealizedGains { get; private set; }
    public decimal TotalDividends { get; private set; }
    public int HoldingsCount { get; private set; }
    public DateOnly? FirstInvestmentDate { get; private set; }
    public DateOnly? LastTransactionDate { get; private set; }
    public Guid? AssignedAdvisorId { get; private set; }
    public bool HasSip { get; private set; }
    public decimal? SipAmount { get; private set; }
    public string? SipFrequency { get; private set; }
    public DateOnly? NextSipDate { get; private set; }
    public Guid? LinkedSavingsAccountId { get; private set; }
    public string? InvestmentGoal { get; private set; }
    public DateOnly? TargetDate { get; private set; }
    public decimal? TargetAmount { get; private set; }

    /// <summary>
    /// Gets the collection of transactions for this investment account.
    /// </summary>
    public ICollection<InvestmentTransaction> Transactions { get; private set; } = new List<InvestmentTransaction>();

    private InvestmentAccount() { }

    public static InvestmentAccount Create(
        Guid memberId,
        string accountNumber,
        string riskProfile,
        Guid? assignedAdvisorId = null,
        string? investmentGoal = null)
    {
        var account = new InvestmentAccount
        {
            MemberId = memberId,
            AccountNumber = accountNumber,
            RiskProfile = riskProfile,
            AssignedAdvisorId = assignedAdvisorId,
            InvestmentGoal = investmentGoal,
            Status = StatusActive,
            TotalInvested = 0,
            CurrentValue = 0,
            HoldingsCount = 0
        };

        account.QueueDomainEvent(new InvestmentAccountCreated(account));
        return account;
    }

    public InvestmentAccount Invest(decimal amount)
    {
        TotalInvested += amount;
        CurrentValue += amount;
        LastTransactionDate = DateOnly.FromDateTime(DateTime.UtcNow);
        FirstInvestmentDate ??= LastTransactionDate;
        UpdateGainLoss();
        QueueDomainEvent(new InvestmentMade(Id, amount));
        return this;
    }

    public InvestmentAccount Redeem(decimal amount, decimal gainLoss)
    {
        CurrentValue -= amount;
        TotalInvested -= (amount - gainLoss);
        RealizedGains += gainLoss;
        LastTransactionDate = DateOnly.FromDateTime(DateTime.UtcNow);
        UpdateGainLoss();
        QueueDomainEvent(new InvestmentRedeemed(Id, amount, gainLoss));
        return this;
    }

    public InvestmentAccount UpdateValuation(decimal newCurrentValue)
    {
        CurrentValue = newCurrentValue;
        UpdateGainLoss();
        QueueDomainEvent(new InvestmentAccountValuationUpdated(Id, newCurrentValue, TotalGainLossPercent));
        return this;
    }

    private void UpdateGainLoss()
    {
        TotalGainLoss = CurrentValue - TotalInvested;
        UnrealizedGains = TotalGainLoss - RealizedGains;
        TotalGainLossPercent = TotalInvested > 0 
            ? (TotalGainLoss / TotalInvested) * 100 
            : 0;
    }

    public InvestmentAccount RecordDividend(decimal amount)
    {
        TotalDividends += amount;
        return this;
    }

    public InvestmentAccount UpdateHoldingsCount(int count)
    {
        HoldingsCount = count;
        return this;
    }

    public InvestmentAccount SetupSip(decimal amount, string frequency, DateOnly nextDate, Guid linkedSavingsAccountId)
    {
        HasSip = true;
        SipAmount = amount;
        SipFrequency = frequency;
        NextSipDate = nextDate;
        LinkedSavingsAccountId = linkedSavingsAccountId;
        QueueDomainEvent(new SipSetup(Id, amount, frequency));
        return this;
    }

    public InvestmentAccount CancelSip()
    {
        HasSip = false;
        SipAmount = null;
        SipFrequency = null;
        NextSipDate = null;
        return this;
    }

    public InvestmentAccount SetTarget(decimal targetAmount, DateOnly targetDate)
    {
        TargetAmount = targetAmount;
        TargetDate = targetDate;
        return this;
    }

    public InvestmentAccount Close()
    {
        if (CurrentValue > 0)
            throw new InvalidOperationException("Cannot close account with holdings.");
        
        Status = StatusClosed;
        return this;
    }

    public InvestmentAccount Update(
        string? riskProfile = null,
        Guid? assignedAdvisorId = null,
        string? investmentGoal = null)
    {
        if (riskProfile is not null) RiskProfile = riskProfile;
        if (assignedAdvisorId.HasValue) AssignedAdvisorId = assignedAdvisorId.Value;
        if (investmentGoal is not null) InvestmentGoal = investmentGoal;

        QueueDomainEvent(new InvestmentAccountUpdated(this));
        return this;
    }
}
