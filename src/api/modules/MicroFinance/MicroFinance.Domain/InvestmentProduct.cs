using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Represents a wealth/investment product offered to members.
/// </summary>
/// <remarks>
/// Use cases:
/// - Define investment products (mutual funds, bonds, T-bills, money market).
/// - Configure minimum investment amounts and lock-in periods.
/// - Set risk levels for suitability matching with member profiles.
/// - Track historical performance and benchmark comparisons.
/// - Manage product activation, discontinuation, and limits.
/// - Calculate management fees and expense ratios.
/// 
/// Default values and constraints:
/// - Code: required unique identifier, max 32 characters (example: "INV-BOND-001")
/// - Type: MutualFund, UnitTrust, Bond, TreasuryBill, MoneyMarket, Equity, Balanced
/// - Status: Active by default (Active, Inactive, Discontinued)
/// - RiskLevel: Low, Medium, High
/// - MinimumInvestment: minimum initial investment amount
/// - LockInPeriod: minimum holding period in months
/// 
/// Business rules:
/// - Product code must be unique.
/// - Risk level must match investor risk profile for suitability.
/// - Lock-in period enforced for early redemption penalties.
/// - Discontinued products cannot accept new investments.
/// - NAV calculated daily for market-linked products.
/// </remarks>
/// <seealso cref="InvestmentAccount"/>
/// <seealso cref="InvestmentTransaction"/>
/// <seealso cref="Member"/>
public sealed class InvestmentProduct : AuditableEntity, IAggregateRoot
{
    // Constants
    public const int CodeMaxLength = 32;
    public const int TypeMaxLength = 64;
    public const int StatusMaxLength = 32;
    public const int RiskLevelMaxLength = 32;
    
    // Product Status
    public const string StatusActive = "Active";
    public const string StatusInactive = "Inactive";
    public const string StatusDiscontinued = "Discontinued";
    
    // Product Types
    public const string TypeMutualFund = "MutualFund";
    public const string TypeUnitTrust = "UnitTrust";
    public const string TypeBond = "Bond";
    public const string TypeTreasuryBill = "TreasuryBill";
    public const string TypeMoneyMarket = "MoneyMarket";
    public const string TypeEquity = "Equity";
    public const string TypeBalanced = "Balanced";
    
    // Risk Levels
    public const string RiskLow = "Low";
    public const string RiskMedium = "Medium";
    public const string RiskHigh = "High";

    public string Code { get; private set; } = default!;
    public string ProductType { get; private set; } = default!;
    public string Status { get; private set; } = StatusActive;
    public string RiskLevel { get; private set; } = RiskMedium;
    public decimal MinimumInvestment { get; private set; }
    public decimal? MaximumInvestment { get; private set; }
    public decimal ManagementFeePercent { get; private set; }
    public decimal? PerformanceFeePercent { get; private set; }
    public decimal? EntryLoadPercent { get; private set; }
    public decimal? ExitLoadPercent { get; private set; }
    public decimal ExpectedReturnMin { get; private set; }
    public decimal ExpectedReturnMax { get; private set; }
    public int LockInPeriodDays { get; private set; }
    public int? MinimumHoldingDays { get; private set; }
    public decimal CurrentNav { get; private set; }
    public DateOnly? NavDate { get; private set; }
    public decimal TotalAum { get; private set; }
    public int TotalInvestors { get; private set; }
    public string? FundManager { get; private set; }
    public string? Benchmark { get; private set; }
    public decimal? YtdReturn { get; private set; }
    public decimal? OneYearReturn { get; private set; }
    public decimal? ThreeYearReturn { get; private set; }
    public bool AllowPartialRedemption { get; private set; }
    public bool AllowSip { get; private set; }
    public int DisplayOrder { get; private set; }

    private InvestmentProduct() { }

