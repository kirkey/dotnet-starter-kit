using Accounting.Domain.Events.AccountsReceivableAccount;

namespace Accounting.Domain.Entities;

/// <summary>
/// Represents an accounts receivable control account for tracking customer balances, aging, and collection activities with subsidiary ledger reconciliation.
/// </summary>
/// <remarks>
/// Use cases:
/// - Track aggregate accounts receivable balances by customer type or segment.
/// - Support aging analysis for collection management and bad debt estimation.
/// - Enable AR reconciliation with subsidiary customer ledgers.
/// - Calculate allowance for doubtful accounts based on aging buckets.
/// - Monitor collection effectiveness and days sales outstanding (DSO).
/// - Support write-off authorization and bad debt expense recognition.
/// - Track customer payment patterns and credit risk indicators.
/// - Generate AR aging reports for management and audit purposes.
/// 
/// Default values:
/// - AccountNumber: required unique identifier (example: "AR-001", "AR-UTILITY")
/// - AccountName: required descriptive name (example: "Accounts Receivable - Utility Customers")
/// - CurrentBalance: 0.00 (total outstanding receivables)
/// - Current0to30: 0.00 (aging bucket 0-30 days)
/// - Days31to60: 0.00 (aging bucket 31-60 days)
/// - Days61to90: 0.00 (aging bucket 61-90 days)
/// - Over90Days: 0.00 (aging bucket over 90 days)
/// - AllowanceForDoubtfulAccounts: 0.00 (estimated uncollectible amount)
/// - IsActive: true (account is active and operational)
/// 
/// Business rules:
/// - AccountNumber must be unique within AR accounts
/// - CurrentBalance = sum of all aging buckets
/// - Aging buckets must be non-negative
/// - Allowance cannot exceed current balance
/// - Bad debt percentage calculated from historical write-offs
/// - DSO calculated as (AR Balance / Daily Sales)
/// - Reconciliation with subsidiary ledger required monthly
/// - Write-offs require proper authorization
/// </remarks>
/// <seealso cref="ArAccountCreated"/>
/// <seealso cref="ArAccountBalanceUpdated"/>
/// <seealso cref="Accounting.Domain.Events.AccountsReceivableAccount.ARAccountAgingUpdated"/>
/// <seealso cref="ArAccountReconciled"/>
public class AccountsReceivableAccount : AuditableEntity, IAggregateRoot
{
    private const int MaxAccountNumberLength = 50;
    private const int MaxAccountNameLength = 256;
    private const int MaxDescriptionLength = 2048;
    private const int MaxNotesLength = 2048;

    /// <summary>
    /// Unique AR account identifier.
    /// Example: "AR-001", "AR-UTILITY", "AR-COMMERCIAL". Max length: 50.
    /// </summary>
    public string AccountNumber { get; private set; } = string.Empty;

    /// <summary>
    /// Descriptive name of the AR account.
    /// Example: "Accounts Receivable - Utility Customers", "AR - Commercial Sales". Max length: 256.
    /// </summary>
    public string AccountName { get; private set; } = string.Empty;

    /// <summary>
    /// Current total outstanding accounts receivable balance.
    /// Example: 1500000.00 for $1.5M total AR. Default: 0.00.
    /// Updated with each invoice and payment. Equals sum of aging buckets.
    /// </summary>
    public decimal CurrentBalance { get; private set; }

    /// <summary>
    /// Receivables 0-30 days old (current).
    /// Example: 800000.00. Default: 0.00. Most current and likely collectible.
    /// </summary>
    public decimal Current0To30 { get; private set; }

    /// <summary>
    /// Receivables 31-60 days old.
    /// Example: 400000.00. Default: 0.00. Slightly past due but generally collectible.
    /// </summary>
    public decimal Days31To60 { get; private set; }

    /// <summary>
    /// Receivables 61-90 days old.
    /// Example: 200000.00. Default: 0.00. Past due, requires collection effort.
    /// </summary>
    public decimal Days61To90 { get; private set; }

    /// <summary>
    /// Receivables over 90 days old.
    /// Example: 100000.00. Default: 0.00. Significantly past due, higher risk.
    /// </summary>
    public decimal Over90Days { get; private set; }

    /// <summary>
    /// Estimated allowance for uncollectible accounts.
    /// Example: 75000.00 for 5% of AR. Default: 0.00.
    /// Calculated based on historical write-off rates and aging analysis.
    /// </summary>
    public decimal AllowanceForDoubtfulAccounts { get; private set; }

