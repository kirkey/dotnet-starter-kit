using Accounting.Domain.Events.Budget;

namespace Accounting.Domain;

/// <summary>
/// Represents a financial budget for a fiscal year and period, comprised of budget lines per account.
/// </summary>
/// <remarks>
/// Tracks type (Operating, Capital, etc.), status lifecycle (Draft, Approved, Active, Closed), totals, and approvals.
/// Defaults: <see cref="Status"/> is "Draft"; <see cref="TotalBudgetedAmount"/> and <see cref="TotalActualAmount"/> start at 0.
/// </remarks>
public class Budget : AuditableEntity, IAggregateRoot
{
    private const int MaxNameLength = 256;
    private const int MaxBudgetTypeLength = 32;
    private const int MaxStatusLength = 16;
    private const int MaxDescriptionLength = 1000;
    private const int MaxNotesLength = 1000;

    /// <summary>
    /// The accounting period the budget is associated with.
    /// </summary>
    public DefaultIdType PeriodId { get; private set; }

    /// <summary>
    /// The fiscal year for which this budget applies.
    /// </summary>
    public int FiscalYear { get; private set; }

    /// <summary>
    /// Budget classification, e.g. Operating, Capital, Cash Flow.
    /// </summary>
    public string BudgetType { get; private set; } = string.Empty; // Operating, Capital, Cash Flow

    /// <summary>
    /// Workflow status of the budget: Draft, Approved, Active, or Closed.
    /// </summary>
    /// <remarks>Defaults to <c>"Draft"</c> on creation.</remarks>
    public string Status { get; private set; } = string.Empty; // Draft, Approved, Active, Closed

    /// <summary>
    /// Sum of all budgeted amounts across lines.
    /// </summary>
    /// <remarks>Recomputed when lines are added/updated/removed. Defaults to 0.</remarks>
    public decimal TotalBudgetedAmount { get; private set; }

    /// <summary>
    /// Sum of actual amounts posted against this budget.
    /// </summary>
    /// <remarks>Updated via <see cref="UpdateActuals"/>. Defaults to 0.</remarks>
    public decimal TotalActualAmount { get; private set; }

    /// <summary>
    /// When the budget was approved, if applicable.
    /// </summary>
    public DateTime? ApprovedDate { get; private set; }

    /// <summary>
    /// User who approved the budget, if applicable.
    /// </summary>
    public string? ApprovedBy { get; private set; }

    private readonly List<BudgetLine> _budgetLines = new();
    /// <summary>
    /// The collection of budget lines, one per account.
    /// </summary>
    public IReadOnlyCollection<BudgetLine> BudgetLines => _budgetLines.AsReadOnly();

    // Parameterless constructor for EF Core
    private Budget()
    {
    }

