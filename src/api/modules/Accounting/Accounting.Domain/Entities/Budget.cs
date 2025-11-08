using Accounting.Domain.Events.Budget;

namespace Accounting.Domain.Entities;

/// <summary>
/// Represents a financial budget for a fiscal year and period, comprised of budget lines per account.
/// </summary>
/// <remarks>
/// Use cases:
/// - Create annual operating budgets for departments and cost centers.
/// - Develop capital expenditure budgets for infrastructure investments.
/// - Build cash flow budgets for liquidity planning and management.
/// - Establish variance analysis by comparing budgeted vs actual amounts.
/// - Support budget approval workflows with status progression (Draft → Approved → Active → Closed).
/// - Enable budget vs actual reporting for management and regulatory compliance.
/// - Track budget line items by account for detailed financial control.
/// - Support budget amendments and revisions during the fiscal year.
/// 
/// Default values:
/// - Status: "Draft" (new budgets start in draft state)
/// - TotalBudgetedAmount: 0.00 (calculated from budget lines)
/// - TotalActualAmount: 0.00 (updated from actual postings)
/// - ApprovedDate: null (set when budget is approved)
/// - ApprovedBy: null (set when budget is approved)
/// </remarks>
/// <seealso cref="Accounting.Domain.Events.Budget.BudgetCreated"/>
/// <seealso cref="Accounting.Domain.Events.Budget.BudgetUpdated"/>
/// <seealso cref="Accounting.Domain.Events.Budget.BudgetApproved"/>
/// <seealso cref="Accounting.Domain.Events.Budget.BudgetActivated"/>
/// <seealso cref="Accounting.Domain.Events.Budget.BudgetClosed"/>
public class Budget : AuditableEntityWithApproval, IAggregateRoot
{
    private const int MaxNameLength = 256;
    private const int MaxBudgetTypeLength = 32;
    private const int MaxStatusLength = 16;
    private const int MaxDescriptionLength = 1000;
    private const int MaxNotesLength = 1000;

    /// <summary>
    /// The accounting period this budget is associated with.
    /// Example: References the period ID for "FY2025-Q1" or "2025-Annual" periods.
    /// </summary>
    public DefaultIdType PeriodId { get; private set; }

    /// <summary>
    /// The human-readable name of the accounting period (denormalized for display).
    /// Example: "FY2025-Q1" or "2025 - Annual".
    /// </summary>
    public string PeriodName { get; private set; } = string.Empty;

    /// <summary>
    /// The fiscal year for which this budget applies (1900-2100).
    /// Example: 2025 for fiscal year 2025 budget planning.
    /// </summary>
    public int FiscalYear { get; private set; }

    /// <summary>
    /// Budget classification indicating the type of budget.
    /// Example: "Operating" for day-to-day expenses, "Capital" for asset purchases, "Cash Flow" for liquidity planning.
    /// Allowed values: Operating, Capital, Cash Flow, Emergency, Special Projects.
    /// </summary>
    public string BudgetType { get; private set; } = string.Empty; // Operating, Capital, Cash Flow

    /// <summary>
    /// Sum of all budgeted amounts across all budget lines.
    /// Example: 1500000.00 for total annual operating budget of $1.5M.
    /// Default: 0.00. Automatically recalculated when budget lines are added/updated/removed.
    /// </summary>
    public decimal TotalBudgetedAmount { get; private set; }

    /// <summary>
    /// Sum of actual amounts posted against this budget from accounting transactions.
    /// Example: 375000.00 for $375K actual spending against budget (25% utilization).
    /// Default: 0.00. Updated via UpdateActuals() method from general ledger postings.
    /// </summary>
    public decimal TotalActualAmount { get; private set; }


    private readonly List<BudgetDetail> _budgetDetails = new();
    /// <summary>
    /// Collection of budget details, each representing a budgeted amount for a specific chart of account.
    /// </summary>
    public IReadOnlyCollection<BudgetDetail> BudgetDetails => _budgetDetails.AsReadOnly();

    // Parameterless constructor for EF Core
    private Budget()
    {
    }

