using Accounting.Domain.Events.AccountsPayableAccount;

namespace Accounting.Domain.Entities;

/// <summary>
/// Represents an accounts payable control account for tracking vendor balances, payment schedules, and AP aging with subsidiary ledger reconciliation.
/// </summary>
/// <remarks>
/// Use cases:
/// - Track aggregate accounts payable balances by vendor type or category.
/// - Support aging analysis for payment planning and cash flow management.
/// - Enable AP reconciliation with subsidiary vendor ledgers.
/// - Monitor payment terms compliance and early payment discount opportunities.
/// - Track payment effectiveness and days payable outstanding (DPO).
/// - Support accrual and payment cycle management.
/// - Generate AP aging reports for management and vendor relations.
/// - Enable cash flow forecasting based on payment schedules.
/// 
/// Default values:
/// - AccountNumber: required unique identifier (example: "AP-001")
/// - AccountName: required descriptive name (example: "Accounts Payable - Vendors")
/// - CurrentBalance: 0.00 (total outstanding payables)
/// - Current0to30: 0.00, Days31to60: 0.00, Days61to90: 0.00, Over90Days: 0.00
/// - IsActive: true
/// 
/// Business rules:
/// - CurrentBalance = sum of all aging buckets
/// - Aging buckets must be non-negative
/// - DPO optimal range: 30-60 days
/// - Reconciliation with subsidiary ledger required monthly
/// - Early payment discounts tracked for savings opportunities
/// </remarks>
public class AccountsPayableAccount : AuditableEntity, IAggregateRoot
{
    public string AccountNumber { get; private set; } = string.Empty;
    public string AccountName { get; private set; } = string.Empty;
    public decimal CurrentBalance { get; private set; }
    public decimal Current0to30 { get; private set; }
    public decimal Days31to60 { get; private set; }
    public decimal Days61to90 { get; private set; }
    public decimal Over90Days { get; private set; }
    public int VendorCount { get; private set; }
    public decimal DaysPayableOutstanding { get; private set; }
    public DateTime? LastReconciliationDate { get; private set; }
    public bool IsReconciled { get; private set; }
    public decimal ReconciliationVariance { get; private set; }
    public bool IsActive { get; private set; }
    public DefaultIdType? GeneralLedgerAccountId { get; private set; }
    public DefaultIdType? PeriodId { get; private set; }
    public decimal YearToDatePayments { get; private set; }
    public decimal YearToDateDiscountsTaken { get; private set; }
    public decimal YearToDateDiscountsLost { get; private set; }

    private AccountsPayableAccount() { }

    private AccountsPayableAccount(string accountNumber, string accountName,
        DefaultIdType? generalLedgerAccountId = null, DefaultIdType? periodId = null,
        string? description = null, string? notes = null)
    {
        if (string.IsNullOrWhiteSpace(accountNumber))
            throw new ArgumentException("Account number is required", nameof(accountNumber));
        if (string.IsNullOrWhiteSpace(accountName))
            throw new ArgumentException("Account name is required", nameof(accountName));

        AccountNumber = accountNumber.Trim();
        Name = accountName.Trim();
        AccountName = accountName.Trim();
        CurrentBalance = 0m;
        Current0to30 = 0m;
        Days31to60 = 0m;
        Days61to90 = 0m;
        Over90Days = 0m;
        VendorCount = 0;
        DaysPayableOutstanding = 0m;
        IsReconciled = false;
        ReconciliationVariance = 0m;
        IsActive = true;
        GeneralLedgerAccountId = generalLedgerAccountId;
        PeriodId = periodId;
        Description = description?.Trim();
        Notes = notes?.Trim();

        QueueDomainEvent(new APAccountCreated(Id, AccountNumber, AccountName, Description, Notes));
    }

    public static AccountsPayableAccount Create(string accountNumber, string accountName,
        DefaultIdType? generalLedgerAccountId = null, DefaultIdType? periodId = null,
        string? description = null, string? notes = null)
    {
        return new AccountsPayableAccount(accountNumber, accountName, generalLedgerAccountId,
            periodId, description, notes);
    }

    public AccountsPayableAccount UpdateBalance(decimal current0to30, decimal days31to60,
        decimal days61to90, decimal over90Days)
    {
        if (current0to30 < 0 || days31to60 < 0 || days61to90 < 0 || over90Days < 0)
            throw new ArgumentException("Aging buckets cannot be negative");

        Current0to30 = current0to30;
        Days31to60 = days31to60;
        Days61to90 = days61to90;
        Over90Days = over90Days;
        CurrentBalance = current0to30 + days31to60 + days61to90 + over90Days;

        QueueDomainEvent(new APAccountBalanceUpdated(Id, AccountNumber, CurrentBalance));
        return this;
    }

    public AccountsPayableAccount RecordPayment(decimal amount, bool discountTaken, decimal discountAmount)
    {
        if (amount <= 0)
            throw new ArgumentException("Payment amount must be positive", nameof(amount));

        YearToDatePayments += amount;
        if (discountTaken)
            YearToDateDiscountsTaken += discountAmount;

        QueueDomainEvent(new APAccountPaymentRecorded(Id, AccountNumber, amount, YearToDatePayments));
        return this;
    }

    public AccountsPayableAccount RecordDiscountLost(decimal discountAmount)
    {
        if (discountAmount <= 0)
            throw new ArgumentException("Discount amount must be positive", nameof(discountAmount));

        YearToDateDiscountsLost += discountAmount;

        QueueDomainEvent(new APAccountDiscountLost(Id, AccountNumber, discountAmount, YearToDateDiscountsLost));
        return this;
    }

    public AccountsPayableAccount Reconcile(decimal subsidiaryLedgerBalance)
    {
        ReconciliationVariance = CurrentBalance - subsidiaryLedgerBalance;
        IsReconciled = Math.Abs(ReconciliationVariance) < 0.01m;
        LastReconciliationDate = DateTime.UtcNow;

        QueueDomainEvent(new APAccountReconciled(Id, AccountNumber, CurrentBalance, subsidiaryLedgerBalance, ReconciliationVariance, IsReconciled));
        return this;
    }

    public AccountsPayableAccount UpdateMetrics(int vendorCount, decimal daysPayableOutstanding)
    {
        VendorCount = vendorCount;
        DaysPayableOutstanding = daysPayableOutstanding;

        QueueDomainEvent(new APAccountMetricsUpdated(Id, AccountNumber, VendorCount, DaysPayableOutstanding));
        return this;
    }

    public (decimal Current, decimal Days31to60, decimal Days61to90, decimal Over90) AgingPercentages
    {
        get
        {
            if (CurrentBalance == 0) return (0, 0, 0, 0);
            return (
                (Current0to30 / CurrentBalance) * 100,
                (Days31to60 / CurrentBalance) * 100,
                (Days61to90 / CurrentBalance) * 100,
                (Over90Days / CurrentBalance) * 100
            );
        }
    }

    public decimal DiscountCaptureRate => (YearToDateDiscountsTaken + YearToDateDiscountsLost) > 0
        ? (YearToDateDiscountsTaken / (YearToDateDiscountsTaken + YearToDateDiscountsLost)) * 100
        : 0m;
}

