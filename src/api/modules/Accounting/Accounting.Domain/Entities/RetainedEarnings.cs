using Accounting.Domain.Events.RetainedEarnings;

namespace Accounting.Domain.Entities;

/// <summary>
/// Represents retained earnings for a fiscal year with tracking of opening balances, net income, distributions, and equity changes.
/// </summary>
/// <remarks>
/// Use cases:
/// - Track annual retained earnings movements for equity reporting.
/// - Support statement of changes in equity preparation.
/// - Manage dividend and patronage distribution allocation.
/// - Track capital contributions and withdrawals for cooperatives.
/// - Enable year-end closing and retained earnings appropriation.
/// - Support regulatory reporting for equity structure and capital adequacy.
/// - Track appropriated vs unappropriated retained earnings.
/// - Enable multi-year retained earnings trend analysis.
/// 
/// Default values:
/// - FiscalYear: required year (example: 2025)
/// - OpeningBalance: carried forward from prior year closing
/// - NetIncome: 0.00 (updated from income statement)
/// - Distributions: 0.00 (dividends, patronage, or other distributions)
/// - CapitalContributions: 0.00 (member or shareholder contributions)
/// - OtherEquityChanges: 0.00 (adjustments, restatements)
/// - ClosingBalance: calculated as Opening + Net Income - Distributions + Contributions + Other
/// - Status: "Open" (active year), "Closed" (finalized)
/// - IsClosed: false (year is still open)
/// - ClosedDate: null (set when year closed)
/// 
/// Business rules:
/// - FiscalYear must be unique (one record per year)
/// - FiscalYear must be reasonable (1900-2100)
/// - Opening balance typically equals prior year closing balance
/// - Closing balance = Opening + NetIncome - Distributions + Contributions + Other
/// - Cannot modify closed years without proper authorization
/// - Net income transferred from income statement at year-end
/// - Distributions cannot exceed available retained earnings
/// - Status transitions: Open → Closed
/// - Year-end closing creates next year's opening balance
/// </remarks>
/// <seealso cref="Accounting.Domain.Events.RetainedEarnings.RetainedEarningsCreated"/>
/// <seealso cref="Accounting.Domain.Events.RetainedEarnings.RetainedEarningsUpdated"/>
/// <seealso cref="Accounting.Domain.Events.RetainedEarnings.RetainedEarningsDistributionRecorded"/>
/// <seealso cref="Accounting.Domain.Events.RetainedEarnings.RetainedEarningsCapitalContributionRecorded"/>
/// <seealso cref="Accounting.Domain.Events.RetainedEarnings.RetainedEarningsClosed"/>
public class RetainedEarnings : AuditableEntity, IAggregateRoot
{
    private const int MaxStatusLength = 32;
    private const int MaxDescriptionLength = 2048;
    private const int MaxNotesLength = 2048;
    private const int MinFiscalYear = 1900;
    private const int MaxFiscalYear = 2100;

    /// <summary>
    /// Fiscal year for this retained earnings record.
    /// Example: 2025 for fiscal year 2025. Must be between 1900 and 2100.
    /// One record per fiscal year. Used as natural key.
    /// </summary>
    public int FiscalYear { get; private set; }

    /// <summary>
    /// Opening retained earnings balance at start of fiscal year.
    /// Example: 500000.00 carried forward from prior year.
    /// Typically equals prior year's closing balance.
    /// </summary>
    public decimal OpeningBalance { get; private set; }

    /// <summary>
    /// Net income or loss for the fiscal year.
    /// Example: 125000.00 for net income, -25000.00 for net loss.
    /// Transferred from income statement at year-end.
    /// Positive = profit, Negative = loss.
    /// </summary>
    public decimal NetIncome { get; private set; }

    /// <summary>
    /// Total distributions to members or shareholders during the year.
    /// Example: 75000.00 for dividends or patronage distributions.
    /// Includes cash dividends, patronage refunds, capital retirements.
    /// Reduces retained earnings.
    /// </summary>
    public decimal Distributions { get; private set; }

