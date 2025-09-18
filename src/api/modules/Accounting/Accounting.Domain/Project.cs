using Accounting.Domain.Events.Project;

namespace Accounting.Domain;

/// <summary>
/// Represents a project for job costing and tracking of budget, actual costs, and revenues.
/// </summary>
/// <remarks>
/// Tracks lifecycle (Active, Completed, On Hold, Canceled), client/department metadata, and costing entries.
/// Defaults: <see cref="Status"/> is "Active"; <see cref="ActualCost"/> and <see cref="ActualRevenue"/> start at 0.
/// </remarks>
public class Project : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Project start date.
    /// </summary>
    public DateTime StartDate { get; private set; }

    /// <summary>
    /// Project completion/cancellation date, when applicable.
    /// </summary>
    public DateTime? EndDate { get; private set; }

    /// <summary>
    /// Approved budget amount for the project; must be non-negative.
    /// </summary>
    public decimal BudgetedAmount { get; private set; }

    /// <summary>
    /// Status: Active, Completed, On Hold, or Cancelled.
    /// </summary>
    public string Status { get; private set; } // Active, Completed, On Hold, Canceled

    /// <summary>
    /// Optional end-customer/client name.
    /// </summary>
    public string? ClientName { get; private set; }

    /// <summary>
    /// Optional project manager name.
    /// </summary>
    public string? ProjectManager { get; private set; }

    /// <summary>
    /// Owning department.
    /// </summary>
    public string? Department { get; private set; }

    /// <summary>
    /// Accumulated actual costs from job costing entries (positive amounts).
    /// </summary>
    public decimal ActualCost { get; private set; }

    /// <summary>
    /// Accumulated actual revenues (sum of negative amount entries categorized as revenue).
    /// </summary>
    public decimal ActualRevenue { get; private set; }
    
    private Project()
    {
        // EF Core requires a parameterless constructor for entity instantiation
    }

    private readonly List<JobCostingEntry> _costingEntries = new();
    /// <summary>
    /// Cost and revenue entries associated with this project.
    /// </summary>
    public IReadOnlyCollection<JobCostingEntry> CostingEntries => _costingEntries.AsReadOnly();

    private Project(string projectName, DateTime startDate, decimal budgetedAmount,
        string? clientName = null, string? projectManager = null, string? department = null,
        string? description = null, string? notes = null)
    {
        Name = projectName.Trim();
        StartDate = startDate;
        BudgetedAmount = budgetedAmount;
        Status = "Active";
        ClientName = clientName?.Trim();
        ProjectManager = projectManager?.Trim();
        Department = department?.Trim();
        ActualCost = 0;
        ActualRevenue = 0;
        Description = description?.Trim();
        Notes = notes?.Trim();

        QueueDomainEvent(new ProjectCreated(Id, Name, StartDate, BudgetedAmount, Description, Notes));
    }

    /// <summary>
    /// Create a project; budget must be non-negative.
    /// </summary>
    public static Project Create(string projectName, DateTime startDate, decimal budgetedAmount,
        string? clientName = null, string? projectManager = null, string? department = null,
        string? description = null, string? notes = null)
    {
        if (budgetedAmount < 0)
            throw new InvalidProjectBudgetException();

        return new Project(projectName, startDate, budgetedAmount, clientName, projectManager, department, description, notes);
    }

    /// <summary>
    /// Update project metadata and figures; validates non-negative budgets.
    /// </summary>
    public Project Update(string? projectName, DateTime? startDate, DateTime? endDate, decimal? budgetedAmount,
        string? status, string? clientName, string? projectManager, string? department,
        string? description, string? notes)
    {
        bool isUpdated = false;

        if (!string.IsNullOrWhiteSpace(projectName) && Name != projectName)
        {
            Name = projectName.Trim();
            isUpdated = true;
        }

        if (startDate.HasValue && StartDate != startDate.Value)
        {
            StartDate = startDate.Value;
            isUpdated = true;
        }

        if (endDate != EndDate)
        {
            EndDate = endDate;
            isUpdated = true;
        }

        if (budgetedAmount.HasValue && BudgetedAmount != budgetedAmount.Value)
        {
            if (budgetedAmount.Value < 0)
                throw new InvalidProjectBudgetException();
            BudgetedAmount = budgetedAmount.Value;
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(status) && Status != status)
        {
            Status = status.Trim();
            isUpdated = true;
        }

        if (clientName != ClientName)
        {
            ClientName = clientName?.Trim();
            isUpdated = true;
        }

        if (projectManager != ProjectManager)
        {
            ProjectManager = projectManager?.Trim();
            isUpdated = true;
        }

        if (department != Department)
        {
            Department = department?.Trim();
            isUpdated = true;
        }

        if (description != Description)
        {
            Description = description?.Trim();
            isUpdated = true;
        }

        if (notes != Notes)
        {
            Notes = notes?.Trim();
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new ProjectUpdated(this));
        }

        return this;
    }

    /// <summary>
    /// Add a cost entry; amount must be positive. Not allowed when project is Completed or Cancelled.
    /// </summary>
    public Project AddCostEntry(DateTime date, string description, decimal amount, DefaultIdType expenseAccountId,
        DefaultIdType? journalEntryId = null, string? category = null)
    {
        if (Status == "Completed" || Status == "Cancelled")
            throw new ProjectCannotBeModifiedException(Id);

        if (amount <= 0)
            throw new InvalidProjectCostEntryException();

        var entry = JobCostingEntry.Create(Id, date, description, amount, expenseAccountId, journalEntryId, category);
        _costingEntries.Add(entry);

        ActualCost += amount;
        QueueDomainEvent(new ProjectCostAdded(Id, amount, ActualCost));
        return this;
    }

    /// <summary>
    /// Add a revenue entry; amount must be positive. Not allowed when project is Completed or Cancelled.
    /// </summary>
    public Project AddRevenueEntry(DateTime date, string description, decimal amount, DefaultIdType revenueAccountId,
        DefaultIdType? journalEntryId = null)
    {
        if (Status == "Completed" || Status == "Cancelled")
            throw new ProjectCannotBeModifiedException(Id);

        if (amount <= 0)
            throw new InvalidProjectRevenueEntryException();

        var entry = JobCostingEntry.Create(Id, date, description, -amount, revenueAccountId, journalEntryId, "Revenue");
        _costingEntries.Add(entry);

        ActualRevenue += amount;
        QueueDomainEvent(new ProjectRevenueAdded(Id, amount, ActualRevenue));
        return this;
    }

    /// <summary>
    /// Mark project as Completed and set <see cref="EndDate"/>.
    /// </summary>
    public Project Complete(DateTime completionDate)
    {
        if (Status == "Completed")
            throw new ProjectAlreadyCompletedException(Id);

        Status = "Completed";
        EndDate = completionDate;
        QueueDomainEvent(new ProjectCompleted(Id, completionDate, ActualCost, ActualRevenue));
        return this;
    }

    /// <summary>
    /// Cancel project and set <see cref="EndDate"/> with a reason (logged in event).
    /// </summary>
    public Project Cancel(DateTime cancellationDate, string reason)
    {
        if (Status == "Completed" || Status == "Cancelled")
            throw new ProjectAlreadyCancelledException(Id);

        Status = "Cancelled";
        EndDate = cancellationDate;
        QueueDomainEvent(new ProjectCancelled(Id, cancellationDate, reason));
        return this;
    }

    /// <summary>
    /// Difference between budget and actual cost (positive = under budget).
    /// </summary>
    public decimal GetBudgetVariance()
    {
        return BudgetedAmount - ActualCost;
    }

    /// <summary>
    /// Profit/Loss calculated as ActualRevenue - ActualCost.
    /// </summary>
    public decimal GetProfitLoss()
    {
        return ActualRevenue - ActualCost;
    }

    /// <summary>
    /// Percentage of budget consumed by actual costs.
    /// </summary>
    public decimal GetBudgetUtilizationPercentage()
    {
        return BudgetedAmount > 0 ? (ActualCost / BudgetedAmount) * 100 : 0;
    }
}

