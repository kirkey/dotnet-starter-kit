using Accounting.Domain.Events.PrepaidExpense;

namespace Accounting.Domain.Entities;

/// <summary>
/// Represents a prepaid expense asset for tracking advance payments and systematic amortization over benefit periods.
/// </summary>
/// <remarks>
/// Use cases:
/// - Track insurance premiums paid in advance and amortize monthly.
/// - Manage prepaid maintenance contracts and service agreements.
/// - Handle prepaid rent, subscriptions, and annual licenses.
/// - Support accurate period-end financial reporting with proper matching.
/// - Enable automated monthly amortization journal entries.
/// - Track remaining prepaid balances for balance sheet reporting.
/// - Support audit trail for prepayment recognition and amortization.
/// - Enable prepaid expense forecasting and budget management.
/// 
/// Default values:
/// - PrepaidNumber: required unique identifier (example: "PREPAID-2025-001")
/// - Description: required description (example: "Annual insurance premium - FY2025")
/// - TotalAmount: required total prepaid amount (example: 12000.00 for annual insurance)
/// - StartDate: required benefit period start (example: 2025-01-01)
/// - EndDate: required benefit period end (example: 2025-12-31)
/// - AmortizedAmount: 0.00 (no amortization initially)
/// - RemainingAmount: equals TotalAmount initially
/// - AmortizationSchedule: "Monthly" (most common frequency)
/// - Status: "Active" (new prepaid expenses start as active)
/// - IsFullyAmortized: false (becomes true when fully amortized)
/// 
/// Business rules:
/// - PrepaidNumber must be unique within the system
/// - TotalAmount must be positive
/// - EndDate must be after StartDate
/// - AmortizedAmount cannot exceed TotalAmount
/// - RemainingAmount = TotalAmount - AmortizedAmount
/// - Monthly amortization = TotalAmount / NumberOfMonths
/// - Cannot delete prepaid expenses with amortization history
/// - Status transitions: Active → FullyAmortized → Closed
/// - Amortization stops when RemainingAmount reaches zero
/// - Requires GL account for prepaid asset and expense account
/// </remarks>
/// <seealso cref="Accounting.Domain.Events.PrepaidExpense.PrepaidExpenseCreated"/>
/// <seealso cref="Accounting.Domain.Events.PrepaidExpense.PrepaidExpenseUpdated"/>
/// <seealso cref="Accounting.Domain.Events.PrepaidExpense.PrepaidExpenseAmortized"/>
/// <seealso cref="Accounting.Domain.Events.PrepaidExpense.PrepaidExpenseFullyAmortized"/>
/// <seealso cref="Accounting.Domain.Events.PrepaidExpense.PrepaidExpenseClosed"/>
public class PrepaidExpense : AuditableEntity, IAggregateRoot
{
    private const int MaxPrepaidNumberLength = 50;
    private const int MaxAmortizationScheduleLength = 32;
    private const int MaxStatusLength = 32;
    private const int MaxVendorNameLength = 256;
    private const int MaxDescriptionLength = 2048;
    private const int MaxNotesLength = 2048;

    /// <summary>
    /// Unique prepaid expense identifier.
    /// Example: "PREPAID-2025-001", "PP-INS-2025". Max length: 50.
    /// </summary>
    public string PrepaidNumber { get; private set; } = string.Empty;

    /// <summary>
    /// Total amount paid in advance.
    /// Example: 12000.00 for $12,000 annual insurance premium.
    /// Must be positive. Represents the prepaid asset balance at inception.
    /// </summary>
    public decimal TotalAmount { get; private set; }

    /// <summary>
    /// Amount amortized to expense to date.
    /// Example: 3000.00 after 3 months of monthly amortization.
    /// Default: 0.00. Updated with each amortization posting.
    /// Cannot exceed TotalAmount.
    /// </summary>
    public decimal AmortizedAmount { get; private set; }

    /// <summary>
    /// Remaining prepaid asset balance.
    /// Example: 9000.00 = 12000 - 3000 after 3 months.
    /// Calculated as TotalAmount - AmortizedAmount.
    /// Represents current prepaid asset on balance sheet.
    /// </summary>
    public decimal RemainingAmount { get; private set; }

    /// <summary>
    /// Start date of the benefit period.
    /// Example: 2025-01-01 for annual insurance starting January 1.
    /// Used to calculate amortization schedule and period allocation.
    /// </summary>
    public DateTime StartDate { get; private set; }

    /// <summary>
    /// End date of the benefit period.
    /// Example: 2025-12-31 for annual insurance ending December 31.
    /// Must be after StartDate. Used to calculate total amortization periods.
    /// </summary>
    public DateTime EndDate { get; private set; }