    /// <summary>
    /// Capital contributions from members or shareholders.
    /// Example: 50000.00 for new member capital or equity injections.
    /// Increases equity. May include paid-in capital.
    /// </summary>
    public decimal CapitalContributions { get; private set; }

    /// <summary>
    /// Other adjustments to retained earnings.
    /// Example: 10000.00 for prior period adjustments or restatements.
    /// Can be positive (increase) or negative (decrease).
    /// Includes: prior period corrections, accounting policy changes, restatements.
    /// </summary>
    public decimal OtherEquityChanges { get; private set; }

    /// <summary>
    /// Closing retained earnings balance at end of fiscal year.
    /// Example: 600000.00 after all changes applied.
    /// Calculated: Opening + NetIncome - Distributions + CapitalContributions + OtherEquityChanges.
    /// Becomes next year's opening balance.
    /// </summary>
    public decimal ClosingBalance { get; private set; }

    /// <summary>
    /// Current status of the retained earnings period.
    /// Values: "Open", "Closed", "Locked".
    /// Default: "Open". Max length: 32.
    /// </summary>
    public string Status { get; private set; } = string.Empty;

    /// <summary>
    /// Whether the fiscal year is closed.
    /// Default: false. True after year-end closing process.
    /// Closed years cannot be modified without authorization.
    /// </summary>
    public bool IsClosed { get; private set; }

    /// <summary>
    /// Date when the fiscal year was closed.
    /// Example: 2026-01-31 after closing FY2025. Null if still open.
    /// </summary>
    public DateTime? ClosedDate { get; private set; }

    /// <summary>
    /// Person who closed the fiscal year.
    /// Example: "john.doe@company.com", "CFO". Max length: 256.
    /// </summary>
    public string? ClosedBy { get; private set; }

    /// <summary>
    /// Optional appropriated amount for specific purposes.
    /// Example: 100000.00 reserved for capital projects.
    /// Portion of retained earnings restricted for designated use.
    /// </summary>
    public decimal ApproprietedAmount { get; private set; }

    /// <summary>
    /// Unappropriated retained earnings available for distribution.
    /// Example: 500000.00 available for dividends or reinvestment.
    /// Calculated: ClosingBalance - ApproprietedAmount.
    /// </summary>
    public decimal UnappropriatedAmount { get; private set; }

    /// <summary>
    /// Start date of the fiscal year.
    /// Example: 2025-01-01 for calendar year. Used for date range validation.
    /// </summary>
    public DateTime FiscalYearStartDate { get; private set; }

    /// <summary>
    /// End date of the fiscal year.
    /// Example: 2025-12-31 for calendar year. Used for date range validation.
    /// </summary>
    public DateTime FiscalYearEndDate { get; private set; }

    /// <summary>
    /// Optional retained earnings GL account reference.
    /// Links to ChartOfAccount entity (typically equity account 300-series).
    /// </summary>
    public DefaultIdType? RetainedEarningsAccountId { get; private set; }

    /// <summary>
    /// Number of distributions made during the year.
    /// Example: 4 for quarterly dividends. Used for tracking and reporting.
    /// </summary>
    public int DistributionCount { get; private set; }

    /// <summary>
    /// Date of the last distribution.
    /// Example: 2025-12-15. Used for tracking distribution frequency.
    /// </summary>
    public DateTime? LastDistributionDate { get; private set; }

    // Parameterless constructor for EF Core
    private RetainedEarnings()
    {
        Status = "Open";
    }