    private Budget(string budgetName, DefaultIdType periodId, string periodName, int fiscalYear, string budgetType, string? description = null, string? notes = null)
    {
        var name = (budgetName ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Budget name is required.");
        if (name.Length > MaxNameLength)
            throw new ArgumentException($"Budget name cannot exceed {MaxNameLength} characters.");

        if (periodId == default)
            throw new ArgumentException("PeriodId is required.");

        var pName = (periodName ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(pName))
            throw new ArgumentException("PeriodName is required.");
        if (pName.Length > 128)
            pName = pName.Substring(0, 128);

        if (fiscalYear is < 1900 or > 2100)
            throw new ArgumentException("Fiscal year is out of range.");

        var bt = (budgetType ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(bt))
            throw new ArgumentException("BudgetType is required.");
        if (bt.Length > MaxBudgetTypeLength)
            throw new ArgumentException($"BudgetType cannot exceed {MaxBudgetTypeLength} characters.");

        Name = name;
        PeriodId = periodId;
        PeriodName = pName;
        FiscalYear = fiscalYear;
        BudgetType = bt;
        Status = "Draft";
        TotalBudgetedAmount = 0;
        TotalActualAmount = 0;

        var desc = description?.Trim();
        if (!string.IsNullOrEmpty(desc) && desc.Length > MaxDescriptionLength)
            desc = desc.Substring(0, MaxDescriptionLength);
        Description = desc;

        var nts = notes?.Trim();
        if (!string.IsNullOrEmpty(nts) && nts.Length > MaxNotesLength)
            nts = nts.Substring(0, MaxNotesLength);
        Notes = nts;

        QueueDomainEvent(new BudgetCreated(Id, Name, PeriodId, PeriodName, FiscalYear, BudgetType, Description, Notes));
    }

    /// <summary>
    /// Factory method to create a budget with initial metadata.
    /// </summary>
    public static Budget Create(string budgetName, DefaultIdType periodId, string periodName, int fiscalYear, string budgetType, string? description = null, string? notes = null)
    {
        return new Budget(budgetName, periodId, periodName, fiscalYear, budgetType, description, notes);
    }

    /// <summary>
    /// Update editable properties when status allows (not Approved/Active).
    /// </summary>
    public Budget Update(DefaultIdType periodId, string periodName, int fiscalYear, string? budgetName, string? budgetType, string? status, string? description, string? notes)
    {
        bool isUpdated = false;
        
        if (periodId == default)
            throw new ArgumentException("PeriodId is required.");
        if (PeriodId != periodId)
        {
            PeriodId = periodId;
            isUpdated = true;
        }
        
        if (string.IsNullOrWhiteSpace(periodName))
            throw new ArgumentException("PeriodName is required.");
        if (periodName.Length > 128)
            periodName = periodName.Substring(0, 128);
        if (periodName != PeriodName)
        {
            PeriodName = periodName;
            isUpdated = true;
        }

        if (Status is "Approved" or "Active")
            throw new BudgetCannotBeModifiedException(Id);
        
        if (fiscalYear is < 1900 or > 2100)
            throw new ArgumentException("Fiscal year is out of range.");
        if (fiscalYear != FiscalYear)
        {
            FiscalYear = fiscalYear;
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(periodName) && PeriodName != periodName)
        {
            var pn = periodName.Trim();
            if (pn.Length > 128)
                pn = pn.Substring(0, 128);
            PeriodName = pn;
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(budgetName) && Name != budgetName)
        {
            var n = budgetName.Trim();
            if (n.Length > MaxNameLength)
                throw new ArgumentException($"Budget name cannot exceed {MaxNameLength} characters.");
            Name = n;
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(budgetType) && BudgetType != budgetType)
        {
            var bt = budgetType.Trim();
            if (bt.Length > MaxBudgetTypeLength)
                throw new ArgumentException($"BudgetType cannot exceed {MaxBudgetTypeLength} characters.");
            BudgetType = bt;
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(status) && Status != status)
        {
            var st = status.Trim();
            if (st.Length > MaxStatusLength)
                throw new ArgumentException($"Status cannot exceed {MaxStatusLength} characters.");
            Status = st;
            isUpdated = true;
        }

        if (description != Description)
        {
            var desc = description?.Trim();
            if (!string.IsNullOrEmpty(desc) && desc.Length > MaxDescriptionLength)
                desc = desc.Substring(0, MaxDescriptionLength);
            Description = desc;
            isUpdated = true;
        }

        if (notes != Notes)
        {
            var nts = notes?.Trim();
            if (!string.IsNullOrEmpty(nts) && nts.Length > MaxNotesLength)
                nts = nts.Substring(0, MaxNotesLength);
            Notes = nts;
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new BudgetUpdated(this));
        }

        return this;
    }

    /// <summary>
    /// Mark the budget as approved; records approver and timestamp. Requires at least one detail.
    /// </summary>
    public Budget Approve(string approvedBy)
    {
        if (Status == "Approved")
            throw new BudgetAlreadyApprovedException(Id);

        if (_budgetDetails.Count == 0)
            throw new EmptyBudgetCannotBeApprovedException(Id);

        Status = "Approved";
        ApprovedOn = DateTime.UtcNow;
        ApprovedBy = Guid.TryParse(approvedBy, out var guidValue) ? guidValue : null;
        ApproverName = approvedBy.Trim();

        QueueDomainEvent(new BudgetApproved(Id, ApprovedOn.Value, ApproverName));
        return this;
    }

    /// <summary>
    /// Transition budget to Active state after approval.
    /// </summary>
    public Budget Activate()
    {
        if (Status != "Approved")
            throw new BudgetNotApprovedException(Id);

        Status = "Active";
        QueueDomainEvent(new BudgetActivated(Id, Name));
        return this;
    }

    /// <summary>
    /// Close an active budget.
    /// </summary>
    public Budget Close()
    {
        if (Status != "Active")
            throw new BudgetNotApprovedException(Id);

        Status = "Closed";
        QueueDomainEvent(new BudgetClosed(Id, Name));
        return this;
    }

    /// <summary>
    /// Update the aggregate totals after details have changed. Intended to be called by application handlers
    /// managing <see cref="BudgetDetail"/> entities, not by consumers directly.
    /// </summary>
    /// <param name="totalBudgeted">New total of all BudgetedAmount values for details belonging to this budget.</param>
    /// <param name="totalActual">New total of all ActualAmount values for details belonging to this budget.</param>
    public Budget SetTotals(decimal totalBudgeted, decimal totalActual)
    {
        if (Status is "Approved" or "Active")
            throw new BudgetCannotBeModifiedException(Id);

        if (totalBudgeted < 0 || totalActual < 0)
            throw new InvalidBudgetAmountException();

        TotalBudgetedAmount = totalBudgeted;
        TotalActualAmount = totalActual;
        QueueDomainEvent(new BudgetUpdated(this));
        return this;
    }

    /// <summary>
    /// Total variance across all budget lines (Budgeted - Actual).
    /// </summary>
    public decimal GetTotalVariance()
    {
        return TotalBudgetedAmount - TotalActualAmount;
    }

    /// <summary>
    /// Percentage variance relative to total budgeted amount.
    /// </summary>
    public decimal GetVariancePercentage()
    {
        return TotalBudgetedAmount > 0 ? ((TotalBudgetedAmount - TotalActualAmount) / TotalBudgetedAmount) * 100 : 0;
    }
}