    /// <summary>
    /// Amortization frequency or schedule.
    /// Values: "Monthly", "Quarterly", "SemiAnnually", "Annually", "Custom".
    /// Example: "Monthly" for standard monthly expense recognition.
    /// Default: "Monthly". Max length: 32.
    /// </summary>
    public string AmortizationSchedule { get; private set; } = string.Empty;

    /// <summary>
    /// Current status of the prepaid expense.
    /// Values: "Active", "FullyAmortized", "Closed", "Cancelled".
    /// Default: "Active". Max length: 32.
    /// </summary>
    public string Status { get; private set; } = string.Empty;

    /// <summary>
    /// Prepaid asset account in chart of accounts.
    /// Links to ChartOfAccount entity (typically 100-series asset accounts).
    /// Example: Account 140 - Prepaid Insurance.
    /// </summary>
    public DefaultIdType PrepaidAssetAccountId { get; private set; }

    /// <summary>
    /// Expense account for amortization postings.
    /// Links to ChartOfAccount entity (typically expense accounts).
    /// Example: Account 620 - Insurance Expense, Account 630 - Maintenance Expense.
    /// </summary>
    public DefaultIdType ExpenseAccountId { get; private set; }

    /// <summary>
    /// Optional vendor who received the prepayment.
    /// Links to Vendor entity for payment tracking and vendor management.
    /// </summary>
    public DefaultIdType? VendorId { get; private set; }

    /// <summary>
    /// Optional vendor name for display purposes.
    /// Example: "ABC Insurance Company". Max length: 256.
    /// Denormalized for reporting convenience.
    /// </summary>
    public string? VendorName { get; private set; }

    /// <summary>
    /// Optional reference to the payment that created this prepaid.
    /// Links to Payment entity for audit trail and reconciliation.
    /// </summary>
    public DefaultIdType? PaymentId { get; private set; }

    /// <summary>
    /// Date the prepayment was made.
    /// Example: 2024-12-15 if paying in advance for calendar year 2025.
    /// Used for cash flow tracking and payment reconciliation.
    /// </summary>
    public DateTime PaymentDate { get; private set; }

    /// <summary>
    /// Date of the last amortization posting.
    /// Example: 2025-03-31 after March amortization.
    /// Null if not yet amortized. Updated with each amortization.
    /// </summary>
    public DateTime? LastAmortizationDate { get; private set; }

    /// <summary>
    /// Date of the next scheduled amortization.
    /// Example: 2025-04-30 for next month's amortization.
    /// Calculated based on AmortizationSchedule and LastAmortizationDate.
    /// </summary>
    public DateTime? NextAmortizationDate { get; private set; }

    /// <summary>
    /// Whether the prepaid expense is fully amortized.
    /// Default: false. True when RemainingAmount reaches zero.
    /// </summary>
    public bool IsFullyAmortized { get; private set; }

    /// <summary>
    /// Optional cost center for expense allocation.
    /// Links to CostCenter entity for departmental reporting.
    /// </summary>
    public DefaultIdType? CostCenterId { get; private set; }

    /// <summary>
    /// Optional accounting period for tracking.
    /// Links to AccountingPeriod entity for period-based reporting.
    /// </summary>
    public DefaultIdType? PeriodId { get; private set; }

    private readonly List<PrepaidAmortizationEntry> _amortizationHistory = new();
    /// <summary>
    /// Historical record of all amortization postings.
    /// Provides audit trail and detailed amortization schedule.
    /// </summary>
    public IReadOnlyCollection<PrepaidAmortizationEntry> AmortizationHistory => _amortizationHistory.AsReadOnly();

    // Parameterless constructor for EF Core
    private PrepaidExpense()
    {
        PrepaidNumber = string.Empty;
        AmortizationSchedule = "Monthly";
        Status = "Active";
    }

