using Accounting.Domain.Events.TrialBalance;

namespace Accounting.Domain.Entities;

/// <summary>
/// Represents a trial balance report for a specific accounting period with balance verification, out-of-balance detection, and financial statement preparation support.
/// </summary>
/// <remarks>
/// Use cases:
/// - Generate period-end trial balance for financial statement preparation.
/// - Verify accounting equation: Assets = Liabilities + Equity.
/// - Detect out-of-balance conditions requiring correction.
/// - Support audit trail and financial close process.
/// - Enable drill-down to account details for investigation.
/// - Track adjusting entries and their impact on trial balance.
/// - Generate comparative trial balances (period-over-period).
/// - Support consolidation of multiple entities.
/// 
/// Default values:
/// - TrialBalanceNumber: required unique identifier (example: "TB-2025-10")
/// - PeriodId: required accounting period reference
/// - GeneratedDate: current date/time
/// - TotalDebits: 0.00 (sum of all debit balances)
/// - TotalCredits: 0.00 (sum of all credit balances)
/// - IsBalanced: false (true when TotalDebits = TotalCredits)
/// - OutOfBalanceAmount: 0.00 (difference between debits and credits)
/// - Status: "Draft" (new trial balances start as draft)
/// 
/// Business rules:
/// - TotalDebits must equal TotalCredits for balanced trial balance
/// - Out-of-balance amount = |TotalDebits - TotalCredits|
/// - Cannot finalize unbalanced trial balance
/// - All account balances must reconcile to GL
/// - Zero-balance accounts can be excluded from report
/// - Adjusting entries require rebalancing
/// </remarks>
public class TrialBalance : AuditableEntity, IAggregateRoot
{
    public string TrialBalanceNumber { get; private set; } = string.Empty;
    public DefaultIdType PeriodId { get; private set; }
    public DateTime GeneratedDate { get; private set; }
    public DateTime PeriodStartDate { get; private set; }
    public DateTime PeriodEndDate { get; private set; }
    public decimal TotalDebits { get; private set; }
    public decimal TotalCredits { get; private set; }
    public decimal TotalAssets { get; private set; }
    public decimal TotalLiabilities { get; private set; }
    public decimal TotalEquity { get; private set; }
    public decimal TotalRevenue { get; private set; }
    public decimal TotalExpenses { get; private set; }
    public bool IsBalanced { get; private set; }
    public decimal OutOfBalanceAmount { get; private set; }
    public string Status { get; private set; } = string.Empty;
    public bool IncludeZeroBalances { get; private set; }
    public int AccountCount { get; private set; }
    public DateTime? FinalizedDate { get; private set; }
    public string? FinalizedBy { get; private set; }

    private readonly List<TrialBalanceLineItem> _lineItems = new();
    public IReadOnlyCollection<TrialBalanceLineItem> LineItems => _lineItems.AsReadOnly();

    private TrialBalance() { Status = "Draft"; }

    private TrialBalance(string trialBalanceNumber, DefaultIdType periodId,
        DateTime periodStartDate, DateTime periodEndDate, bool includeZeroBalances = false,
        string? description = null, string? notes = null)
    {
        if (string.IsNullOrWhiteSpace(trialBalanceNumber))
            throw new ArgumentException("Trial balance number is required", nameof(trialBalanceNumber));

        TrialBalanceNumber = trialBalanceNumber.Trim();
        Name = trialBalanceNumber.Trim();
        PeriodId = periodId;
        GeneratedDate = DateTime.UtcNow;
        PeriodStartDate = periodStartDate;
        PeriodEndDate = periodEndDate;
        TotalDebits = 0m;
        TotalCredits = 0m;
        IsBalanced = true;
        OutOfBalanceAmount = 0m;
        Status = "Draft";
        IncludeZeroBalances = includeZeroBalances;
        Description = description?.Trim();
        Notes = notes?.Trim();

        QueueDomainEvent(new TrialBalanceCreated(Id, TrialBalanceNumber, PeriodId, PeriodStartDate, PeriodEndDate));
    }

    public static TrialBalance Create(string trialBalanceNumber, DefaultIdType periodId,
        DateTime periodStartDate, DateTime periodEndDate, bool includeZeroBalances = false,
        string? description = null, string? notes = null)
    {
        return new TrialBalance(trialBalanceNumber, periodId, periodStartDate, periodEndDate,
            includeZeroBalances, description, notes);
    }

    public TrialBalance AddLineItem(string accountCode, string accountName, string accountType,
        decimal debitBalance, decimal creditBalance)
    {
        if (Status == "Finalized")
            throw new InvalidOperationException("Cannot modify finalized trial balance");

        if (!IncludeZeroBalances && debitBalance == 0 && creditBalance == 0)
            return this;

        var lineItem = TrialBalanceLineItem.Create(accountCode, accountName, accountType,
            debitBalance, creditBalance);
        _lineItems.Add(lineItem);

        RecalculateTotals();
        return this;
    }