    private Budget(string budgetName, DefaultIdType periodId, int fiscalYear, string budgetType, string? description = null, string? notes = null)
    {
        var name = (budgetName ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Budget name is required.");
        if (name.Length > MaxNameLength)
            throw new ArgumentException($"Budget name cannot exceed {MaxNameLength} characters.");

        if (periodId == default)
            throw new ArgumentException("PeriodId is required.");

        if (fiscalYear is < 1900 or > 2100)
            throw new ArgumentException("Fiscal year is out of range.");

        var bt = (budgetType ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(bt))
            throw new ArgumentException("BudgetType is required.");
        if (bt.Length > MaxBudgetTypeLength)
            throw new ArgumentException($"BudgetType cannot exceed {MaxBudgetTypeLength} characters.");

        Name = name;
        PeriodId = periodId;
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

        QueueDomainEvent(new BudgetCreated(Id, Name, PeriodId, FiscalYear, BudgetType, Description, Notes));
    }

    /// <summary>
    /// Factory method to create a budget with initial metadata.
    /// </summary>
    public static Budget Create(string budgetName, DefaultIdType periodId, int fiscalYear, string budgetType, string? description = null, string? notes = null)
    {
        return new Budget(budgetName, periodId, fiscalYear, budgetType, description, notes);
    }

    /// <summary>
    /// Update editable properties when status allows (not Approved/Active).
    /// </summary>
    public Budget Update(string? budgetName, string? budgetType, string? status, string? description, string? notes)
    {
        bool isUpdated = false;

        if (Status is "Approved" or "Active")
            throw new BudgetCannotBeModifiedException(Id);

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
    /// Add a new budget line for a given account and amount.
    /// </summary>
    public Budget AddBudgetLine(DefaultIdType accountId, decimal budgetedAmount, string? description = null)
    {
        if (Status is "Approved" or "Active")
            throw new BudgetCannotBeModifiedException(Id);

        if (budgetedAmount < 0)
            throw new InvalidBudgetAmountException();

        var existingLine = _budgetLines.FirstOrDefault(bl => bl.AccountId == accountId);
        if (existingLine != null)
            throw new BudgetLineAlreadyExistsException(Id, accountId);

        var budgetLine = BudgetLine.Create(Id, accountId, budgetedAmount, description);
        _budgetLines.Add(budgetLine);

        RecalculateTotalBudgetedAmount();
        QueueDomainEvent(new BudgetLineAdded(Id, accountId, budgetedAmount));
        return this;
    }

    /// <summary>
    /// Update an existing budget line for a given account.
    /// </summary>
    public Budget UpdateBudgetLine(DefaultIdType accountId, decimal? budgetedAmount, string? description)
    {
        if (Status is "Approved" or "Active")
            throw new BudgetCannotBeModifiedException(Id);

        var budgetLine = _budgetLines.FirstOrDefault(bl => bl.AccountId == accountId);
        if (budgetLine == null)
            throw new BudgetLineNotFoundException(Id, accountId);

        budgetLine.Update(budgetedAmount, description);
        RecalculateTotalBudgetedAmount();
        QueueDomainEvent(new BudgetLineUpdated(Id, accountId, budgetLine.BudgetedAmount));
        return this;
    }

    /// <summary>
    /// Remove a budget line for a given account.
    /// </summary>
    public Budget RemoveBudgetLine(DefaultIdType accountId)
    {
        if (Status is "Approved" or "Active")
            throw new BudgetCannotBeModifiedException(Id);

        var budgetLine = _budgetLines.FirstOrDefault(bl => bl.AccountId == accountId);
        if (budgetLine == null)
            throw new BudgetLineNotFoundException(Id, accountId);

        _budgetLines.Remove(budgetLine);
        RecalculateTotalBudgetedAmount();
        QueueDomainEvent(new BudgetLineRemoved(Id, accountId));
        return this;
    }

    /// <summary>
    /// Mark the budget as approved; records approver and timestamp. Requires at least one line.
    /// </summary>
    public Budget Approve(string approvedBy)
    {
        if (Status == "Approved")
            throw new BudgetAlreadyApprovedException(Id);

        if (_budgetLines.Count == 0)
            throw new EmptyBudgetCannotBeApprovedException(Id);

        Status = "Approved";
        ApprovedDate = DateTime.UtcNow;
        ApprovedBy = approvedBy.Trim();

        QueueDomainEvent(new BudgetApproved(Id, ApprovedDate.Value, ApprovedBy));
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
    /// Update actual spending for a specific account and refresh totals.
    /// </summary>
    public Budget UpdateActuals(DefaultIdType accountId, decimal actualAmount)
    {
        var budgetLine = _budgetLines.FirstOrDefault(bl => bl.AccountId == accountId);
        if (budgetLine == null)
            throw new InvalidOperationException("Budget line not found");

        budgetLine.UpdateActual(actualAmount);
        RecalculateTotalActualAmount();
        return this;
    }

    private void RecalculateTotalBudgetedAmount()
    {
        TotalBudgetedAmount = _budgetLines.Sum(bl => bl.BudgetedAmount);
    }

    private void RecalculateTotalActualAmount()
    {
        TotalActualAmount = _budgetLines.Sum(bl => bl.ActualAmount);
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

/// <summary>
/// A single line item within a budget representing an amount allocated to an account.
/// </summary>
public class BudgetLine : BaseEntity
{
    private const int MaxBudgetLineDescriptionLength = 500;

    /// <summary>
    /// The parent budget identifier.
    /// </summary>
    public DefaultIdType BudgetId { get; private set; }

    /// <summary>
    /// The account this budget line applies to.
    /// </summary>
    public DefaultIdType AccountId { get; private set; }

    /// <summary>
    /// The amount allocated for the account. Must be non-negative.
    /// </summary>
    public decimal BudgetedAmount { get; private set; }

    /// <summary>
    /// The actual amount recorded against the account.
    /// </summary>
    /// <remarks>Defaults to 0 until updated via <see cref="UpdateActual"/>.</remarks>
    public decimal ActualAmount { get; private set; }

    /// <summary>
    /// Optional description for the budget line.
    /// </summary>
    public string? Description { get; private set; }

    private BudgetLine(DefaultIdType budgetId, DefaultIdType accountId, 
        decimal budgetedAmount, string? description = null)
    {
        if (budgetedAmount < 0)
            throw new InvalidBudgetAmountException();

        BudgetId = budgetId;
        AccountId = accountId;
        BudgetedAmount = budgetedAmount;
        ActualAmount = 0;

        var desc = description?.Trim();
        if (!string.IsNullOrEmpty(desc) && desc.Length > MaxBudgetLineDescriptionLength)
            desc = desc.Substring(0, MaxBudgetLineDescriptionLength);
        Description = desc;
    }

    /// <summary>
    /// Create a new budget line for the specified account.
    /// </summary>
    public static BudgetLine Create(DefaultIdType budgetId, DefaultIdType accountId,
        decimal budgetedAmount, string? description = null)
    {
        if (budgetedAmount < 0)
            throw new InvalidBudgetAmountException();

        return new BudgetLine(budgetId, accountId, budgetedAmount, description);
    }

    /// <summary>
    /// Update budgeted amount and/or description.
    /// </summary>
    public BudgetLine Update(decimal? budgetedAmount, string? description)
    {
        if (budgetedAmount.HasValue && BudgetedAmount != budgetedAmount.Value)
        {
            if (budgetedAmount.Value < 0)
                throw new InvalidBudgetAmountException();
            BudgetedAmount = budgetedAmount.Value;
        }

        if (description != Description)
        {
            var desc = description?.Trim();
            if (!string.IsNullOrEmpty(desc) && desc.Length > MaxBudgetLineDescriptionLength)
                desc = desc.Substring(0, MaxBudgetLineDescriptionLength);
            Description = desc;
        }

        return this;
    }

    /// <summary>
    /// Update the actual amount posted for this account.
    /// </summary>
    public BudgetLine UpdateActual(decimal actualAmount)
    {
        ActualAmount = actualAmount;
        return this;
    }

    /// <summary>
    /// The variance for this line (Budgeted - Actual).
    /// </summary>
    public decimal GetVariance()
    {
        return BudgetedAmount - ActualAmount;
    }

    /// <summary>
    /// The percentage variance relative to the budgeted amount.
    /// </summary>
    public decimal GetVariancePercentage()
    {
        return BudgetedAmount > 0 ? ((BudgetedAmount - ActualAmount) / BudgetedAmount) * 100 : 0;
    }
}