    /// <summary>
    /// Net realizable AR value (Current Balance - Allowance).
    /// Example: 1425000.00. Represents expected collectible amount.
    /// </summary>
    public decimal NetReceivables { get; private set; }

    /// <summary>
    /// Number of customer accounts with outstanding balances.
    /// Example: 5234. Updated with each transaction.
    /// </summary>
    public int CustomerCount { get; private set; }

    /// <summary>
    /// Average days sales outstanding (DSO).
    /// Example: 35.5 days. Calculated as (AR Balance / Average Daily Sales).
    /// Industry benchmark: 30-45 days for utilities.
    /// </summary>
    public decimal DaysSalesOutstanding { get; private set; }

    /// <summary>
    /// Historical bad debt percentage (write-offs / total AR).
    /// Example: 0.025 for 2.5% bad debt rate. Used for allowance estimation.
    /// </summary>
    public decimal BadDebtPercentage { get; private set; }

    /// <summary>
    /// Date of last reconciliation with subsidiary ledger.
    /// Example: 2025-10-31. Should be monthly at minimum.
    /// </summary>
    public DateTime? LastReconciliationDate { get; private set; }

    /// <summary>
    /// Whether the AR account is reconciled with subsidiary ledgers.
    /// Default: false. True after successful reconciliation.
    /// </summary>
    public bool IsReconciled { get; private set; }

    /// <summary>
    /// Reconciliation variance amount (if any).
    /// Example: 125.50 for small difference. Should be zero after reconciliation.
    /// </summary>
    public decimal ReconciliationVariance { get; private set; }

    /// <summary>
    /// Whether the AR account is active and operational.
    /// Default: true. Inactive accounts don't accept new transactions.
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Optional GL account reference for this AR control account.
    /// Links to ChartOfAccount entity (typically account 120-series).
    /// </summary>
    public DefaultIdType? GeneralLedgerAccountId { get; private set; }

    /// <summary>
    /// Optional accounting period for reporting.
    /// Links to AccountingPeriod entity for period-based tracking.
    /// </summary>
    public DefaultIdType? PeriodId { get; private set; }

    /// <summary>
    /// Total amount written off year-to-date.
    /// Example: 25000.00. Used for bad debt expense tracking.
    /// </summary>
    public decimal YearToDateWriteOffs { get; private set; }

    /// <summary>
    /// Total collections year-to-date.
    /// Example: 10500000.00. Used for collection effectiveness analysis.
    /// </summary>
    public decimal YearToDateCollections { get; private set; }

    private AccountsReceivableAccount()
    {
        AccountNumber = string.Empty;
        AccountName = string.Empty;
    }

    private AccountsReceivableAccount(string accountNumber, string accountName,
        DefaultIdType? generalLedgerAccountId = null, DefaultIdType? periodId = null,
        string? description = null, string? notes = null)
    {
        if (string.IsNullOrWhiteSpace(accountNumber))
            throw new ArgumentException("Account number is required", nameof(accountNumber));

        if (accountNumber.Length > MaxAccountNumberLength)
            throw new ArgumentException($"Account number cannot exceed {MaxAccountNumberLength} characters", nameof(accountNumber));

        if (string.IsNullOrWhiteSpace(accountName))
            throw new ArgumentException("Account name is required", nameof(accountName));

        if (accountName.Length > MaxAccountNameLength)
            throw new ArgumentException($"Account name cannot exceed {MaxAccountNameLength} characters", nameof(accountName));

        AccountNumber = accountNumber.Trim();
        Name = accountName.Trim();
        AccountName = accountName.Trim();
        CurrentBalance = 0m;
        Current0To30 = 0m;
        Days31To60 = 0m;
        Days61To90 = 0m;
        Over90Days = 0m;
        AllowanceForDoubtfulAccounts = 0m;
        NetReceivables = 0m;
        CustomerCount = 0;
        DaysSalesOutstanding = 0m;
        BadDebtPercentage = 0m;
        IsReconciled = false;
        ReconciliationVariance = 0m;
        IsActive = true;
        GeneralLedgerAccountId = generalLedgerAccountId;
        PeriodId = periodId;
        YearToDateWriteOffs = 0m;
        YearToDateCollections = 0m;
        Description = description?.Trim();
        Notes = notes?.Trim();

        QueueDomainEvent(new ArAccountCreated(Id, AccountNumber, AccountName, Description, Notes));
    }