    public static InvestmentProduct Create(
        string name,
        string code,
        string productType,
        string riskLevel,
        decimal minimumInvestment,
        decimal managementFeePercent,
        decimal expectedReturnMin,
        decimal expectedReturnMax,
        int lockInPeriodDays = 0,
        string? description = null)
    {
        var product = new InvestmentProduct
        {
            Code = code,
            ProductType = productType,
            RiskLevel = riskLevel,
            MinimumInvestment = minimumInvestment,
            ManagementFeePercent = managementFeePercent,
            ExpectedReturnMin = expectedReturnMin,
            ExpectedReturnMax = expectedReturnMax,
            LockInPeriodDays = lockInPeriodDays,
            Status = StatusActive,
            CurrentNav = 100, // Starting NAV
            TotalAum = 0,
            TotalInvestors = 0
        };
        product.Name = name;
        product.Description = description;

        product.QueueDomainEvent(new InvestmentProductCreated(product));
        return product;
    }

    public InvestmentProduct UpdateNav(decimal newNav, DateOnly navDate)
    {
        CurrentNav = newNav;
        NavDate = navDate;
        QueueDomainEvent(new InvestmentProductNavUpdated(Id, newNav, navDate));
        return this;
    }

    public InvestmentProduct UpdatePerformance(
        decimal? ytdReturn = null,
        decimal? oneYearReturn = null,
        decimal? threeYearReturn = null)
    {
        if (ytdReturn.HasValue) YtdReturn = ytdReturn.Value;
        if (oneYearReturn.HasValue) OneYearReturn = oneYearReturn.Value;
        if (threeYearReturn.HasValue) ThreeYearReturn = threeYearReturn.Value;
        return this;
    }

    public InvestmentProduct AddInvestor(decimal investmentAmount)
    {
        TotalInvestors++;
        TotalAum += investmentAmount;
        return this;
    }

    public InvestmentProduct RemoveInvestor(decimal redemptionAmount)
    {
        TotalInvestors--;
        TotalAum -= redemptionAmount;
        return this;
    }

    public InvestmentProduct Activate()
    {
        Status = StatusActive;
        return this;
    }

    public InvestmentProduct Deactivate()
    {
        Status = StatusInactive;
        return this;
    }

    public InvestmentProduct Discontinue()
    {
        Status = StatusDiscontinued;
        return this;
    }

    public InvestmentProduct Update(
        string? name = null,
        string? description = null,
        decimal? minimumInvestment = null,
        decimal? maximumInvestment = null,
        decimal? managementFeePercent = null,
        decimal? performanceFeePercent = null,
        decimal? entryLoadPercent = null,
        decimal? exitLoadPercent = null,
        int? minimumHoldingDays = null,
        string? fundManager = null,
        string? benchmark = null,
        bool? allowPartialRedemption = null,
        bool? allowSip = null,
        int? displayOrder = null)
    {
        if (name is not null) this.Name = name;
        if (description is not null) this.Description = description;
        if (minimumInvestment.HasValue) MinimumInvestment = minimumInvestment.Value;
        if (maximumInvestment.HasValue) MaximumInvestment = maximumInvestment.Value;
        if (managementFeePercent.HasValue) ManagementFeePercent = managementFeePercent.Value;
        if (performanceFeePercent.HasValue) PerformanceFeePercent = performanceFeePercent.Value;
        if (entryLoadPercent.HasValue) EntryLoadPercent = entryLoadPercent.Value;
        if (exitLoadPercent.HasValue) ExitLoadPercent = exitLoadPercent.Value;
        if (minimumHoldingDays.HasValue) MinimumHoldingDays = minimumHoldingDays.Value;
        if (fundManager is not null) FundManager = fundManager;
        if (benchmark is not null) Benchmark = benchmark;
        if (allowPartialRedemption.HasValue) AllowPartialRedemption = allowPartialRedemption.Value;
        if (allowSip.HasValue) AllowSip = allowSip.Value;
        if (displayOrder.HasValue) DisplayOrder = displayOrder.Value;

        QueueDomainEvent(new InvestmentProductUpdated(this));
        return this;
    }
}