    private PrepaidExpense(string prepaidNumber, string description, decimal totalAmount,
        DateTime startDate, DateTime endDate, DefaultIdType prepaidAssetAccountId,
        DefaultIdType expenseAccountId, DateTime paymentDate,
        string amortizationSchedule = "Monthly", DefaultIdType? vendorId = null,
        string? vendorName = null, DefaultIdType? paymentId = null,
        DefaultIdType? costCenterId = null, DefaultIdType? periodId = null,
        string? notes = null)
    {
        // Validations
        if (string.IsNullOrWhiteSpace(prepaidNumber))
            throw new ArgumentException("Prepaid number is required", nameof(prepaidNumber));

        if (prepaidNumber.Length > MaxPrepaidNumberLength)
            throw new ArgumentException($"Prepaid number cannot exceed {MaxPrepaidNumberLength} characters", nameof(prepaidNumber));

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description is required", nameof(description));

        if (totalAmount <= 0)
            throw new ArgumentException("Total amount must be positive", nameof(totalAmount));

        if (endDate <= startDate)
            throw new ArgumentException("End date must be after start date", nameof(endDate));

        if (string.IsNullOrWhiteSpace(amortizationSchedule))
            throw new ArgumentException("Amortization schedule is required", nameof(amortizationSchedule));

        PrepaidNumber = prepaidNumber.Trim();
        Name = prepaidNumber.Trim(); // For AuditableEntity compatibility
        Description = description.Trim();
        TotalAmount = totalAmount;
        AmortizedAmount = 0m;
        RemainingAmount = totalAmount;
        StartDate = startDate;
        EndDate = endDate;
        AmortizationSchedule = amortizationSchedule.Trim();
        Status = "Active";
        PrepaidAssetAccountId = prepaidAssetAccountId;
        ExpenseAccountId = expenseAccountId;
        VendorId = vendorId;
        VendorName = vendorName?.Trim();
        PaymentId = paymentId;
        PaymentDate = paymentDate;
        NextAmortizationDate = CalculateNextAmortizationDate(startDate, amortizationSchedule);
        IsFullyAmortized = false;
        CostCenterId = costCenterId;
        PeriodId = periodId;
        Notes = notes?.Trim();

        QueueDomainEvent(new PrepaidExpenseCreated(Id, PrepaidNumber, Description, TotalAmount, StartDate, EndDate, Notes));
    }

    /// <summary>
    /// Factory method to create a new prepaid expense with validation.
    /// </summary>
    public static PrepaidExpense Create(string prepaidNumber, string description, decimal totalAmount,
        DateTime startDate, DateTime endDate, DefaultIdType prepaidAssetAccountId,
        DefaultIdType expenseAccountId, DateTime paymentDate,
        string amortizationSchedule = "Monthly", DefaultIdType? vendorId = null,
        string? vendorName = null, DefaultIdType? paymentId = null,
        DefaultIdType? costCenterId = null, DefaultIdType? periodId = null,
        string? notes = null)
    {
        return new PrepaidExpense(prepaidNumber, description, totalAmount, startDate, endDate,
            prepaidAssetAccountId, expenseAccountId, paymentDate, amortizationSchedule,
            vendorId, vendorName, paymentId, costCenterId, periodId, notes);
    }

    /// <summary>
    /// Update prepaid expense details; not allowed when fully amortized.
    /// </summary>
    public PrepaidExpense Update(string? description = null, DateTime? endDate = null,
        DefaultIdType? costCenterId = null, string? notes = null)
    {
        if (IsFullyAmortized)
            throw new InvalidOperationException("Cannot modify fully amortized prepaid expense");

        bool isUpdated = false;

        if (!string.IsNullOrWhiteSpace(description) && Description != description.Trim())
        {
            Description = description.Trim();
            isUpdated = true;
        }

        if (endDate.HasValue && EndDate != endDate.Value)
        {
            if (endDate.Value <= StartDate)
                throw new ArgumentException("End date must be after start date");
            EndDate = endDate.Value;
            isUpdated = true;
        }

        if (costCenterId.HasValue && CostCenterId != costCenterId.Value)
        {
            CostCenterId = costCenterId.Value;
            isUpdated = true;
        }

        if (notes != Notes)
        {
            Notes = notes?.Trim();
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new PrepaidExpenseUpdated(Id, PrepaidNumber, Description, Notes));
        }