    public static AccountsReceivableAccount Create(string accountNumber, string accountName,
        DefaultIdType? generalLedgerAccountId = null, DefaultIdType? periodId = null,
        string? description = null, string? notes = null)
    {
        return new AccountsReceivableAccount(accountNumber, accountName, generalLedgerAccountId,
            periodId, description, notes);
    }

    public AccountsReceivableAccount UpdateBalance(decimal current0To30, decimal days31To60,
        decimal days61To90, decimal over90Days)
    {
        if (current0To30 < 0 || days31To60 < 0 || days61To90 < 0 || over90Days < 0)
            throw new ArgumentException("Aging buckets cannot be negative");

        Current0To30 = current0To30;
        Days31To60 = days31To60;
        Days61To90 = days61To90;
        Over90Days = over90Days;
        CurrentBalance = current0To30 + days31To60 + days61To90 + over90Days;
        NetReceivables = CurrentBalance - AllowanceForDoubtfulAccounts;

        QueueDomainEvent(new ArAccountBalanceUpdated(Id, AccountNumber, CurrentBalance, NetReceivables));
        return this;
    }

    public AccountsReceivableAccount UpdateAllowance(decimal allowanceAmount)
    {
        if (allowanceAmount < 0)
            throw new ArgumentException("Allowance cannot be negative", nameof(allowanceAmount));

        if (allowanceAmount > CurrentBalance)
            throw new ArgumentException("Allowance cannot exceed current balance");

        AllowanceForDoubtfulAccounts = allowanceAmount;
        NetReceivables = CurrentBalance - AllowanceForDoubtfulAccounts;

        QueueDomainEvent(new ArAccountAllowanceUpdated(Id, AccountNumber, AllowanceForDoubtfulAccounts));
        return this;
    }

    public AccountsReceivableAccount RecordWriteOff(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Write-off amount must be positive", nameof(amount));

        YearToDateWriteOffs += amount;
        
        // Update bad debt percentage
        if (YearToDateCollections > 0)
        {
            BadDebtPercentage = YearToDateWriteOffs / (YearToDateCollections + CurrentBalance);
        }

        QueueDomainEvent(new ArAccountWriteOffRecorded(Id, AccountNumber, amount, YearToDateWriteOffs));
        return this;
    }

    public AccountsReceivableAccount RecordCollection(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Collection amount must be positive", nameof(amount));

        YearToDateCollections += amount;

        QueueDomainEvent(new ArAccountCollectionRecorded(Id, AccountNumber, amount, YearToDateCollections));
        return this;
    }

    public AccountsReceivableAccount Reconcile(decimal subsidiaryLedgerBalance)
    {
        ReconciliationVariance = CurrentBalance - subsidiaryLedgerBalance;
        IsReconciled = Math.Abs(ReconciliationVariance) < 0.01m; // Allow penny rounding
        LastReconciliationDate = DateTime.UtcNow;

        QueueDomainEvent(new ArAccountReconciled(Id, AccountNumber, CurrentBalance, subsidiaryLedgerBalance, ReconciliationVariance, IsReconciled));
        return this;
    }

    public AccountsReceivableAccount UpdateMetrics(int customerCount, decimal daysSalesOutstanding)
    {
        CustomerCount = customerCount;
        DaysSalesOutstanding = daysSalesOutstanding;

        QueueDomainEvent(new ArAccountMetricsUpdated(Id, AccountNumber, CustomerCount, DaysSalesOutstanding));
        return this;
    }

    public AccountsReceivableAccount Deactivate()
    {
        if (!IsActive)
            throw new InvalidOperationException("AR account is already inactive");

        IsActive = false;
        return this;
    }

    public AccountsReceivableAccount Activate()
    {
        if (IsActive)
            throw new InvalidOperationException("AR account is already active");

        IsActive = true;
        return this;
    }

    /// <summary>
    /// Percentage of AR in each aging bucket.
    /// </summary>
    public (decimal Current, decimal Days31to60, decimal Days61to90, decimal Over90) AgingPercentages
    {
        get
        {
            if (CurrentBalance == 0) return (0, 0, 0, 0);
            return (
                (Current0To30 / CurrentBalance) * 100,
                (Days31To60 / CurrentBalance) * 100,
                (Days61To90 / CurrentBalance) * 100,
                (Over90Days / CurrentBalance) * 100
            );
        }
    }

    /// <summary>
    /// Collection effectiveness (Collections / Beginning Balance + New Charges).
    /// </summary>
    public decimal CollectionEffectiveness => YearToDateCollections > 0 
        ? (YearToDateCollections / (YearToDateCollections + CurrentBalance)) * 100 
        : 0m;
}