    private void RecalculateTotals()
    {
        TotalDebits = _lineItems.Sum(l => l.DebitBalance);
        TotalCredits = _lineItems.Sum(l => l.CreditBalance);
        OutOfBalanceAmount = Math.Abs(TotalDebits - TotalCredits);
        IsBalanced = OutOfBalanceAmount < 0.01m; // Allow penny rounding

        // Calculate account type totals
        TotalAssets = _lineItems.Where(l => l.AccountType == "Asset").Sum(l => l.DebitBalance - l.CreditBalance);
        TotalLiabilities = _lineItems.Where(l => l.AccountType == "Liability").Sum(l => l.CreditBalance - l.DebitBalance);
        TotalEquity = _lineItems.Where(l => l.AccountType == "Equity").Sum(l => l.CreditBalance - l.DebitBalance);
        TotalRevenue = _lineItems.Where(l => l.AccountType == "Revenue").Sum(l => l.CreditBalance);
        TotalExpenses = _lineItems.Where(l => l.AccountType == "Expense").Sum(l => l.DebitBalance);
        AccountCount = _lineItems.Count;

        QueueDomainEvent(new TrialBalanceRecalculated(Id, TrialBalanceNumber, TotalDebits, TotalCredits, IsBalanced));
    }

    public TrialBalance Finalize(string finalizedBy)
    {
        if (string.IsNullOrWhiteSpace(finalizedBy))
            throw new ArgumentException("Finalizer information is required", nameof(finalizedBy));

        if (Status == "Finalized")
            throw new InvalidOperationException("Trial balance is already finalized");

        if (!IsBalanced)
            throw new InvalidOperationException($"Cannot finalize unbalanced trial balance (out of balance by {OutOfBalanceAmount:N2})");

        // Verify accounting equation
        var accountingEquationBalance = TotalAssets - (TotalLiabilities + TotalEquity);
        if (Math.Abs(accountingEquationBalance) > 0.01m)
            throw new InvalidOperationException($"Accounting equation does not balance: Assets ({TotalAssets:N2}) ≠ Liabilities ({TotalLiabilities:N2}) + Equity ({TotalEquity:N2})");

        Status = "Finalized";
        FinalizedDate = DateTime.UtcNow;
        FinalizedBy = finalizedBy.Trim();

        QueueDomainEvent(new TrialBalanceFinalized(Id, TrialBalanceNumber, FinalizedBy, FinalizedDate.Value, TotalDebits, TotalCredits));
        return this;
    }

    public TrialBalance Reopen(string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("Reason is required to reopen", nameof(reason));

        if (Status != "Finalized")
            throw new InvalidOperationException("Can only reopen finalized trial balance");

        Status = "Draft";
        Notes = $"{Notes}\n\nReopened: {reason.Trim()}".Trim();

        QueueDomainEvent(new TrialBalanceReopened(Id, TrialBalanceNumber, reason));
        return this;
    }

    public decimal NetIncome => TotalRevenue - TotalExpenses;

    public bool AccountingEquationBalances
    {
        get
        {
            var difference = TotalAssets - (TotalLiabilities + TotalEquity);
            return Math.Abs(difference) < 0.01m;
        }
    }
}

public class TrialBalanceLineItem
{
    public string AccountCode { get; private set; } = string.Empty;
    public string AccountName { get; private set; } = string.Empty;
    public string AccountType { get; private set; } = string.Empty;
    public decimal DebitBalance { get; private set; }
    public decimal CreditBalance { get; private set; }
    public decimal NetBalance => DebitBalance - CreditBalance;

    private TrialBalanceLineItem() { }

    private TrialBalanceLineItem(string accountCode, string accountName, string accountType,
        decimal debitBalance, decimal creditBalance)
    {
        if (string.IsNullOrWhiteSpace(accountCode))
            throw new ArgumentException("Account code is required", nameof(accountCode));
        if (debitBalance < 0 || creditBalance < 0)
            throw new ArgumentException("Balances cannot be negative");

        AccountCode = accountCode.Trim();
        AccountName = accountName.Trim();
        AccountType = accountType.Trim();
        DebitBalance = debitBalance;
        CreditBalance = creditBalance;
    }

    public static TrialBalanceLineItem Create(string accountCode, string accountName, string accountType,
        decimal debitBalance, decimal creditBalance)
    {
        return new TrialBalanceLineItem(accountCode, accountName, accountType, debitBalance, creditBalance);
    }
}