    private RetainedEarnings(int fiscalYear, decimal openingBalance, DateTime fiscalYearStartDate,
        DateTime fiscalYearEndDate, DefaultIdType? retainedEarningsAccountId = null,
        string? description = null, string? notes = null)
    {
        // Validations
        if (fiscalYear < MinFiscalYear || fiscalYear > MaxFiscalYear)
            throw new ArgumentException($"Fiscal year must be between {MinFiscalYear} and {MaxFiscalYear}", nameof(fiscalYear));

        if (fiscalYearEndDate <= fiscalYearStartDate)
            throw new ArgumentException("Fiscal year end date must be after start date", nameof(fiscalYearEndDate));

        FiscalYear = fiscalYear;
        Name = $"FY{fiscalYear}"; // For AuditableEntity compatibility
        OpeningBalance = openingBalance;
        NetIncome = 0m;
        Distributions = 0m;
        CapitalContributions = 0m;
        OtherEquityChanges = 0m;
        ClosingBalance = openingBalance; // Initially equals opening
        Status = "Open";
        IsClosed = false;
        ApproprietedAmount = 0m;
        UnappropriatedAmount = openingBalance;
        FiscalYearStartDate = fiscalYearStartDate;
        FiscalYearEndDate = fiscalYearEndDate;
        RetainedEarningsAccountId = retainedEarningsAccountId;
        DistributionCount = 0;
        Description = description?.Trim();
        Notes = notes?.Trim();

        QueueDomainEvent(new RetainedEarningsCreated(Id, FiscalYear, OpeningBalance, FiscalYearStartDate, FiscalYearEndDate, Description, Notes));
    }

    /// <summary>
    /// Factory method to create a new retained earnings record with validation.
    /// </summary>
    public static RetainedEarnings Create(int fiscalYear, decimal openingBalance,
        DateTime fiscalYearStartDate, DateTime fiscalYearEndDate,
        DefaultIdType? retainedEarningsAccountId = null,
        string? description = null, string? notes = null)
    {
        return new RetainedEarnings(fiscalYear, openingBalance, fiscalYearStartDate,
            fiscalYearEndDate, retainedEarningsAccountId, description, notes);
    }

    /// <summary>
    /// Update net income from income statement.
    /// </summary>
    public RetainedEarnings UpdateNetIncome(decimal netIncome)
    {
        if (IsClosed)
            throw new InvalidOperationException("Cannot modify closed retained earnings period");

        NetIncome = netIncome;
        RecalculateClosingBalance();

        QueueDomainEvent(new RetainedEarningsNetIncomeUpdated(Id, FiscalYear, NetIncome, ClosingBalance));
        return this;
    }

    /// <summary>
    /// Record a distribution to members or shareholders.
    /// </summary>
    public RetainedEarnings RecordDistribution(decimal amount, DateTime distributionDate, string distributionType)
    {
        if (IsClosed)
            throw new InvalidOperationException("Cannot modify closed retained earnings period");

        if (amount <= 0)
            throw new ArgumentException("Distribution amount must be positive", nameof(amount));

        if (string.IsNullOrWhiteSpace(distributionType))
            throw new ArgumentException("Distribution type is required", nameof(distributionType));

        Distributions += amount;
        DistributionCount++;
        LastDistributionDate = distributionDate;
        RecalculateClosingBalance();

        QueueDomainEvent(new RetainedEarningsDistributionRecorded(Id, FiscalYear, amount, distributionType, distributionDate, ClosingBalance));
        return this;
    }

    /// <summary>
    /// Record a capital contribution.
    /// </summary>
    public RetainedEarnings RecordCapitalContribution(decimal amount, DateTime contributionDate, string contributionType)
    {
        if (IsClosed)
            throw new InvalidOperationException("Cannot modify closed retained earnings period");

        if (amount <= 0)
            throw new ArgumentException("Capital contribution amount must be positive", nameof(amount));

        if (string.IsNullOrWhiteSpace(contributionType))
            throw new ArgumentException("Contribution type is required", nameof(contributionType));

        CapitalContributions += amount;
        RecalculateClosingBalance();

        QueueDomainEvent(new RetainedEarningsCapitalContributionRecorded(Id, FiscalYear, amount, contributionType, contributionDate, ClosingBalance));
        return this;
    }

    /// <summary>
    /// Record other equity changes (adjustments, restatements).
    /// </summary>
    public RetainedEarnings RecordEquityChange(decimal amount, string changeType, string reason)
    {
        if (IsClosed)
            throw new InvalidOperationException("Cannot modify closed retained earnings period");

        if (string.IsNullOrWhiteSpace(changeType))
            throw new ArgumentException("Change type is required", nameof(changeType));

        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("Reason is required for equity changes", nameof(reason));

        OtherEquityChanges += amount;
        RecalculateClosingBalance();

        QueueDomainEvent(new RetainedEarningsEquityChangeRecorded(Id, FiscalYear, amount, changeType, reason, ClosingBalance));
        return this;
    }