/// <summary>
/// Represents a single job costing entry for a project, either cost (positive) or revenue (negative).
/// </summary>
public class JobCostingEntry : BaseEntity
{
    /// <summary>
    /// Parent project identifier.
    /// </summary>
    public DefaultIdType ProjectId { get; private set; }

    /// <summary>
    /// Date of the cost/revenue entry.
    /// </summary>
    public DateTime Date { get; private set; }

    /// <summary>
    /// Description of the entry.
    /// </summary>
    public string Description { get; private set; }

    /// <summary>
    /// Amount (positive for cost, negative for revenue).
    /// </summary>
    public decimal Amount { get; private set; }

    /// <summary>
    /// Related GL account identifier.
    /// </summary>
    public DefaultIdType AccountId { get; private set; }

    /// <summary>
    /// Optional link to the journal entry that recorded this transaction.
    /// </summary>
    public DefaultIdType? JournalEntryId { get; private set; }

    /// <summary>
    /// Optional category text (e.g., Revenue when created via AddRevenueEntry).
    /// </summary>
    public string? Category { get; private set; }

    private JobCostingEntry(DefaultIdType projectId, DateTime date, string description,
        decimal amount, DefaultIdType accountId, DefaultIdType? journalEntryId = null, string? category = null)
    {
        ProjectId = projectId;
        Date = date;
        Description = description.Trim();
        Amount = amount;
        AccountId = accountId;
        JournalEntryId = journalEntryId;
        Category = category?.Trim();
    }

    /// <summary>
    /// Create a job costing entry.
    /// </summary>
    public static JobCostingEntry Create(DefaultIdType projectId, DateTime date, string description,
        decimal amount, DefaultIdType accountId, DefaultIdType? journalEntryId = null, string? category = null)
    {
        return new JobCostingEntry(projectId, date, description, amount, accountId, journalEntryId, category);
    }

    /// <summary>
    /// Update fields of the costing entry.
    /// </summary>
    public JobCostingEntry Update(DateTime? date, string? description, decimal? amount, string? category)
    {
        if (date.HasValue && Date != date.Value)
        {
            Date = date.Value;
        }

        if (!string.IsNullOrWhiteSpace(description) && Description != description)
        {
            Description = description.Trim();
        }

        if (amount.HasValue && Amount != amount.Value)
        {
            Amount = amount.Value;
        }

        if (category != Category)
        {
            Category = category?.Trim();
        }

        return this;
    }
}