        return this;
    }

    /// <summary>
    /// Record an amortization posting for the period.
    /// </summary>
    public PrepaidExpense RecordAmortization(decimal amortizationAmount, DateTime postingDate,
        DefaultIdType? journalEntryId = null)
    {
        if (IsFullyAmortized)
            throw new InvalidOperationException("Prepaid expense is already fully amortized");

        if (amortizationAmount <= 0)
            throw new ArgumentException("Amortization amount must be positive", nameof(amortizationAmount));

        if (amortizationAmount > RemainingAmount)
            throw new ArgumentException($"Amortization amount {amortizationAmount:N2} exceeds remaining balance {RemainingAmount:N2}");

        AmortizedAmount += amortizationAmount;
        RemainingAmount = TotalAmount - AmortizedAmount;
        LastAmortizationDate = postingDate;

        // Add to history
        var entry = PrepaidAmortizationEntry.Create(postingDate, amortizationAmount, RemainingAmount, journalEntryId);
        _amortizationHistory.Add(entry);

        // Check if fully amortized
        if (RemainingAmount <= 0.01m) // Allow small rounding difference
        {
            IsFullyAmortized = true;
            Status = "FullyAmortized";
            NextAmortizationDate = null;
            QueueDomainEvent(new PrepaidExpenseFullyAmortized(Id, PrepaidNumber, Description, TotalAmount, postingDate));
        }
        else
        {
            NextAmortizationDate = CalculateNextAmortizationDate(postingDate, AmortizationSchedule);
            QueueDomainEvent(new PrepaidExpenseAmortized(Id, PrepaidNumber, amortizationAmount, RemainingAmount, postingDate));
        }

        return this;
    }

    /// <summary>
    /// Calculate standard monthly amortization amount.
    /// </summary>
    public decimal CalculateMonthlyAmortization()
    {
        int totalMonths = CalculateTotalMonths();
        return totalMonths > 0 ? TotalAmount / totalMonths : TotalAmount;
    }

    /// <summary>
    /// Calculate total months in benefit period.
    /// </summary>
    private int CalculateTotalMonths()
    {
        int months = ((EndDate.Year - StartDate.Year) * 12) + EndDate.Month - StartDate.Month + 1;
        return Math.Max(1, months);
    }

    /// <summary>
    /// Calculate next amortization date based on schedule.
    /// </summary>
    private DateTime? CalculateNextAmortizationDate(DateTime fromDate, string schedule)
    {
        DateTime nextDate = schedule.ToLowerInvariant() switch
        {
            "monthly" => fromDate.AddMonths(1),
            "quarterly" => fromDate.AddMonths(3),
            "semiannually" => fromDate.AddMonths(6),
            "annually" => fromDate.AddYears(1),
            _ => fromDate.AddMonths(1) // Default to monthly
        };

        return nextDate <= EndDate ? nextDate : null;
    }

    /// <summary>
    /// Close the prepaid expense.
    /// </summary>
    public PrepaidExpense Close()
    {
        if (!IsFullyAmortized)
            throw new InvalidOperationException("Can only close fully amortized prepaid expenses");

        Status = "Closed";
        QueueDomainEvent(new PrepaidExpenseClosed(Id, PrepaidNumber, Description));
        return this;
    }

    /// <summary>
    /// Cancel the prepaid expense (if not yet amortized).
    /// </summary>
    public PrepaidExpense Cancel(string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("Cancellation reason is required", nameof(reason));

        if (AmortizedAmount > 0)
            throw new InvalidOperationException("Cannot cancel prepaid expense with amortization history");

        Status = "Cancelled";
        Notes = $"{Notes}\n\nCancelled: {reason.Trim()}".Trim();

        QueueDomainEvent(new PrepaidExpenseCancelled(Id, PrepaidNumber, reason));
        return this;
    }

    /// <summary>
    /// Percentage of prepaid expense amortized.
    /// </summary>
    public decimal AmortizationPercentage => TotalAmount > 0 ? (AmortizedAmount / TotalAmount) * 100 : 0;

    /// <summary>
    /// Number of amortization postings recorded.
    /// </summary>
    public int AmortizationCount => _amortizationHistory.Count;
}

/// <summary>
/// Represents a single amortization entry in the prepaid expense history.
/// </summary>
public class PrepaidAmortizationEntry
{
    /// <summary>
    /// Date the amortization was posted.
    /// Example: 2025-03-31 for March amortization.
    /// </summary>
    public DateTime PostingDate { get; private set; }

    /// <summary>
    /// Amount amortized in this posting.
    /// Example: 1000.00 for monthly amortization of annual premium.
    /// </summary>
    public decimal AmortizationAmount { get; private set; }

    /// <summary>
    /// Remaining prepaid balance after this posting.
    /// Example: 9000.00 remaining after first month.
    /// </summary>
    public decimal RemainingBalance { get; private set; }

    /// <summary>
    /// Optional reference to journal entry that posted the amortization.
    /// Links to JournalEntry entity for GL integration.
    /// </summary>
    public DefaultIdType? JournalEntryId { get; private set; }

    private PrepaidAmortizationEntry()
    {
        // EF Core constructor
    }

    private PrepaidAmortizationEntry(DateTime postingDate, decimal amortizationAmount,
        decimal remainingBalance, DefaultIdType? journalEntryId)
    {
        PostingDate = postingDate;
        AmortizationAmount = amortizationAmount;
        RemainingBalance = remainingBalance;
        JournalEntryId = journalEntryId;
    }

    public static PrepaidAmortizationEntry Create(DateTime postingDate, decimal amortizationAmount,
        decimal remainingBalance, DefaultIdType? journalEntryId = null)
    {
        return new PrepaidAmortizationEntry(postingDate, amortizationAmount, remainingBalance, journalEntryId);
    }
}