    /// <summary>
    /// Appropriate a portion of retained earnings.
    /// </summary>
    public RetainedEarnings AppropriateRetainedEarnings(decimal amount, string purpose)
    {
        if (IsClosed)
            throw new InvalidOperationException("Cannot modify closed retained earnings period");

        if (amount <= 0)
            throw new ArgumentException("Appropriation amount must be positive", nameof(amount));

        if (string.IsNullOrWhiteSpace(purpose))
            throw new ArgumentException("Appropriation purpose is required", nameof(purpose));

        if (amount > UnappropriatedAmount)
            throw new InvalidOperationException($"Appropriation amount {amount:N2} exceeds unappropriated balance {UnappropriatedAmount:N2}");

        ApproprietedAmount += amount;
        UnappropriatedAmount = ClosingBalance - ApproprietedAmount;

        QueueDomainEvent(new RetainedEarningsAppropriated(Id, FiscalYear, amount, purpose, ApproprietedAmount));
        return this;
    }

    /// <summary>
    /// Release appropriated retained earnings back to unappropriated.
    /// </summary>
    public RetainedEarnings ReleaseAppropriation(decimal amount, string reason)
    {
        if (IsClosed)
            throw new InvalidOperationException("Cannot modify closed retained earnings period");

        if (amount <= 0)
            throw new ArgumentException("Release amount must be positive", nameof(amount));

        if (amount > ApproprietedAmount)
            throw new InvalidOperationException($"Release amount {amount:N2} exceeds appropriated balance {ApproprietedAmount:N2}");

        ApproprietedAmount -= amount;
        UnappropriatedAmount = ClosingBalance - ApproprietedAmount;

        QueueDomainEvent(new RetainedEarningsAppropriationReleased(Id, FiscalYear, amount, reason, ApproprietedAmount));
        return this;
    }

    /// <summary>
    /// Close the fiscal year retained earnings.
    /// </summary>
    public RetainedEarnings Close(string closedBy)
    {
        if (string.IsNullOrWhiteSpace(closedBy))
            throw new ArgumentException("Closed by information is required", nameof(closedBy));

        if (IsClosed)
            throw new InvalidOperationException("Retained earnings period is already closed");

        IsClosed = true;
        ClosedDate = DateTime.UtcNow;
        ClosedBy = closedBy.Trim();
        Status = "Closed";

        QueueDomainEvent(new RetainedEarningsClosed(Id, FiscalYear, ClosingBalance, ClosedBy, ClosedDate.Value));
        return this;
    }

    /// <summary>
    /// Reopen a closed fiscal year (requires authorization).
    /// </summary>
    public RetainedEarnings Reopen(string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("Reason is required to reopen", nameof(reason));

        if (!IsClosed)
            throw new InvalidOperationException("Retained earnings period is not closed");

        IsClosed = false;
        Status = "Open";
        Notes = $"{Notes}\n\nReopened: {reason.Trim()}".Trim();

        QueueDomainEvent(new RetainedEarningsReopened(Id, FiscalYear, reason));
        return this;
    }

    /// <summary>
    /// Recalculate closing balance based on all components.
    /// </summary>
    private void RecalculateClosingBalance()
    {
        ClosingBalance = OpeningBalance + NetIncome - Distributions + CapitalContributions + OtherEquityChanges;
        UnappropriatedAmount = ClosingBalance - ApproprietedAmount;
    }

    /// <summary>
    /// Return on equity percentage.
    /// </summary>
    public decimal ReturnOnEquity => OpeningBalance > 0 ? (NetIncome / OpeningBalance) * 100 : 0;

    /// <summary>
    /// Distribution payout ratio.
    /// </summary>
    public decimal PayoutRatio => NetIncome > 0 ? (Distributions / NetIncome) * 100 : 0;
}

